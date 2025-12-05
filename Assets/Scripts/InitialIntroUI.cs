using UnityEngine;

public class InitialIntroUI : MonoBehaviour
{
    void Start()
    {

    }

    void OnEnable()
    {
        Time.timeScale = 0f; 
    }

    void OnDisable()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f; 
        }
    }

    void Update()
    {
        if (gameObject.activeInHierarchy) {
            Time.timeScale = 0f;
        } else {
            Time.timeScale = 1f;
        }
    }
}