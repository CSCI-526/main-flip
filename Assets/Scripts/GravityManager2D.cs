using System.Collections.Generic;
using UnityEngine;

public class GlobalGravity2D : MonoBehaviour
{
    [Header("Gravity")]
    [Min(0f)] public float gravityMagnitude = 3f;   
    public bool startUpwards = true;               
    public bool resetVerticalVelocityOnFlip = true; 

    [Header("Targets")]
    public List<Rigidbody2D> targets = new List<Rigidbody2D>(); 

    float currentSign; 

    void Awake()
    {
        currentSign = startUpwards ? -1f : 1f;
        ApplyGravityScaleToAll();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentSign *= -1f;   
            ApplyGravityScaleToAll();
        }
    }

    void ApplyGravityScaleToAll()
    {
        float g = currentSign * Mathf.Abs(gravityMagnitude);

        for (int i = 0; i < targets.Count; i++)
        {
            var rb = targets[i];
            if (!rb) continue;

            if (resetVerticalVelocityOnFlip)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

            rb.gravityScale = g;
        }
    }

    public void AddTarget(Rigidbody2D rb)
    {
        if (rb && !targets.Contains(rb))
        {
            targets.Add(rb);
            rb.gravityScale = currentSign * Mathf.Abs(gravityMagnitude);
        }
    }

    public void RemoveTarget(Rigidbody2D rb)
    {
        if (rb) targets.Remove(rb);
    }
}
