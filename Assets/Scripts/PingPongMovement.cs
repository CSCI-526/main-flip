using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongMovement : MonoBehaviour
{
    public enum MovementType { Horizontal, Vertical } 
    public MovementType movementType = MovementType.Horizontal;

    public float distance = 3f;   
    public float speed = 1.0f;    

    private Vector3 startPosition;
    private Vector3 endPosition;

    void Start()
    {
        startPosition = transform.position;

        if (movementType == MovementType.Horizontal)
        {
            endPosition = new Vector3(startPosition.x + distance, startPosition.y, startPosition.z);
        }
        else if (movementType == MovementType.Vertical)
        {
            endPosition = new Vector3(startPosition.x, startPosition.y + distance, startPosition.z);
        }
    }

    void Update()
    {
        transform.position = Vector3.Lerp(startPosition, endPosition, Mathf.PingPong(Time.time * speed, 1.0f));
    }
}


