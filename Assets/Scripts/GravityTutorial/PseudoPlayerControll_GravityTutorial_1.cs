using System.Collections.Generic;
using UnityEngine;

public class PseudoPlayerControll_GravityTutorial_1 : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 originalPosition;
    private float gravityMagnitude = 3f;   
    private float startTime;
    private List<float> switchTimeList = new List<float>(new float[] {0f, 3f});
    private int currentSwitchIndex = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = rb.position;
        startTime = Time.time;
    }

    void Update()
    {
        if (currentSwitchIndex < switchTimeList.Count)
        {
            if (Time.time - startTime >= switchTimeList[currentSwitchIndex])
            {
                gravityMagnitude *= -1f;
                rb.gravityScale = gravityMagnitude;
                currentSwitchIndex++;
            }
        } 
        else
        {
            rb.position = originalPosition;
            rb.linearVelocity = Vector2.zero;
            startTime = Time.time;
            currentSwitchIndex = 0;
        }
    }
}
