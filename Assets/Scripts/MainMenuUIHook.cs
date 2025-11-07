using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuUIHook : MonoBehaviour
{
    [Header("Scenes")]
#if UNITY_EDITOR
    public SceneAsset startSceneAsset;
    public SceneAsset levelsSceneAsset;
#endif
    [SerializeField, HideInInspector] private string startSceneName;
    [SerializeField, HideInInspector] private string levelsSceneName;

    [Header("Buttons")]
    public Button btnStart;
    public Button btnLevels;

    void Awake() { Time.timeScale = 1f; }

    void Start()
    {
        Wire(btnStart,  () => Load(startSceneName));
        Wire(btnLevels, () => Load(levelsSceneName));
    }

    void Wire(Button b, System.Action act)
    {
        if (!b) return;
        b.onClick.RemoveAllListeners();
        b.onClick.AddListener(() => act());
    }

    void Load(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
            SceneManager.LoadScene(sceneName);
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        startSceneName = startSceneAsset ? startSceneAsset.name : "";
        levelsSceneName   = levelsSceneAsset   ? levelsSceneAsset.name   : "";
    }
#endif
}
