using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private CanvasGroup group;
    [SerializeField] private Button backToGameButton;
    [SerializeField] private Button respawnButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button backToMenuButton;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 0.15f;
    [SerializeField] private bool hideOnAwake = true;

    private bool isPaused = false;

    void Reset()
    {
        if (!group) group = GetComponent<CanvasGroup>();
    }

    void Awake()
    {
        if (!group) group = GetComponent<CanvasGroup>();

        if (backToGameButton) backToGameButton.onClick.AddListener(OnBackToGameClicked);
        if (respawnButton)    respawnButton.onClick.AddListener(OnRespawnClicked);
        if (restartButton)    restartButton.onClick.AddListener(OnRestartClicked);
        if (backToMenuButton) backToMenuButton.onClick.AddListener(OnBackToMenuClicked);

        if (hideOnAwake) HideImmediate();
    }


    public void TogglePause()
    {
        if (isPaused)
            Resume();
        else
            Show();
    }

    public void Show()
    {
        if (isPaused) return;

        isPaused = true;
        gameObject.SetActive(true);
        StartCoroutine(FadeCanvas(1f));

        Time.timeScale = 0f;
    }

    public void Resume()
    {
        if (!isPaused) return;

        isPaused = false;
        StartCoroutine(FadeCanvas(0f));

        Time.timeScale = 1f;
    }

    private void OnBackToGameClicked()
    {
        Resume();
    }

    private void OnRespawnClicked()
    {
        Time.timeScale = 1f;
        isPaused = false;
        HideImmediate();

        if (LevelManager.Instance != null)
            LevelManager.Instance.RespawnPlayer();
    }

    private void OnRestartClicked()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (LevelManager.Instance != null)
            LevelManager.Instance.RestartLevel();
    }

    private void OnBackToMenuClicked()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (LevelManager.Instance != null)
            LevelManager.Instance.BackToMenu();
    }

    private void HideImmediate()
    {
        if (!group) return;

        group.alpha = 0f;
        group.interactable = false;
        group.blocksRaycasts = false;
        gameObject.SetActive(false);
    }

    private IEnumerator FadeCanvas(float target)
    {
        if (!group) yield break;

        float start = group.alpha;
        float t = 0f;

        if (target > start)
        {
            group.blocksRaycasts = true;
            group.interactable = true;
        }

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            group.alpha = Mathf.Lerp(start, target, t / fadeDuration);
            yield return null;
        }
        group.alpha = target;

        if (Mathf.Approximately(target, 0f))
        {
            group.interactable = false;
            group.blocksRaycasts = false;
            gameObject.SetActive(false);
        }
    }
}
