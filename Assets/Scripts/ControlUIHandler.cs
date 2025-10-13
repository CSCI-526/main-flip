using System.Collections;
using UnityEngine;
using TMPro; 
public class ControlUIHandler : MonoBehaviour
{
    public TextMeshProUGUI northControlText; 
    public TextMeshProUGUI southControlText; 

    [Tooltip("How long the text stays highlighted when pressed.")]
    public float highlightDuration = 0.1f;

    private Color defaultNorthColor = Color.white;
    private Color defaultSouthColor = Color.white;
    private Color highlightColor = new Color(0.8f, 0.8f, 0.8f); 

    void Start()
    {
        if (northControlText != null)
        {
            defaultNorthColor = northControlText.color;
        }
        if (southControlText != null)
        {
            defaultSouthColor = southControlText.color;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N) && northControlText != null)
        {
            HighlightControl(northControlText, defaultNorthColor);
        }

        if (Input.GetKeyDown(KeyCode.S) && southControlText != null)
        {
            HighlightControl(southControlText, defaultSouthColor);
        }
    }

    private void HighlightControl(TMP_Text controlText, Color originalColor)
    {
        StopAllCoroutines(); 
        controlText.color = highlightColor;
        StartCoroutine(FadeToColor(controlText, originalColor));
    }

    private IEnumerator FadeToColor(TMP_Text controlText, Color targetColor)
    {
        yield return new WaitForSeconds(highlightDuration);
        controlText.color = targetColor;
    }
}
