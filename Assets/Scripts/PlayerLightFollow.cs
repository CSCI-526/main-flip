using UnityEngine;

public class PlayerLightFollow : MonoBehaviour
{
    public Transform target;         
    public Vector3 offset = Vector3.zero;

    void Awake()
    {
        if (!target)
        {
            var go = GameObject.FindWithTag("Player"); 
            if (go) target = go.transform;
        }
    }

    void LateUpdate()
    {
        if (!target) return;
        transform.position = target.position + offset;
    }
}
