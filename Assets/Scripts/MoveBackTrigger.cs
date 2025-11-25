using UnityEngine;
using System.Collections.Generic;

public class MoveBackTrigger : MonoBehaviour
{
    public GameObject platform;
    public SmoothMovement smoothMovement;
    private bool isTriggered = false;

    public List<Collider2D> checkColliders;
    public List<bool> checkCollidersTriggered;
    public bool includePlayer=true;

    void Start()
    {
        if (includePlayer)
        {
            Collider2D playerColider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
            if (playerColider != null && !checkColliders.Contains(playerColider))
            {
                checkColliders.Add(playerColider);
            }
        }
        while (checkCollidersTriggered.Count < checkColliders.Count)
        {
            checkCollidersTriggered.Add(false);
        }   
    }

    void Update()
    {
        bool readyToTrigger = true;
        for (int i = 0; i < checkCollidersTriggered.Count; i++)
        {
            if (!checkCollidersTriggered[i])
            {
                readyToTrigger = false;
                break;
            }
        }
        if (readyToTrigger)
        {
            for (int i = 0; i < checkCollidersTriggered.Count; i++)
            {
                checkCollidersTriggered[i] = false;
            }
            smoothMovement.Deactivate();
        }
        
    }

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    //void OnTriggerEnter2D(Collider2D other)
    void OnTriggerStay2D(Collider2D other)
    {
        //if (other.CompareTag("Player"))
        //{
        //    smoothMovement.Deactivate();
        //}
        if (checkColliders.Contains(other))
        {
            Debug.Log("collider: "+other.name+", index: "+checkColliders.IndexOf(other)+" stay close trigger area.");
            checkCollidersTriggered[checkColliders.IndexOf(other)] = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (checkColliders.Contains(other))
        {
            Debug.Log("collider: "+other.name+", index: "+checkColliders.IndexOf(other)+" exit close trigger area.");
            checkCollidersTriggered[checkColliders.IndexOf(other)] = false;
        }
    }
}
