using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActionSwitchTracker : MonoBehaviour
{
    public static ActionSwitchTracker Instance { get; private set; }

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
            File.WriteAllText(csvPath, "scene,N,switch_count,action_switch_rate,sequence\n");
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
                if (seq[i] != seq[i + 1]) switches++;
        }
        float rate = (N == 0) ? 0f : (float)switches / N;

        string scene = SceneManager.GetActiveScene().name;
        string sequenceStr = string.Join("|", seq);

        string line = $"{scene},{N},{switches},{rate},{sequenceStr}\n";
        File.AppendAllText(csvPath, line);

        seq.Clear();
    }
}
