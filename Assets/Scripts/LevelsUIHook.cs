using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;   
#endif

[System.Serializable]
public class LevelItem
{
    public Button button;

#if UNITY_EDITOR
    public SceneAsset sceneAsset;
#endif
    [SerializeField, HideInInspector] private string sceneName;
    public string SceneName => sceneName;

    // optional visuals
    public GameObject lockedVisual;    // padlock, dim layer, etc.
    public GameObject unlockedVisual;  // check mark, glow, etc.

#if UNITY_EDITOR
    public void SyncName() { sceneName = sceneAsset ? sceneAsset.name : ""; }
#endif

    public void ApplyLockState()
    {
        bool canPlay = LevelProgress.IsUnlocked(SceneName);
        if (button) button.interactable = canPlay;
        if (lockedVisual)   lockedVisual.SetActive(!canPlay);
        if (unlockedVisual) unlockedVisual.SetActive(canPlay);
    }
}

public class LevelsUIHook : MonoBehaviour
{
    [Header("Back")]
    public Button btnBack;
#if UNITY_EDITOR
    public SceneAsset backSceneAsset;
#endif
    [SerializeField, HideInInspector] private string backSceneName = "MainMenu";

    [Header("Levels (Button + Scene)")]
    public List<LevelItem> levels = new List<LevelItem>();

    void Awake() { Time.timeScale = 1f; }

    void OnEnable()  { RefreshButtons(); } // refresh when coming back
    void Start()     { WireAll(); }

    void WireAll()
    {
        // Back
        if (btnBack && !string.IsNullOrEmpty(backSceneName))
            Wire(btnBack, backSceneName);

        // Levels
        foreach (var it in levels)
        {
            if (it == null || it.button == null) continue;
            if (string.IsNullOrEmpty(it.SceneName)) continue;
            Wire(it.button, it.SceneName);
        }
    }

    void RefreshButtons()
    {
        foreach (var it in levels)
        {
            if (it == null) continue;
            it.ApplyLockState();
        }
    }

    void Wire(Button b, string scene)
    {
        b.onClick.RemoveAllListeners();
        b.onClick.AddListener(() =>
        {
            Debug.Log("CLICK -> " + scene);
            if (SceneLoader.Instance != null) SceneLoader.Instance.LoadScene(scene);
            else
            {
                Debug.LogError("SceneLoader not found! Falling back to raw SceneManager.");
                SceneManager.LoadScene(scene);
            }
        });
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (levels != null) foreach (var it in levels) it?.SyncName();
        backSceneName = backSceneAsset ? backSceneAsset.name : backSceneName;
    }
#endif
}
