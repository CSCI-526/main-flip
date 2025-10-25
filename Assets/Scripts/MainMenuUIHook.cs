using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUIHook : MonoBehaviour
{
    public Button btnStart;
    public Button btnLevels;

    void Awake() { Time.timeScale = 1f; }

    void Start()
    {
        Wire(btnStart,  () => Load("flip level 1 (tutorial)"));
        Wire(btnLevels, () => Load("Levels"));
        Debug.Log("MainMenuUIHook wired");
    }

    void Wire(Button b, System.Action act)
    {
        if (!b) return;
        b.onClick.RemoveAllListeners();
        b.onClick.AddListener(() => act());
    }

    void Load(string sceneName)
    {
        if (SceneLoader.Instance != null) SceneLoader.Instance.LoadScene(sceneName);
        else SceneManager.LoadScene(sceneName);
    }
}
