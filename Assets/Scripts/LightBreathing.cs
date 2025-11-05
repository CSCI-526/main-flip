using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(SpriteRenderer))]
public class LightBreathing : MonoBehaviour
{
    [Header("Light Settings")]
    public Light2D spotLight;
    public float minIntensity = 0.1f;
    public float maxIntensity = 1f;
    public float speed = 2f;

    private float t = 0f;

    void Start()
    {
        if (spotLight == null)
        {
            spotLight = GetComponentInChildren<Light2D>();
        }
    }

    void Update()
    {
        if (spotLight == null) return;

        t += Time.deltaTime * speed;
        float pingpong = (Mathf.Sin(t) + 1f) / 2f;

        spotLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, pingpong);

    }
}
