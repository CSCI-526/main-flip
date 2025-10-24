using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelAnalytics : MonoBehaviour
{
    public static LevelAnalytics Instance;

    private float triggerActivatedTime = -1f;
    private string analyticsFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        analyticsFilePath = Path.Combine(Application.dataPath, "AnalyticsData.csv");

        if (!File.Exists(analyticsFilePath))
        {
            File.WriteAllText(analyticsFilePath, "Scene,TriggerActivatedTime,TriggerResponseTime\n");
        }
    }

    public void OnTriggerActivated()
    {
        triggerActivatedTime = Time.time;
        Debug.Log($"[Analytics] Trigger activated at {triggerActivatedTime:F2}s");
    }

    public void OnLevelCompleted()
    {
        if (triggerActivatedTime > 0f)
        {
            float completionTime = Time.time - triggerActivatedTime;
            string sceneName = SceneManager.GetActiveScene().name;

            string line = $"{sceneName},{triggerActivatedTime:F2},{completionTime:F2}\n";
            File.AppendAllText(analyticsFilePath, line);

            Debug.Log($"[Analytics] Time from trigger to gate: {completionTime:F2}s saved to AnalyticsData.csv");
        }
        else
        {
            Debug.Log("[Analytics] Gate reached before trigger or trigger not logged.");
        }
    }
}
