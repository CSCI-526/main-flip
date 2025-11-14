using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ActionSwitchTracker : MonoBehaviour
{
    public static ActionSwitchTracker Instance { get; private set; }

    private const string GFORM_URL =
        "https://docs.google.com/forms/d/e/1FAIpQLSfrklA_gmW2PxyYOF2whtSaFZVM5cg_jTHkmLNoZivKzaVSjw/formResponse";

    private const string ENTRY_ACTION_SWITCH_RATE = "entry.1249336548";
    private const string ENTRY_PLAYER_ID          = "entry.1290620746";
    private const string ENTRY_SCENE              = "entry.1425215553";

    private readonly List<int> seq = new List<int>();   // 0=gravity, 1=magnet
    private string playerId;

    private string lastSceneSubmitted = null;
    private float  lastSubmitAtRealtime = -999f;
    [SerializeField] private float submitCooldownSec = 1.0f; 

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        playerId = SystemInfo.deviceUniqueIdentifier;
    }

    public void RecordGravityFlip() { seq.Add(0); }
    public void RecordMagnetFlip()  { seq.Add(1); }

    public void SaveAndReset()
    {
        string scene = SceneManager.GetActiveScene().name;
        if (lastSceneSubmitted == scene &&
            (Time.realtimeSinceStartup - lastSubmitAtRealtime) < submitCooldownSec)
        {
            seq.Clear();
            return;
        }

        int N = seq.Count;
        int switches = 0;
        if (N >= 2)
            for (int i = 0; i < N - 1; i++) if (seq[i] != seq[i + 1]) switches++;

        float rate = (N == 0) ? 0f : (float)switches / N;

        StartCoroutine(UploadMetric(rate, playerId, scene));

        lastSceneSubmitted = scene;
        lastSubmitAtRealtime = Time.realtimeSinceStartup;

        seq.Clear();
    }

    private IEnumerator UploadMetric(float actionSwitchRate, string pId, string sceneName)
    {
        WWWForm form = new WWWForm();
        form.AddField(ENTRY_ACTION_SWITCH_RATE, actionSwitchRate.ToString(CultureInfo.InvariantCulture));
        form.AddField(ENTRY_PLAYER_ID, pId);
        form.AddField(ENTRY_SCENE, sceneName);

        using (UnityWebRequest www = UnityWebRequest.Post(GFORM_URL, form))
        {
            yield return www.SendWebRequest();

#if UNITY_2020_2_OR_NEWER
            if (www.result != UnityWebRequest.Result.Success)
                Debug.LogWarning("Google Form post failed: " + www.error);
#else
            if (www.isNetworkError || www.isHttpError)
                Debug.LogWarning("Google Form post failed: " + www.error);
#endif
        }
    }
}
