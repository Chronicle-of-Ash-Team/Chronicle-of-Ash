using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Component to store original HDRP intensity for each light
public class LightIntensityData : MonoBehaviour
{
    [SerializeField] public float originalHdrpIntensity; // Stores the original HDRP intensity
}

public class LightIntensityConverter : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)] private float urpIntensity = 0.01f; // Slider for URP multiplier
    [SerializeField, Range(0f, 200f)] private float hdrpIntensity = 100.0f; // Slider for HDRP multiplier
    [SerializeField] private LightIntensityDefaults defaultIntensities; // ScriptableObject for default intensities

    void Start()
    {
        AdjustLightsForPipeline();
    }

    void OnValidate()
    {
        // Ensure the ScriptableObject is assigned or created
        if (defaultIntensities == null)
        {
            defaultIntensities = CreateDefaultIntensitiesAsset();
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }
    }

    public void AdjustLightsForPipeline()
    {
        // Detect the current render pipeline
        var pipeline = GraphicsSettings.currentRenderPipeline;
        bool isURP = pipeline != null && pipeline.GetType().Name.Contains("Universal");

        // Debug log to track pipeline detection
        Debug.Log($"Running AdjustLightsForPipeline for {(isURP ? "URP" : "HDRP")} pipeline");

        // Ensure default intensities asset exists
        if (defaultIntensities == null)
        {
            defaultIntensities = CreateDefaultIntensitiesAsset();
        }

        // Adjust intensity for all lights
        foreach (var light in FindObjectsOfType<Light>())
        {
            // Get or add LightIntensityData to store original HDRP intensity
            LightIntensityData intensityData = light.GetComponent<LightIntensityData>();
            string lightId = GetLightId(light);
            bool hasDefault = defaultIntensities.TryGetDefaultIntensity(lightId, out float defaultIntensity);

            if (intensityData == null)
            {
                intensityData = light.gameObject.AddComponent<LightIntensityData>();
                // Set original intensity: use default if exists, else current intensity
                intensityData.originalHdrpIntensity = hasDefault ? defaultIntensity : light.intensity;
                // Save to ScriptableObject only if no default exists (first time)
                if (!hasDefault)
                {
                    defaultIntensities.SaveDefaultIntensity(lightId, light.intensity);
                    Debug.Log($"Stored default HDRP intensity for {light.name}: {light.intensity}");
                    #if UNITY_EDITOR
                    EditorUtility.SetDirty(defaultIntensities);
                    #endif
                }
            }

            // Set intensity based on pipeline
            float multiplier = isURP ? urpIntensity : hdrpIntensity / 100f;
            float targetIntensity = intensityData.originalHdrpIntensity * multiplier;
            light.intensity = targetIntensity;

            // Mark light as dirty to ensure rendering update (Unity 2022.3+)
            light.SetLightDirty();

            // Debug log to confirm intensity change
            Debug.Log($"Set {light.name} ({light.type}) intensity to {targetIntensity} (original HDRP: {intensityData.originalHdrpIntensity}, multiplier: {multiplier})");
        }

        // Force rendering refresh
        ForceRenderRefresh();
    }

    public void RevertToHdrpIntensities()
    {
        Debug.Log("Running RevertToHdrpIntensities");

        foreach (var light in FindObjectsOfType<Light>())
        {
            LightIntensityData intensityData = light.GetComponent<LightIntensityData>();
            if (intensityData != null && intensityData.originalHdrpIntensity > 0)
            {
                light.intensity = intensityData.originalHdrpIntensity;
                light.SetLightDirty();
                Debug.Log($"Reverted {light.name} ({light.type}) intensity to {intensityData.originalHdrpIntensity}");
            }
            else
            {
                Debug.LogWarning($"No HDRP intensity stored for {light.name}; skipping revert");
            }
        }

        // Reset HDRP intensity slider to 100
        hdrpIntensity = 100.0f;
        #if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        #endif

        // Force rendering refresh
        ForceRenderRefresh();
    }

    public void ClearLightIntensityData()
    {
        Debug.Log("Running ClearLightIntensityData");

        foreach (var light in FindObjectsOfType<Light>())
        {
            LightIntensityData intensityData = light.GetComponent<LightIntensityData>();
            if (intensityData != null)
            {
                DestroyImmediate(intensityData);
                Debug.Log($"Removed LightIntensityData from {light.name}");
            }
        }

        // Force rendering refresh
        ForceRenderRefresh();
    }

    public void RestoreDefaultIntensities()
    {
        Debug.Log("Running RestoreDefaultIntensities");

        if (defaultIntensities == null)
        {
            Debug.LogWarning("No default intensities asset found; cannot restore defaults");
            return;
        }

        foreach (var light in FindObjectsOfType<Light>())
        {
            string lightId = GetLightId(light);
            if (defaultIntensities.TryGetDefaultIntensity(lightId, out float defaultIntensity))
            {
                LightIntensityData intensityData = light.GetComponent<LightIntensityData>();
                if (intensityData == null)
                {
                    intensityData = light.gameObject.AddComponent<LightIntensityData>();
                }
                intensityData.originalHdrpIntensity = defaultIntensity;
                light.intensity = defaultIntensity;
                light.SetLightDirty();
                Debug.Log($"Restored default HDRP intensity for {light.name}: {defaultIntensity}");
                #if UNITY_EDITOR
                EditorUtility.SetDirty(intensityData);
                #endif
            }
            else
            {
                Debug.LogWarning($"No default intensity found for {light.name}; skipping restore");
            }
        }

        // Reset HDRP intensity slider to 100
        hdrpIntensity = 100.0f;
        #if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        #endif

        // Force rendering refresh
        ForceRenderRefresh();
    }

    private string GetLightId(Light light)
    {
        // Use GameObject name as a unique identifier
        return light.gameObject.name;
    }

    private LightIntensityDefaults CreateDefaultIntensitiesAsset()
    {
        #if UNITY_EDITOR
        LightIntensityDefaults asset = ScriptableObject.CreateInstance<LightIntensityDefaults>();
        string path = "Assets/LightIntensityDefaults.asset";
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();
        Debug.Log($"Created LightIntensityDefaults asset at {path}");
        return asset;
        #else
        return null;
        #endif
    }

    private void ForceRenderRefresh()
    {
        #if UNITY_EDITOR
        // Mark scene as dirty and repaint views in Editor
        if (UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene() != null)
        {
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
        }
        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        Debug.Log("Forced rendering refresh in Editor");
        #endif
    }

    // Run in Editor via context menu (as backup)
    [ContextMenu("Adjust Lights for Current Pipeline")]
    private void AdjustLightsInEditor()
    {
        Debug.Log("Running AdjustLightsInEditor via context menu");
        AdjustLightsForPipeline();
    }

    [ContextMenu("Revert to HDRP Intensities")]
    private void RevertToHdrpInEditor()
    {
        Debug.Log("Running RevertToHdrpInEditor via context menu");
        RevertToHdrpIntensities();
    }

    [ContextMenu("Clear Light Intensity Data")]
    private void ClearLightIntensityDataInEditor()
    {
        Debug.Log("Running ClearLightIntensityDataInEditor via context menu");
        ClearLightIntensityData();
    }

    [ContextMenu("Restore Default HDRP Intensities")]
    private void RestoreDefaultIntensitiesInEditor()
    {
        Debug.Log("Running RestoreDefaultIntensitiesInEditor via context menu");
        RestoreDefaultIntensities();
    }
}