using UnityEngine;
using UnityEngine.UI;


public class PolarityIndicator : MonoBehaviour
{
    public Image polarityImage;
    public Image polarityDisabledImage;
    public PlayerMagnetismControll playerMagnetismController;

    public Sprite NorthSprite;
    public Sprite SouthSprite;
    public Magnetism indicatingMagnetism;


    void Update()
    {
        MagneticPole currentPole = indicatingMagnetism.currentPole;
        if (currentPole == MagneticPole.North)
        {   
            if (polarityImage != null && NorthSprite != null)
            {
                polarityImage.sprite = NorthSprite;
            }
        }
        else
        {
            if (polarityImage != null && SouthSprite != null)
            {
                polarityImage.sprite = SouthSprite;
            }
        }

        if (playerMagnetismController.forceFieldSwitchEnergy < 1.0f)
        {
            polarityDisabledImage.enabled = true;
        }
        else
        {
            polarityDisabledImage.enabled = false;
        }
    }
}
