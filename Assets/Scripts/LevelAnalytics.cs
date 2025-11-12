using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LevelAnalytics : MonoBehaviour
{
    public static LevelAnalytics Instance;

    [Header("Google Forms")]
    [SerializeField] private string googleFormUrl =
        "https://docs.google.com/forms/u/0/d/e/1FAIpQLSekWv6SHlgGQzyTCn161UAmmqfBzhG71c1jLxpzo_vVMgh7kg/formResponse";

    [SerializeField] private string entryTriggerId      = "entry.108756292"; 
    [SerializeField] private string entryResponseTimeId = "entry.385238708"; 
    [SerializeField] private string entrySceneId        = "entry.1061002081"; 

    [SerializeField] private bool alsoWriteCsv = false;
    private string csvPath;

    private readonly Dictionary<string, float> lastTriggerTime = new();
    private string lastTriggerId = null;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        csvPath = Path.Combine(Application.persistentDataPath, "AnalyticsData.csv");
        if (alsoWriteCsv && !File.Exists(csvPath))
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
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "flip level 2")
        {
            Debug.Log("[Analytics] Skipped metric upload — this is the second scene.");
            return;
        }

        if (string.IsNullOrEmpty(lastTriggerId) || !lastTriggerTime.ContainsKey(lastTriggerId))
        {
            Debug.Log("[Analytics] Gate reached but no trigger was recorded.");
            return;
        }

        float start = lastTriggerTime[lastTriggerId];
        float response = Mathf.Max(0f, Time.time - start);
        string sceneName = currentScene.name;

        if (alsoWriteCsv)
        {
            string csvLine = $"{lastTriggerId},{response:F2},{sceneName}\n";
            File.AppendAllText(csvPath, csvLine);
            Debug.Log($"[Analytics→CSV] {csvLine.Trim()} → {csvPath}");
        }

        StartCoroutine(UploadMetric(lastTriggerId, response, sceneName));
    }
    
    public void MarkPlatformReached()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (string.IsNullOrEmpty(lastTriggerId) || !lastTriggerTime.ContainsKey(lastTriggerId))
        {
            Debug.Log("[Analytics] Gate reached but no trigger was recorded.");
            return;
        }

        float start = lastTriggerTime[lastTriggerId];
        float response = Mathf.Max(0f, Time.time - start);
        string sceneName = currentScene.name;

        if (alsoWriteCsv)
        {
            string csvLine = $"{lastTriggerId},{response:F2},{sceneName}\n";
            File.AppendAllText(csvPath, csvLine);
            Debug.Log($"[Analytics→CSV] {csvLine.Trim()} → {csvPath}");
        }

        StartCoroutine(UploadMetric(lastTriggerId, response, sceneName));
    }

    public void ResetForLevel()
    {
        lastTriggerId = null;
        lastTriggerTime.Clear();
    }

    IEnumerator UploadMetric(string lastTriggerId, float response, string sceneName)
    {
        string URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSekWv6SHlgGQzyTCn161UAmmqfBzhG71c1jLxpzo_vVMgh7kg/formResponse";
        WWWForm form = new WWWForm();
        form.AddField("entry.108756292", lastTriggerId.ToString());
        form.AddField("entry.385238708", response.ToString());
        form.AddField("entry.1061002081", sceneName.ToString());
        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        yield return www.SendWebRequest();
    }
}