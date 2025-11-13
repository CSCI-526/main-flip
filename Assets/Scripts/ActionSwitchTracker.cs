using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class ActionSwitchTracker : MonoBehaviour
{
    public static ActionSwitchTracker Instance { get; private set; }

    private const string GFORM_URL =
        "https://docs.google.com/forms/d/e/1FAIpQLSfrklA_gmW2PxyYOF2whtSaFZVM5cg_jTHkmLNoZivKzaVSjw/formResponse";

    // entry ids
    private const string ENTRY_ACTION_SWITCH_RATE = "entry.1249336548";
    private const string ENTRY_PLAYER_ID          = "entry.1290620746";
    private const string ENTRY_SCENE              = "entry.1425215553";

    private readonly List<int> seq = new List<int>();   // 0=gravity, 1=magnet
    private string csvPath;
    private string playerId;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        csvPath = Path.Combine(Application.dataPath, "ActionSwitchRate.csv");
        playerId = SystemInfo.deviceUniqueIdentifier;
        EnsureHeader();
    }

    void EnsureHeader()
    {
        if (!File.Exists(csvPath))
        {
            File.WriteAllText(csvPath, "player_id,scene,N,switch_count,action_switch_rate,sequence\n");
        }
    }

    public void RecordGravityFlip() { seq.Add(0); }
    public void RecordMagnetFlip()  { seq.Add(1); }

    public void SaveAndReset()
    {
        int N = seq.Count;
        int switches = 0;
        if (N >= 2)
        {
            for (int i = 0; i < N - 1; i++)
            {
                if (seq[i] != seq[i + 1]) switches++;
            }
        }
        float rate = (N == 0) ? 0f : (float)switches / N;

        string scene = SceneManager.GetActiveScene().name;
        string sequenceStr = string.Join("|", seq.Select(i => i.ToString()).ToArray());

        string line = $"{playerId},{scene},{N},{switches},{rate.ToString(CultureInfo.InvariantCulture)},{sequenceStr}\n";
        File.AppendAllText(csvPath, line);

        StartCoroutine(UploadMetric(rate, playerId, scene));

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
