using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MagnetismAxis
{
    None,
    X,
    Y
}

public enum MagnetismMode
{
    Radial,
    Axial
}

public enum MagneticPole
{
    None,
    North,
    South
}

public class Magnetism : MonoBehaviour
{
    public float maxMagnetismStrength = 100f;
    public float minMagnetismStrength = 100f;
    public bool changeableMode = true;
    public MagneticPole currentPole;
    public MagnetismMode currentMode;
    public MagnetismAxis currentAxis;

    public List<Rigidbody2D> attractedObjects = new List<Rigidbody2D>();
    public List<GameObject> collidingMagnets = new List<GameObject>();

    private GameObject connctionLine, arrowLineA, arrowLineB, arrowArrowA, arrowArrowB;
    private LineRenderer connectionLineRenderer, arrowLineRendererA, arrowLineRendererB;
    private SpriteRenderer arrowArrowRendererA, arrowArrowRendererB;
    private Color attractColor = Color.green;
    private Color repelColor = Color.red;
    public Texture2D dashTexture;
    public float dashesPerUnit = 5f;

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

        connectionLineRenderer = GetComponent<LineRenderer>();
        if (connectionLineRenderer == null && !this.gameObject.name.Contains("Player"))
        {
            connectionLineRenderer = connctionLine.AddComponent<LineRenderer>();
        }

        if (connectionLineRenderer != null)
        {
            connectionLineRenderer.sortingLayerName = "Foreground";
            connectionLineRenderer.sortingOrder = 100;
        
            connectionLineRenderer.positionCount = 0;
            connectionLineRenderer.useWorldSpace = true;
            connectionLineRenderer.widthMultiplier = 0.1f;

            Shader lineShader = Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply");
            Material lineMaterial = new Material(lineShader);
            if (dashTexture != null)
            {
                lineMaterial.mainTexture = dashTexture;
            }
            else {
                dashTexture = Resources.Load<Texture2D>("dotline_8x16.drawio");
                lineMaterial.mainTexture = dashTexture;
            }
            connectionLineRenderer.material = lineMaterial;
        }

        arrowLineRendererA = GetComponent<LineRenderer>();
        if (arrowLineRendererA == null && !this.gameObject.name.Contains("Player"))
        {
            arrowLineRendererA = arrowLineA.AddComponent<LineRenderer>();
        }
        if (arrowLineRendererA != null)
        {
            arrowLineRendererA.sortingLayerName = "Foreground";
            arrowLineRendererA.sortingOrder = 100;
        
            arrowLineRendererA.positionCount = 0;
            arrowLineRendererA.useWorldSpace = true;
            arrowLineRendererA.widthMultiplier = 0.1f;

            Shader lineShader = Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply");
            Material lineMaterial = new Material(lineShader);
            arrowLineRendererA.material = lineMaterial;
        }
        
        arrowLineRendererB = GetComponent<LineRenderer>();
        if (arrowLineRendererB == null && !this.gameObject.name.Contains("Player"))
        {
            arrowLineRendererB = arrowLineB.AddComponent<LineRenderer>();
        }
        if (arrowLineRendererB != null)
        {
            arrowLineRendererB.sortingLayerName = "Foreground";
            arrowLineRendererB.sortingOrder = 100;
        
            arrowLineRendererB.positionCount = 0;
            arrowLineRendererB.useWorldSpace = true;
            arrowLineRendererB.widthMultiplier = 0.1f;

            Shader lineShader = Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply");
            Material lineMaterial = new Material(lineShader);
            arrowLineRendererB.material = lineMaterial;
        }

        if (arrowArrowRendererA == null && !this.gameObject.name.Contains("Player"))
        {
            arrowArrowRendererA = arrowArrowA.AddComponent<SpriteRenderer>();
        }
        if (arrowArrowRendererA != null)
        {
            arrowArrowRendererA.sortingLayerName = "Foreground";
            arrowArrowRendererA.sortingOrder = 100;
        }

        if (arrowArrowRendererB == null && !this.gameObject.name.Contains("Player"))
        {
            arrowArrowRendererB = arrowArrowB.AddComponent<SpriteRenderer>();
        }
        if (arrowArrowRendererB != null)
        {
            arrowArrowRendererB.sortingLayerName = "Foreground";
            arrowArrowRendererB.sortingOrder = 100;
        }
    }

    void FixedUpdate()
    {
        UpdateMagneticForce();
    }

    void Update()
    {
        UpdateColor();
        UpdateConnectionLine();
    }

    void UpdateMagneticForce()
    {
        if (currentPole == MagneticPole.None) return;

        foreach (Rigidbody2D rb in attractedObjects) {
            if (rb == null) continue; // skip null references
            if (rb.gameObject == this.gameObject) continue; // skip self
            if (!rb.tag.Contains("Player")) continue; // only affect the player

            Magnetism otherMagnetism = rb.GetComponent<Magnetism>();
            if (otherMagnetism != null && otherMagnetism.currentPole != MagneticPole.None && currentPole != MagneticPole.None) {
                Vector2 direction = (Vector2)transform.position - rb.position;
                MagnetismMode effectiveMode = currentMode;

                
                if (collidingMagnets.Contains(rb.gameObject) && changeableMode)
                {
                    effectiveMode = MagnetismMode.Radial;
                }

                if (effectiveMode == MagnetismMode.Axial)
                {
                    if (currentAxis == MagnetismAxis.None)
                    {
                        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                            direction.x = 0;
                        else
                            direction.y = 0;
                    }
                    else if (currentAxis == MagnetismAxis.X)
                        direction.y = 0;
                    else if (currentAxis == MagnetismAxis.Y)
                        direction.x = 0;
                }
                
                float currentMagnetismStrength = maxMagnetismStrength;
                float distance = direction.magnitude;
                currentMagnetismStrength /= (distance * distance);
                currentMagnetismStrength = Mathf.Max(minMagnetismStrength, Mathf.Min(currentMagnetismStrength, maxMagnetismStrength));
                
                bool isAttracting = (currentPole != otherMagnetism.currentPole);
                
                if (!isAttracting)
                {
                    rb.AddForce(-direction.normalized * currentMagnetismStrength, ForceMode2D.Force);
                }
                else
                {
                    rb.AddForce(direction.normalized * currentMagnetismStrength, ForceMode2D.Force);
                }
            }
        }
    }

    void UpdateConnectionLine()
    {
        if (currentPole == MagneticPole.None) return;

        foreach (Rigidbody2D rb in attractedObjects) {
            if (rb == null) continue; // skip null references
            if (rb.gameObject == this.gameObject) continue; // skip self
            if (!rb.tag.Contains("Player")) continue; // only affect the player

            Magnetism otherMagnetism = rb.GetComponent<Magnetism>();
            if (otherMagnetism != null && otherMagnetism.currentPole != MagneticPole.None && currentPole != MagneticPole.None) {
                Vector2 direction = (Vector2)transform.position - rb.position;
                MagnetismMode effectiveMode = currentMode;

                
                if (collidingMagnets.Contains(rb.gameObject) && changeableMode)
                {
                    effectiveMode = MagnetismMode.Radial;
                }

                if (effectiveMode == MagnetismMode.Axial)
                {
                    if (currentAxis == MagnetismAxis.None)
                    {
                        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                            direction.x = 0;
                        else
                            direction.y = 0;
                    }
                    else if (currentAxis == MagnetismAxis.X)
                        direction.y = 0;
                    else if (currentAxis == MagnetismAxis.Y)
                        direction.x = 0;
                }

                float currentMagnetismStrength = maxMagnetismStrength;
                float distance = direction.magnitude;
                currentMagnetismStrength /= (distance * distance);
                currentMagnetismStrength = Mathf.Max(minMagnetismStrength, Mathf.Min(currentMagnetismStrength, maxMagnetismStrength));
                bool isAttracting = (currentPole != otherMagnetism.currentPole);
                drawConnectionLine(rb, isAttracting, currentMagnetismStrength, distance);
            }
        }
    }

    void drawConnectionLine(Rigidbody2D rb, bool isAttracting, float currentMagnetismStrength, float distance)
    {
        if (connectionLineRenderer != null)
        {
            connectionLineRenderer.positionCount = 2;
            connectionLineRenderer.SetPosition(0, transform.position);
            connectionLineRenderer.SetPosition(1, rb.position);

            Color currColor = isAttracting ? attractColor : repelColor;
            connectionLineRenderer.startColor = currColor;
            connectionLineRenderer.endColor = currColor;

            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(currColor, 0.0f), new GradientColorKey(currColor, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
            );
            connectionLineRenderer.colorGradient = gradient;

            if (dashTexture != null)
            {
                connectionLineRenderer.material.mainTextureScale = new Vector2(distance * dashesPerUnit, 1f);
            }

            connectionLineRenderer.widthMultiplier = Mathf.Max(0.001f * currentMagnetismStrength, 0.1f);
        }

        if (arrowLineRendererA != null)
        {
            arrowLineRendererA.positionCount = 2;
            Vector2 direction = (rb.position - (Vector2)transform.position).normalized;
            Vector2 startPos = transform.position;
            Vector2 endPos = startPos;
            if (isAttracting)
                endPos += direction * (distance / 3.0f);
            else
                endPos -= direction * (distance / 3.0f);
            arrowLineRendererA.SetPosition(0, startPos);
            arrowLineRendererA.SetPosition(1, endPos);

            Color currColor = isAttracting ? attractColor : repelColor;
            arrowLineRendererA.startColor = currColor;
            arrowLineRendererA.endColor = currColor;

            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(currColor, 0.0f), new GradientColorKey(currColor, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
            );
            arrowLineRendererA.colorGradient = gradient;
            arrowLineRendererA.widthMultiplier = 0.001f * currentMagnetismStrength;

            if (arrowArrowRendererA != null)
            {
                Sprite loadedSprite = Resources.Load<Sprite>("Triangle");
                arrowArrowRendererA.sprite = loadedSprite;
                arrowArrowRendererA.transform.position = endPos;
                
                float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                float effectAngle = isAttracting ? -90f : 90f;
                float finalAngle = targetAngle+effectAngle;
                arrowArrowRendererA.transform.rotation = Quaternion.Euler(0, 0, finalAngle);

                //arrowArrowRendererA.transform.localScale = new Vector3(Mathf.Max(0.005f * currentMagnetismStrength, 0.5f), Mathf.Max(0.01f * currentMagnetismStrength, 1f), Mathf.Max(0.01f * currentMagnetismStrength, 1f));
                arrowArrowRendererA.transform.localScale = new Vector3(0.5f, 1f, 1f);
                arrowArrowRendererA.color = currColor;
            }
        }

        if (arrowLineRendererB != null)
        {
            arrowLineRendererB.positionCount = 2;
            Vector2 direction = (rb.position - (Vector2)transform.position).normalized;
            Vector2 startPos = rb.position;
            Vector2 endPos = startPos;
            if (isAttracting)
                endPos -= direction * (distance / 3.0f);
            else
                endPos += direction * (distance / 3.0f);
            arrowLineRendererB.SetPosition(0, startPos);
            arrowLineRendererB.SetPosition(1, endPos);

            Color currColor = isAttracting ? attractColor : repelColor;
            arrowLineRendererB.startColor = currColor;
            arrowLineRendererB.endColor = currColor;

            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(currColor, 0.0f), new GradientColorKey(currColor, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
            );
            arrowLineRendererB.colorGradient = gradient;
            arrowLineRendererB.widthMultiplier = 0.001f * currentMagnetismStrength;

            if (arrowArrowRendererB != null)
            {
                Sprite loadedSprite = Resources.Load<Sprite>("Triangle");
                arrowArrowRendererB.sprite = loadedSprite;
                arrowArrowRendererB.transform.position = endPos;
                
                float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                float effectAngle = isAttracting ? 90f : -90f;
                float finalAngle = targetAngle+effectAngle;
                arrowArrowRendererB.transform.rotation = Quaternion.Euler(0, 0, finalAngle);

                //arrowArrowRendererB.transform.localScale = new Vector3(Mathf.Max(0.005f * currentMagnetismStrength, 0.5f), Mathf.Max(0.01f * currentMagnetismStrength, 1f), Mathf.Max(0.01f * currentMagnetismStrength, 1f));
                arrowArrowRendererB.transform.localScale = new Vector3(0.5f, 1f, 1f);
                arrowArrowRendererB.color = currColor;
            }
        }
    }

    void UpdateColor()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;
        Color newColor;

        if (this.tag.Equals("PseudoPlayer"))
        {
            if (currentPole == MagneticPole.North)
            {
                if (ColorUtility.TryParseHtmlString("#e76a6380", out newColor))
                {
                    sr.color = newColor;
                }
            }
            else if (currentPole == MagneticPole.South)
            {
                if (ColorUtility.TryParseHtmlString("#79A1D880", out newColor))
                {
                    sr.color = newColor;
                }
            }
            else
            {
                sr.color = Color.black;
            }
        }
        if (this.tag.Equals("Player"))
        {
            if (SceneManager.GetActiveScene().name.Contains("level 1"))
            {
                sr.color = Color.white;
                return;
            }

            if (currentPole == MagneticPole.North)
            {
                if (ColorUtility.TryParseHtmlString("#E76963", out newColor))
                {
                    sr.color = newColor;
                }
            }
            else if (currentPole == MagneticPole.South)
            {
                if (ColorUtility.TryParseHtmlString("#79A1D8", out newColor))
                {
                    sr.color = newColor;
                }
            }
            else
            {
                sr.color = Color.black;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null && !attractedObjects.Contains(rb))
            if (rb.gameObject != this.gameObject && rb.tag.Contains("Player"))
                attractedObjects.Add(rb);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            attractedObjects.Remove(rb);
            if (connectionLineRenderer != null)
                connectionLineRenderer.positionCount = 0;
            if (arrowLineRendererA != null)
                arrowLineRendererA.positionCount = 0;
            if (arrowLineRendererB != null)
                arrowLineRendererB.positionCount = 0;
            if (arrowArrowRendererA != null)
                arrowArrowRendererA.sprite = null;
            if (arrowArrowRendererB != null)
                arrowArrowRendererB.sprite = null;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Magnetism otherMagnetism = collision.gameObject.GetComponent<Magnetism>();
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (otherMagnetism != null && !collidingMagnets.Contains(collision.gameObject) && rb.tag.Contains("Player"))
            collidingMagnets.Add(collision.gameObject);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        Magnetism otherMagnetism = collision.gameObject.GetComponent<Magnetism>();
        if (otherMagnetism != null)
            collidingMagnets.Remove(collision.gameObject);
    }
}



