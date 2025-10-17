// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public enum MagnetismAxis
// {
//     None,
//     X,
//     Y
// }

// public enum MagnetismMode
// {
//     Radial,
//     Axial
// }

// public enum MagneticPole
// {
//     None,
//     North,
//     South
// }

// public class Magetism : MonoBehaviour
// {
//     public float magetismStrength = 20f;
//     public MagneticPole currentPole;
//     public MagnetismMode currentMode;
//     public MagnetismAxis currentAxis;
//     public float attractionRange = 5f;

//     private List<GameObject> collidingMagnets = new List<GameObject>();

//     void Update()
//     {
//         UpdateMagneticField();
//         UpdateColor();
//     }

//     void UpdateMagneticField()
//     {
//         Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attractionRange);
//         foreach (Collider2D other in colliders)
//         {
//             if (other.gameObject == this.gameObject) continue; // Skip self

//             Magetism otherMagetism = other.GetComponent<Magetism>(); // Skip non-magnetic objects
//             if (otherMagetism != null && otherMagetism.currentPole != MagneticPole.None && currentPole != MagneticPole.None)
//             {
//                 Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
//                 if (rb != null)
//                 {
//                     Vector2 direction = (Vector2)transform.position - rb.position;
//                     MagnetismMode effectiveMode = currentMode;
//                     if (collidingMagnets.Contains(other.gameObject))
//                     {
//                         effectiveMode = MagnetismMode.Radial;
//                     }
//                     if (effectiveMode == MagnetismMode.Axial)
//                     {
//                         if (currentAxis == MagnetismAxis.None)
//                         {
//                             if (direction.x > direction.y)
//                             {
//                                 direction.x = 0; // Ignore X axis
//                             }
//                             else
//                             {
//                                 direction.y = 0; // Ignore Y axis
//                             }
//                         }
//                         else if (currentAxis == MagnetismAxis.X)
//                         {
//                             direction.y = 0; // Ignore Y axis
//                         }
//                         else if (currentAxis == MagnetismAxis.Y)
//                         {
//                             direction.x = 0; // Ignore X axis
//                         }
//                     }

//                     if (currentPole == otherMagetism.currentPole)
//                     {
//                         //Repulsion
//                         rb.AddForce(-direction.normalized * magetismStrength);
//                     }
//                     else
//                     {
//                         //Attraction
//                         rb.AddForce(direction.normalized * magetismStrength);
//                     }
//                 }
//             }

//         }
//     }

//     void UpdateColor()
//     {
//         if (this.currentPole == MagneticPole.North)
//         {
//             this.GetComponent<SpriteRenderer>().color = Color.red;
//         }
//         else if (this.currentPole == MagneticPole.South)
//         {
//             this.GetComponent<SpriteRenderer>().color = Color.blue;
//         }
//         else
//         {
//             this.GetComponent<SpriteRenderer>().color = Color.black;
//         }
//     }

//     void OnCollisionEnter2D(Collision2D collision)
//     {
//         Magetism otherMagetism = collision.gameObject.GetComponent<Magetism>();
//         if (otherMagetism != null && !collidingMagnets.Contains(collision.gameObject))
//         {
//             collidingMagnets.Add(collision.gameObject);
//         }
//     }
    
//     void OnCollisionExit2D(Collision2D collision)
//     {
//         Magetism otherMagetism = collision.gameObject.GetComponent<Magetism>();
//         if (otherMagetism != null)
//         {
//             collidingMagnets.Remove(collision.gameObject);
//         }
//     }
// }

        


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

public class Magetism : MonoBehaviour
{
    public float magetismStrength = 100f;
    public MagneticPole currentPole;
    public MagnetismMode currentMode;
    public MagnetismAxis currentAxis;
    public float attractionRange = 5f;

    private List<GameObject> collidingMagnets = new List<GameObject>();

    void Update()
    {
        UpdateMagneticField();
        UpdateColor();
    }

    void UpdateMagneticField()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attractionRange);
        foreach (Collider2D other in colliders)
        {
            if (other.gameObject == this.gameObject) continue; // Skip self

            // Only affect the player
            if (!other.CompareTag("Player")) continue;

            Magetism otherMagetism = other.GetComponent<Magetism>();
            if (otherMagetism != null && otherMagetism.currentPole != MagneticPole.None && currentPole != MagneticPole.None)
            {
                Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 direction = (Vector2)transform.position - rb.position;
                    MagnetismMode effectiveMode = currentMode;

                    if (collidingMagnets.Contains(other.gameObject))
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

                    if (currentPole == otherMagetism.currentPole)
                        rb.AddForce(-direction.normalized * magetismStrength);
                    else
                        rb.AddForce(direction.normalized * magetismStrength);
                }
            }
        }
    }

    void UpdateColor()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (currentPole == MagneticPole.North)
            sr.color = Color.red;
        else if (currentPole == MagneticPole.South)
            sr.color = Color.blue;
        else
            sr.color = Color.black;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Magetism otherMagetism = collision.gameObject.GetComponent<Magetism>();
        if (otherMagetism != null && !collidingMagnets.Contains(collision.gameObject))
            collidingMagnets.Add(collision.gameObject);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        Magetism otherMagetism = collision.gameObject.GetComponent<Magetism>();
        if (otherMagetism != null)
            collidingMagnets.Remove(collision.gameObject);
    }
}



