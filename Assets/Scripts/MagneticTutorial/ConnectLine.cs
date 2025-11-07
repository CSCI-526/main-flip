using UnityEngine;

public class ConnectLine : MonoBehaviour
{
    
    public Rigidbody2D obj1, obj2;
    public Color lineColor;
    private LineRenderer lineRenderer;
    

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        lineRenderer.sortingLayerName = "Foreground";
        lineRenderer.sortingOrder = 100;
        
        lineRenderer.positionCount = 0;
        lineRenderer.useWorldSpace = true;
        lineRenderer.widthMultiplier = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (obj1 != null && obj2 != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, obj1.position);
            lineRenderer.SetPosition(1, obj2.position);
            lineRenderer.startColor = lineColor;
            lineRenderer.endColor = lineColor;

            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(lineColor, 0.0f), new GradientColorKey(lineColor, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
            );
            lineRenderer.colorGradient = gradient;
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }
}
