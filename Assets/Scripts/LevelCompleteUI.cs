using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelCompleteUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private Button replayButton;
    [SerializeField] private Button nextLevelButton;

    [SerializeField] private float fadeDuration = 0.15f;
    [SerializeField] private bool pauseOnShow = true;
    [SerializeField] private bool hideOnAwake = true;

    void Reset()
    {
        if (!group) group = GetComponent<CanvasGroup>();
        if (!replayButton) replayButton = GetComponentInChildren<Button>(true);
    }

    void Awake()
    {
        if (!group) group = GetComponent<CanvasGroup>();
        if (!replayButton) replayButton = GetComponentInChildren<Button>(true);
        if (replayButton) replayButton.onClick.AddListener(OnReplayClicked);

        if (hideOnAwake) HideImmediate();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeCanvas(1f));

        if (pauseOnShow)
            Time.timeScale = 0f;

        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextLevelButton != null)
        {
            bool hasNextLevel = nextIndex < SceneManager.sceneCountInBuildSettings;
            nextLevelButton.gameObject.SetActive(hasNextLevel);
        }
    }

    public void Hide()
    {
        StartCoroutine(FadeCanvas(0f));
        if (pauseOnShow) Time.timeScale = 1f;
    }

    private void OnReplayClicked()
    {
        if (pauseOnShow) Time.timeScale = 1f;
        var current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
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

    public void OnNextLevelClicked()
    {
        if (pauseOnShow) Time.timeScale = 1f;
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        if (next < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(next);
    }
}
