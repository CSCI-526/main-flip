// using UnityEngine;

// public class MoveBackTrigger : MonoBehaviour
// {
//     public GameObject platform;
//     public SmoothMovement smoothMovement;
//     private bool isTriggered = false;

//     void Reset()
//     {
//         var col = GetComponent<Collider2D>();
//         if (col) col.isTrigger = true;
//     }

//     void OnTriggerEnter2D(Collider2D other)
//     {
//         //if (isTriggered) return;
//         if (other.CompareTag("Player"))
//         {
//             //isTriggered = true;
//             smoothMovement.Deactivate();
//             //Destroy(gameObject);
//         }
        
//     }
// }


using UnityEngine;

public class MoveBackTrigger : MonoBehaviour
{
    public GameObject platform;
    public SmoothMovement smoothMovement;
    private bool isTriggered = false;

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Play trigger sound
            PlayerAudio playerAudio = other.GetComponent<PlayerAudio>();
            if (playerAudio != null)
            {
                playerAudio.PlayTriggerSound();
            }

            smoothMovement.Deactivate();
        }
    }
}
