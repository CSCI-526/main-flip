using UnityEngine;
using UnityEngine.UI;


public class PolarityIndicator : MonoBehaviour
{
    public Image polarityImage;
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
    }
}
