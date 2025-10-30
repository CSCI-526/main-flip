using UnityEngine;
using UnityEngine.SceneManagement;  
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerMagnetismControll : MonoBehaviour
{
    public float forceFieldSwitchEnergy = 1.0f;
    public float forceFieldSwitchMaxEnergy = 1.0f;
    public float forceFieldSwitchEnergyRechargeSpeed = 0.1f;
    public Image energyBar;
    float lastSwitchTime = -Mathf.Infinity;

    void Update()
    {
        UpdatePole();

        if (forceFieldSwitchEnergy < forceFieldSwitchMaxEnergy)
        {
            forceFieldSwitchEnergy += Time.deltaTime * forceFieldSwitchEnergyRechargeSpeed;
            if (forceFieldSwitchEnergy > forceFieldSwitchMaxEnergy)
                forceFieldSwitchEnergy = forceFieldSwitchMaxEnergy;
        }

        if (energyBar)
        {
            Magnetism magnetism = GetComponent<Magnetism>();
            energyBar.fillAmount = forceFieldSwitchEnergy / forceFieldSwitchMaxEnergy;
             if (magnetism.currentPole == MagneticPole.North)
            {
                Color newColor;
                if (ColorUtility.TryParseHtmlString("#E76963", out newColor))
                {
                    energyBar.color = newColor;
                }
            }
            else
            {
                Color newColor;
                if (ColorUtility.TryParseHtmlString("#79A1D8", out newColor))
                {
                    energyBar.color = newColor;
                }
            }
        }
    }

    void UpdatePole()
    {
        Magnetism magnetism = GetComponent<Magnetism>();

        if (magnetism == null) return;

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) {
            if (forceFieldSwitchEnergy < 1.0f)
                return;
            
            if (magnetism.currentPole == MagneticPole.North)
            {
                magnetism.currentPole = MagneticPole.South;
            }
            else
            {
                magnetism.currentPole = MagneticPole.North;
            }

            forceFieldSwitchEnergy -= 1.0f;
            lastSwitchTime = Time.time;
        }
    }

    public void switchPole() {
        Magnetism magnetism = GetComponent<Magnetism>();

        if (magnetism == null) return;

        if (forceFieldSwitchEnergy < 1.0f)
            return;

        if (magnetism.currentPole == MagneticPole.North)
        {
            magnetism.currentPole = MagneticPole.South;
        }
        else
        {
            magnetism.currentPole = MagneticPole.North;
        }
        
        forceFieldSwitchEnergy -= 1.0f;
        lastSwitchTime = Time.time;
    }
}