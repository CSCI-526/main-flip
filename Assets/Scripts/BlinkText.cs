using UnityEngine;
using TMPro;

public class BlinkText : MonoBehaviour
{
    public TextMeshProUGUI text; 
    public float speed = 3f;     
    public float minAlpha = 0.3f;  
    public float maxAlpha = 1f;  

    void Reset()
    {
        if (!text) text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy) return;
        if (!text) return;

        float t = (Mathf.Sin(Time.time * speed) + 1f) * 0.5f;
        float a = Mathf.Lerp(minAlpha, maxAlpha, t);

        Color c = text.color;
        c.a = a;
        text.color = c;
    }
}
