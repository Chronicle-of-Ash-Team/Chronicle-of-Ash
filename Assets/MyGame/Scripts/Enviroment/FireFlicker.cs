using UnityEngine;

public class FireFlicker : MonoBehaviour
{
    private Light fireLight;

    [SerializeField] private float minPercent = 0.3f;

    private float maxPercent = 1f;

    [SerializeField] private float flickerSpeed = 10f;

    private float baseIntensity;

    void Start()
    {
        fireLight = GetComponent<Light>();
        baseIntensity = fireLight.intensity;
    }

    void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
        float percent = Mathf.Lerp(minPercent, maxPercent, noise);

        fireLight.intensity = baseIntensity * percent;
    }
}
