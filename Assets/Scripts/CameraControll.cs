using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    
    public float followSpeed = 5f;
    
    public float deadZoneX = 1f;
    
    public float deadZoneY = 1f;

    void LateUpdate()
    {
        Vector3 newCameraPosition = transform.position;
        float distanceX = target.position.x - transform.position.x;
        if (Mathf.Abs(distanceX) > deadZoneX)
        {
            newCameraPosition.x = Mathf.Lerp(transform.position.x, target.position.x, followSpeed * Time.deltaTime);
        }

        float distanceY = target.position.y - transform.position.y;
        if (Mathf.Abs(distanceY) > deadZoneY)
        {
            newCameraPosition.y = Mathf.Lerp(transform.position.y, target.position.y, followSpeed * Time.deltaTime);
        }

        transform.position = newCameraPosition;
    }

    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, new Vector3(deadZoneX * 2, deadZoneY * 2, 0));
        }
    }
}