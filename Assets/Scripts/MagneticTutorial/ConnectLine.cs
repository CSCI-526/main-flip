using UnityEngine;

public class ConnectLine : MonoBehaviour
{
    public Rigidbody2D obj1, obj2;
    public Color lineColor;
    public Color arrowColor;
    public bool isAttracting;
    
    private GameObject connctionLine, arrowLineA, arrowLineB, arrowArrowA, arrowArrowB;
    private LineRenderer connectionLineRenderer, arrowLineRendererA, arrowLineRendererB;
    private SpriteRenderer arrowArrowRendererA, arrowArrowRendererB;
    
    public Texture2D dashTexture;
    public float dashesPerUnit = 2f;
    public Sprite triangleSprite;
    

    void Start()
    {
        connctionLine = new GameObject("ConnectionLine");
        arrowLineA = new GameObject("ArrowLineA");
        arrowLineB = new GameObject("ArrowLineB");
        arrowArrowA = new GameObject("ArrowArrowA_"+this.gameObject.name);
        arrowArrowB = new GameObject("ArrowArrowB_"+this.gameObject.name);
        connctionLine.transform.parent = this.transform;
        arrowLineA.transform.parent = this.transform;
        arrowLineB.transform.parent = this.transform;
        arrowArrowA.transform.parent = this.transform;
        arrowArrowB.transform.parent = this.transform;

        Shader connctionLineShader = Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply");
        Material connctionLineMaterial = new Material(connctionLineShader);
        dashTexture = Resources.Load<Texture2D>("dotline_8x16.drawio");
        connctionLineMaterial.mainTexture = dashTexture;

        connectionLineRenderer = GetComponent<LineRenderer>();
        if (connectionLineRenderer == null)
        {
            connectionLineRenderer = connctionLine.AddComponent<LineRenderer>();
        }
        connectionLineRenderer.sortingLayerName = "Foreground";
        connectionLineRenderer.sortingOrder = 100;
        connectionLineRenderer.positionCount = 0;
        connectionLineRenderer.useWorldSpace = true;
        connectionLineRenderer.widthMultiplier = 0.1f;
        connectionLineRenderer.material = connctionLineMaterial;

        Shader arrowLineShader = Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply");
        Material arrowLineMaterial = new Material(arrowLineShader);        

        arrowLineRendererA = GetComponent<LineRenderer>();
        if (arrowLineRendererA == null)
        {
            arrowLineRendererA = arrowLineA.AddComponent<LineRenderer>();
        }
        arrowLineRendererA.sortingLayerName = "Foreground";
        arrowLineRendererA.sortingOrder = 100;
        arrowLineRendererA.positionCount = 0;
        arrowLineRendererA.useWorldSpace = true;
        arrowLineRendererA.widthMultiplier = 0.1f;
        arrowLineRendererA.material = arrowLineMaterial;

        arrowLineRendererB = GetComponent<LineRenderer>();
        if (arrowLineRendererB == null)
        {
            arrowLineRendererB = arrowLineB.AddComponent<LineRenderer>();
        }
        arrowLineRendererB.sortingLayerName = "Foreground";
        arrowLineRendererB.sortingOrder = 100;
        arrowLineRendererB.positionCount = 0;
        arrowLineRendererB.useWorldSpace = true;
        arrowLineRendererB.widthMultiplier = 0.1f;
        arrowLineRendererB.material = arrowLineMaterial;

        triangleSprite = Resources.Load<Sprite>("Triangle");
        if (arrowArrowRendererA == null)
        {
            arrowArrowRendererA = arrowArrowA.AddComponent<SpriteRenderer>();
        }
        if (arrowArrowRendererA != null)
        {
            arrowArrowRendererA.sortingLayerName = "Foreground";
            arrowArrowRendererA.sortingOrder = 100;
        }
        if (arrowArrowRendererB == null)
        {
            arrowArrowRendererB = arrowArrowB.AddComponent<SpriteRenderer>();
        }
        if (arrowArrowRendererB != null)
        {
            arrowArrowRendererB.sortingLayerName = "Foreground";
            arrowArrowRendererB.sortingOrder = 100;
        }

        Shader spriteShader = Shader.Find("Sprites/Default");
        if (spriteShader == null) {
            spriteShader = Shader.Find("Legacy Shaders/Sprites/Default"); 
        }

        if (spriteShader != null)
        {
            Material unlitSpriteMaterial = new Material(spriteShader);
            if (arrowArrowRendererA != null)
            {
                arrowArrowRendererA.material = unlitSpriteMaterial;
            }
            if (arrowArrowRendererB != null)
            {
                arrowArrowRendererB.material = unlitSpriteMaterial;
            }
        }
    }

    void Update()
    {
        if (obj1 != null && obj2 != null)
        {
            connectionLineRenderer.positionCount = 2;
            connectionLineRenderer.SetPosition(0, obj1.position);
            connectionLineRenderer.SetPosition(1, obj2.position);
            connectionLineRenderer.startColor = lineColor;
            connectionLineRenderer.endColor = lineColor;

            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(lineColor, 0.0f), new GradientColorKey(lineColor, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
            );
            connectionLineRenderer.colorGradient = gradient;

            Vector2 direction = (obj2.position - obj1.position).normalized;
            float distance = Vector2.Distance(obj1.position, obj2.position);
            connectionLineRenderer.material.mainTextureScale = new Vector2(distance * dashesPerUnit, 1);

            if (isAttracting)
            {
                arrowLineRendererA.positionCount = 2;
                arrowLineRendererB.positionCount = 2;

                arrowLineRendererA.SetPosition(0, obj1.position);
                arrowLineRendererA.SetPosition(1, obj1.position + direction * Mathf.Min(distance / 3f, 1f));

                arrowLineRendererB.SetPosition(0, obj2.position);
                arrowLineRendererB.SetPosition(1, obj2.position - direction * Mathf.Min(distance / 3f, 1f));

                Gradient arrowGradient = new Gradient();
                arrowGradient.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(lineColor, 0.0f), new GradientColorKey(lineColor, 1.0f) },
                    new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
                );
                arrowLineRendererA.colorGradient = arrowGradient;
                arrowLineRendererB.colorGradient = arrowGradient;

                triangleSprite = Resources.Load<Sprite>("Triangle");
                arrowArrowRendererA.sprite = triangleSprite;
                arrowArrowRendererB.sprite = triangleSprite;
                arrowArrowRendererA.transform.position = obj1.position + direction * Mathf.Min(distance / 3f, 1f);
                arrowArrowRendererB.transform.position = obj2.position - direction * Mathf.Min(distance / 3f, 1f);
                float angleA = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                float angleB = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
                float arrowAOffset = 90f;
                float arrowBOffset = -90f;
                arrowArrowRendererA.transform.rotation = Quaternion.Euler(0, 0, -90);
                arrowArrowRendererB.transform.rotation = Quaternion.Euler(0, 0, 90);

                arrowArrowRendererA.transform.localScale = new Vector3(0.12f, 1f, 1f);
                arrowArrowRendererB.transform.localScale = new Vector3(0.12f, 1f, 1f);
                arrowArrowRendererA.color = arrowColor;
                arrowArrowRendererB.color = arrowColor;
            }
            else
            {
                arrowLineRendererA.positionCount = 2;
                arrowLineRendererB.positionCount = 2;
                
                arrowLineRendererA.SetPosition(0, obj1.position);
                arrowLineRendererA.SetPosition(1, obj1.position - direction * Mathf.Min(distance / 3f, 1f));

                arrowLineRendererB.SetPosition(0, obj2.position);
                arrowLineRendererB.SetPosition(1, obj2.position + direction * Mathf.Min(distance / 3f, 1f));

                Gradient arrowGradient = new Gradient();
                arrowGradient.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(lineColor, 0.0f), new GradientColorKey(lineColor, 1.0f) },
                    new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
                );
                arrowLineRendererA.colorGradient = arrowGradient;
                arrowLineRendererB.colorGradient = arrowGradient;

                triangleSprite = Resources.Load<Sprite>("Triangle");
                arrowArrowRendererA.sprite = triangleSprite;
                arrowArrowRendererB.sprite = triangleSprite;
                arrowArrowRendererA.transform.position = obj1.position - direction * Mathf.Min(distance / 3f, 1f);
                arrowArrowRendererB.transform.position = obj2.position + direction * Mathf.Min(distance / 3f, 1f);
                float angleA = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                float angleB = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
                float arrowAOffset = -90f;
                float arrowBOffset = 90f;
                arrowArrowRendererA.transform.rotation = Quaternion.Euler(0, 0, 90);
                arrowArrowRendererB.transform.rotation = Quaternion.Euler(0, 0, -90);

                arrowArrowRendererA.transform.localScale = new Vector3(0.12f, 1f, 1f);
                arrowArrowRendererB.transform.localScale = new Vector3(0.12f, 1f, 1f);
                arrowArrowRendererA.color = arrowColor;
                arrowArrowRendererB.color = arrowColor;
            }
        }
        else
        {
            connectionLineRenderer.positionCount = 0;
        }
    }
}
