
using System.Collections.Generic;
using UnityEngine;

public class PseudoPlayerControll_MagneticTutorial_1 : MonoBehaviour
{
    public Magnetism targetMagnetism;
    private Rigidbody2D rb;
    private Vector2 originalPosition;
    private float startTime;
    private List<float> switchTimeList = new List<float>(new float[] {2f, 4f});
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
                if (targetMagnetism != null)
                {
                    if (targetMagnetism.currentPole == MagneticPole.North)
                    {
                        targetMagnetism.currentPole = MagneticPole.South;
                    }
                    else if (targetMagnetism.currentPole == MagneticPole.South)
                    {
                        targetMagnetism.currentPole = MagneticPole.North;
                    }
                }
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
