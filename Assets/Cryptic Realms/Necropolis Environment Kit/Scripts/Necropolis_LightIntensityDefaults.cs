using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LightIntensityConverter))]
public class LightIntensityConverterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default Inspector (shows urpIntensity, hdrpIntensity, and defaultIntensities fields)
        DrawDefaultInspector();

        // Add space for clarity
        EditorGUILayout.Space();

        // Add button to adjust lights for current pipeline
        LightIntensityConverter script = (LightIntensityConverter)target;
        if (GUILayout.Button("Adjust Lights for Current Pipeline"))
        {
            Debug.Log("Adjust button clicked in Inspector");
            script.AdjustLightsForPipeline();
            EditorUtility.SetDirty(script); // Mark the component as dirty
        }

        // Add button to revert to HDRP intensities
        if (GUILayout.Button("Revert to HDRP Intensities"))
        {
            Debug.Log("Revert button clicked in Inspector");
            script.RevertToHdrpIntensities();
            EditorUtility.SetDirty(script); // Mark the component as dirty
        }

        // Add button to clear LightIntensityData components
        if (GUILayout.Button("Clear Light Intensity Data"))
        {
            Debug.Log("Clear Light Intensity Data button clicked in Inspector");
            script.ClearLightIntensityData();
            EditorUtility.SetDirty(script); // Mark the component as dirty
        }

        // Add button to restore default HDRP intensities
        if (GUILayout.Button("Restore Default HDRP Intensities"))
        {
            Debug.Log("Restore Default HDRP Intensities button clicked in Inspector");
            script.RestoreDefaultIntensities();
            EditorUtility.SetDirty(script); // Mark the component as dirty
        }
    }
}