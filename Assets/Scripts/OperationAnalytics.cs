using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;


public class OperationAnalytics : MonoBehaviour
{
    public static OperationAnalytics Instance;
    [SerializeField] private string entryFromId   = "entry.111415707";
    [SerializeField] private string entryIdealId  = "entry.530231308";
    [SerializeField] private string entryActualId = "entry.1013680714";
    [SerializeField] private string entryDeltaId  = "entry.871820406";
    [SerializeField] private string entrySceneId  = "entry.1645766974";

    private Checkpoint prevCheckpoint; 
    private int opsThisSegment = 0;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterOperation()
    {
        opsThisSegment++;
    }

public void OnCheckpointReached(Checkpoint reached)
{
    prevCheckpoint = reached;

    if (prevCheckpoint != null)
    {
        int ideal = Mathf.Max(0, prevCheckpoint.idealOpsToNext);
        //StartCoroutine(UploadSegment(prevCheckpoint.checkpointId,
        //                             reached.checkpointId,
        //                             ideal,
        //                             opsThisSegment));
        UploadSegment(prevCheckpoint.checkpointId,
                        reached.checkpointId,
                        ideal,
                        opsThisSegment);
    }

    prevCheckpoint = reached;
    opsThisSegment = 0;
}


    public void OnGateReached(int idealFromLastCheckpointToGate)
    {
        string toId = "GATE";

        // if (prevCheckpoint != null)
        // {
        //     StartCoroutine(UploadSegment(prevCheckpoint.checkpointId, toId,
        //                                  idealFromLastCheckpointToGate, opsThisSegment));
        // }
        // else
        // {
        //     StartCoroutine(UploadSegment("START", toId,
        //                                  idealFromLastCheckpointToGate, opsThisSegment));
        // }

        prevCheckpoint = null;
        opsThisSegment = 0;
    }

    private async Task UploadSegment(string fromId, string toId, int idealOps, int actualOps)
    {
        int delta = actualOps - idealOps;
        string sceneName = SceneManager.GetActiveScene().name;

        WWWForm form = new WWWForm();
        form.AddField(entryFromId, fromId);
        form.AddField(entryIdealId, idealOps.ToString());
        form.AddField(entryActualId, actualOps.ToString());
        form.AddField(entryDeltaId, delta.ToString());
        form.AddField(entrySceneId, sceneName);

        string URL = "https://docs.google.com/forms/d/e/1FAIpQLSdV_R7GeC2pZl8s_91xWHJY8xi7y7PKM-AXnV_Tjn4w4XB6HQ/formResponse"; 
        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        
        try
        {
            await www.SendWebRequest();
            Debug.Log("[Checkpoint] upload result ");
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("[OpsAnalytics] Upload failed: " + www.error);
            }
            else
            {
                Debug.Log("[OpsAnalytics] Upload successful!");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[OpsAnalytics] Exception during upload: {ex.Message}");
        }
    }
}
