// using UnityEngine;

// public class MoveTrigger : MonoBehaviour
// {
//     public GameObject platform;
//     public SmoothMovement smoothMovement;
//     private bool isTriggered = false;
//     private SpriteRenderer spriteRenderer;

//     void Awake()
//     {
//         spriteRenderer = GetComponent<SpriteRenderer>();
//     }

//     void Reset()
//     {
//         var col = GetComponent<Collider2D>();
//         if (col) col.isTrigger = true;
//     }

//     void OnTriggerEnter2D(Collider2D other)
//     {
//         if (isTriggered) return;
//         if (other.CompareTag("Player"))
//         {
//             if (spriteRenderer) spriteRenderer.enabled = false;

//             // isTriggered = true;
//             smoothMovement.Activate();
//             // Destroy(gameObject);
//         }
//     }
    
//     void OnTriggerExit2D(Collider2D other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             if (spriteRenderer) spriteRenderer.enabled = true;
//         }
//     }
// }


using UnityEngine;
using System.Collections.Generic;

public class MoveTrigger : MonoBehaviour
{
    public GameObject platform;
    public SmoothMovement smoothMovement;
    private bool isTriggered = false;
    private SpriteRenderer spriteRenderer;

    public List<Collider2D> checkColliders;
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
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
    //    if (isTriggered) return;
    //    if (other.CompareTag("Player"))
    //    {
    //        if (spriteRenderer) spriteRenderer.enabled = false;
    //        smoothMovement.Activate();
    //    }
        if (!other.CompareTag("Player")) return;
        
        // Play trigger sound
        PlayerAudio playerAudio = other.GetComponent<PlayerAudio>();
        if (playerAudio != null)
        {
            playerAudio.PlayTriggerSound();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //if (isTriggered) return;
        if (checkColliders.Contains(other))
        {
            Debug.Log("collider: "+other.name+" stay in open trigger area: "+gameObject.name);
            if (spriteRenderer) spriteRenderer.enabled = false;

            // isTriggered = true;
            smoothMovement.Activate();
            // Destroy(gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (spriteRenderer) spriteRenderer.enabled = true;
        }
    }
}
