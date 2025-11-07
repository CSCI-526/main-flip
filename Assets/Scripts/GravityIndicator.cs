using UnityEngine;
using UnityEngine.UI;

public class GravityIndicator : MonoBehaviour
{
    public Image gravityDirectionImage;
    public Image gravityDirectionDisabledImage;
    public GlobalGravity2D GlobalGravity2DController;

    public Sprite upGravitySprite;
    public Sprite downGravitySprite;
    public Rigidbody2D indicatingRigidbody;

    void Update()
    {
        float gravityScale = indicatingRigidbody.gravityScale;
        if (gravityScale <= 0)
        {   
            if (gravityDirectionImage != null && upGravitySprite != null)
            {
                gravityDirectionImage.sprite = upGravitySprite;
            }
        }
        else
        {
            if (gravityDirectionImage != null && downGravitySprite != null)
            {
                gravityDirectionImage.sprite = downGravitySprite;
            }
        }

        if (GlobalGravity2DController.forceFieldSwitchEnergy < 1.0f)
        {   
            gravityDirectionDisabledImage.enabled = true;
        }
        else
        {
            gravityDirectionDisabledImage.enabled = false;
        }
    }
}
