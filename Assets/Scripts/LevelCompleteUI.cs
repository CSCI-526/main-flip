using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelCompleteUI : MonoBehaviour
{
    private CanvasGroup group;
    private Button replayButton;
    private float fadeDuration = 0.25f;
    private bool pauseOnShow = true;

    void Awake()
    {
        if (!group) group = GetComponent<CanvasGroup>();
        if (replayButton) replayButton.onClick.AddListener(OnReplayClicked);
        HideImmediate();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeCanvas(1f));

        if (pauseOnShow)
            Time.timeScale = 0f;
    }
    
    public void Hide()
    {
        StartCoroutine(FadeCanvas(0f));
        if (pauseOnShow)
            Time.timeScale = 1f;
    }

    private void OnReplayClicked()
    {
        if (pauseOnShow)
            Time.timeScale = 1f;

        Scene current = SceneManager.GetActiveScene();
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
}
