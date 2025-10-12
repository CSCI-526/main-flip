using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPad : MonoBehaviour
{
    public Transform target;         
    public float exitOffsetY = 0.5f;  
    public float cooldown = 0.25f;    

    static readonly HashSet<Collider2D> lockset = new HashSet<Collider2D>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!target) return;
        if (lockset.Contains(other)) return;

        var rb = other.attachedRigidbody;
        if (rb) rb.linearVelocity = Vector2.zero;
        other.transform.position = target.position + Vector3.up * exitOffsetY;

        lockset.Add(other);
        StartCoroutine(Release(other, cooldown));
    }

    IEnumerator Release(Collider2D c, float t)
    {
        yield return new WaitForSeconds(t);
        lockset.Remove(c);
    }
}
