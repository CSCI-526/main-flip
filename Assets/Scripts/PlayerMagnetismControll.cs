using UnityEngine;
using UnityEngine.SceneManagement;  

public class PlayerMagnetismControl : MonoBehaviour
{
    void Update()
    {
        UpdatePole();
    }

    void UpdatePole()
    {
        Magnetism magnetism = GetComponent<Magnetism>();

        if (magnetism == null) return;

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) {
            if (magnetism.currentPole == MagneticPole.North)
            {
                magnetism.currentPole = MagneticPole.South;
            }
            else
            {
                magnetism.currentPole = MagneticPole.North;
            }
        }
    }
}