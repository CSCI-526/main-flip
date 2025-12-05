using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SyncPlayerImage : MonoBehaviour
{
    
    public GameObject playerObject;
    public MagneticPole currentPole;
    public GlobalGravity2D gravityManager;
    public float currentSign;

    public Sprite northUPImage;
    public Sprite southUPImage;
    public Sprite northDOWNImage;
    public Sprite southDOWNImage;

    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        playerObject = this.gameObject;
        currentPole = playerObject.GetComponent<Magnetism>().currentPole;
        gravityManager = FindObjectOfType<GlobalGravity2D>();
        currentSign = gravityManager.currentSign;
        spriteRenderer = playerObject.GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        currentPole = playerObject.GetComponent<Magnetism>().currentPole;
        currentSign = gravityManager.currentSign;

        if (currentPole == MagneticPole.North)
        {
            if (currentSign < 0)
            {
                spriteRenderer.color = Color.white;
                spriteRenderer.sprite = northUPImage;
                
            }
            else
            {
                spriteRenderer.color = Color.white;
                spriteRenderer.sprite = northDOWNImage;
            }
        }
        else
        {
            if (currentSign < 0)
            {
                spriteRenderer.color = Color.white;
                spriteRenderer.sprite = southUPImage;
            }
            else
            {
                spriteRenderer.color = Color.white;
                spriteRenderer.sprite = southDOWNImage;
            }
        }

        playerObject.transform.localScale = Vector3.one;
    }
}
