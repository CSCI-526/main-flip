using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelAnalytics : MonoBehaviour
{
    public static LevelAnalytics Instance;

    private string csvPath;

    private readonly Dictionary<string, float> lastTriggerTime = new();
    private string lastTriggerId = null;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        csvPath = Path.Combine(Application.dataPath, "AnalyticsData.csv");
        EnsureHeader();
    }

    private void EnsureHeader()
    {
        if (!File.Exists(csvPath))
        {
            File.WriteAllText(csvPath, "Trigger,ResponseTimeSec,Scene\n");
        }
    }

    public void MarkTrigger(string triggerId)
    {
        float t = Time.time;
        lastTriggerTime[triggerId] = t;
        lastTriggerId = triggerId;
        Debug.Log($"[Analytics] Trigger '{triggerId}' at {t:F2}s");
    }

    public void MarkGateReached()
    {
        if (string.IsNullOrEmpty(lastTriggerId) || !lastTriggerTime.ContainsKey(lastTriggerId))
        {
            Debug.Log("[Analytics] Gate reached but no trigger was recorded.");
            return;
        }

        float start = lastTriggerTime[lastTriggerId];
        float response = Mathf.Max(0f, Time.time - start);

        string sceneName = SceneManager.GetActiveScene().name;

        string line = $"{lastTriggerId},{response:F2},{sceneName}\n";
        File.AppendAllText(csvPath, line);

        Debug.Log($"[Analytics] Logged: {line.Trim()} → {csvPath}");
    }

    public void ResetForLevel()
    {
        lastTriggerId = null;
        lastTriggerTime.Clear();
    }
}
