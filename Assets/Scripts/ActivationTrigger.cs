// using UnityEngine;

// public class ActivationTrigger : MonoBehaviour
// {
//     // 这两个就是你要“解锁”的区域
//     public Collider2D openAreaCollider;
//     public Collider2D closeAreaCollider;

//     private bool isActivated = false;

//     // 确保自己是 Trigger
//     void Reset()
//     {
//         var col = GetComponent<Collider2D>();
//         if (col) col.isTrigger = true;
//     }

//     void Start()
//     {
//         // 一开始禁用它们，让玩家不能用
//         if (openAreaCollider != null) openAreaCollider.enabled = false;
//         if (closeAreaCollider != null) closeAreaCollider.enabled = false;
//     }

//     void OnTriggerEnter2D(Collider2D other)
//     {
//         if (isActivated) return;

//         if (other.CompareTag("Player"))
//         {
//             isActivated = true;

//             // 玩家碰到 Trigger 后，启用这两个区域
//             if (openAreaCollider != null) openAreaCollider.enabled = true;
//             if (closeAreaCollider != null) closeAreaCollider.enabled = true;

//             // 如果这个 Trigger 只需要生效一次，可以把自己销毁
//             Destroy(gameObject);
//         }
//     }
// }


using UnityEngine;

public class ActivationTrigger : MonoBehaviour
{
    public Collider2D openAreaCollider;
    public Collider2D closeAreaCollider;

    private bool isActivated = false;

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    void Start()
    {
        if (openAreaCollider != null) openAreaCollider.enabled = false;
        if (closeAreaCollider != null) closeAreaCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isActivated) return;

        if (other.CompareTag("Player"))
        {
            isActivated = true;

            if (openAreaCollider != null) openAreaCollider.enabled = true;
            if (closeAreaCollider != null) closeAreaCollider.enabled = true;

            // --- Play Trigger Collectible Sound ---
            PlayerAudio playerAudio = other.GetComponent<PlayerAudio>();
            if (playerAudio != null)
            {
                playerAudio.PlayTriggerSound();
            }

            Destroy(gameObject);
        }
    }
}
