using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelsUIHook : MonoBehaviour
{
    public Button btnBack;
    public Button btnLevel1;
    public Button btnLevel2;
    public Button btnLevel3; 

    void Awake()
    {
        Time.timeScale = 1f;
    }

    void Start()
    {
        Wire(btnBack,   "MainMenu");
        Wire(btnLevel1, "flip level 1 (tutorial)");
        Wire(btnLevel2, "flip level 2");
        Wire(btnLevel3, "flip level 3");
        Debug.Log("LevelsUIHook wired");
    }

    void Wire(Button b, string scene)
    {
        if (!b || string.IsNullOrEmpty(scene)) return;
        b.onClick.RemoveAllListeners();
        b.onClick.AddListener(() => {
            Debug.Log("CLICK -> " + scene);
            SceneManager.LoadScene(scene);
        });
    }
}
