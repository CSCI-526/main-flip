using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HelpUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private CanvasGroup group;
    [SerializeField] private Button backButton;

    [Header("Pages")]
    [SerializeField] private GameObject controlsPage;
    [SerializeField] private GameObject basicsPage;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 0.15f;
    [SerializeField] private bool hideOnAwake = true;

    private bool isShowing = false;
    public bool IsShowing => isShowing;

    private int pageIndex = 0;

    void Reset()
    {
        if (!group) group = GetComponent<CanvasGroup>();
    }

    void Awake()
    {
        if (!group) group = GetComponent<CanvasGroup>();

        if (backButton)
            backButton.onClick.AddListener(Hide);

        if (nextButton)
            nextButton.onClick.AddListener(NextPage);

        if (prevButton)
            prevButton.onClick.AddListener(PrevPage);

        SetPage(0);

        if (hideOnAwake)
            HideImmediate();
    }

    public void Toggle()
    {
        if (isShowing)
            Hide();
        else
            Show();
    }

    public void Show()
    {
        if (isShowing) return;

        isShowing = true;

        SetPage(0);

        gameObject.SetActive(true);
        StartCoroutine(FadeCanvas(1f));

        Time.timeScale = 0f;
    }

    public void Hide()
    {
        if (!isShowing) return;

        isShowing = false;

        StartCoroutine(FadeCanvas(0f));

        Time.timeScale = 1f;
    }

    private void HideImmediate()
    {
        if (!group) return;

        group.alpha = 0f;
        group.interactable = false;
        group.blocksRaycasts = false;
        gameObject.SetActive(false);
    }

    private IEnumerator FadeCanvas(float targetAlpha)
    {
        if (!group) yield break;

        float start = group.alpha;
        float t = 0f;

        if (targetAlpha > start)
        {
            group.interactable = true;
            group.blocksRaycasts = true;
        }

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            group.alpha = Mathf.Lerp(start, targetAlpha, t / fadeDuration);
            yield return null;
        }

        group.alpha = targetAlpha;

        if (Mathf.Approximately(targetAlpha, 0f))
        {
            group.interactable = false;
            group.blocksRaycasts = false;
            gameObject.SetActive(false);
        }
    }

    private void NextPage()
    {
        SetPage(1);
    }

    private void PrevPage()
    {
        SetPage(0);
    }

    private void SetPage(int index)
    {
        pageIndex = Mathf.Clamp(index, 0, 1);

        bool onControls = (pageIndex == 0);
        bool onBasics   = (pageIndex == 1);

        if (controlsPage) controlsPage.SetActive(onControls);
        if (basicsPage)   basicsPage.SetActive(onBasics);

        if (prevButton) prevButton.gameObject.SetActive(!onControls);
        if (nextButton) nextButton.gameObject.SetActive(!onBasics);
    }
}
