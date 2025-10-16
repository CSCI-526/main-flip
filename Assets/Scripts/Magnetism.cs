using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private List<Rigidbody2D> attractedObjects = new List<Rigidbody2D>();
    private List<GameObject> collidingMagnets = new List<GameObject>();

    void FixedUpdate()
    {
        UpdateMagneticForce();
        UpdateColor();
    }

    void UpdateMagneticForce()
    {
        if (currentPole == MagneticPole.None) return;

        foreach (Rigidbody2D rb in attractedObjects) {
            if (rb == null) continue; // skip null references
            if (rb.gameObject == this.gameObject) continue; // skip self
            if (!rb.CompareTag("Player")) continue; // only affect the player

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
                if (currentPole == otherMagnetism.currentPole)
                    rb.AddForce(-direction.normalized * currentMagnetismStrength, ForceMode2D.Force);
                else
                    rb.AddForce(direction.normalized * currentMagnetismStrength, ForceMode2D.Force);
            }
        }
    }

    void UpdateColor()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;
        Color newColor;
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

    void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null && !attractedObjects.Contains(rb))
            attractedObjects.Add(rb);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
            attractedObjects.Remove(rb);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Magnetism otherMagnetism = collision.gameObject.GetComponent<Magnetism>();
        if (otherMagnetism != null && !collidingMagnets.Contains(collision.gameObject))
            collidingMagnets.Add(collision.gameObject);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        Magnetism otherMagnetism = collision.gameObject.GetComponent<Magnetism>();
        if (otherMagnetism != null)
            collidingMagnets.Remove(collision.gameObject);
    }
}



