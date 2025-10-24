using System.Collections;
using UnityEngine.Networking;
using UnityEngine;

public class Gate : MonoBehaviour
{

    [Header("Colliders")]
    public BoxCollider2D triggerZone;

    [Header("Gate States")]
    public GameObject openedGate;
    public LevelCompleteUI levelCompleteUI;
    public GlobalGravity2D gravityManager;


    // Initialize references to colliders and gate states.
    private void Reset()
    {
        triggerZone = transform.Find("TriggerZone")?.GetComponent<BoxCollider2D>();
        openedGate = transform.Find("Opened")?.gameObject;

        if (triggerZone) triggerZone.isTrigger = true;
    }

    // Level complete when player enters the open gate.
    void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.CompareTag("Player"))
        {
            levelCompleteUI.Show();   
            StartCoroutine(UploadMetric());
        }
    }

    IEnumerator UploadMetric()
    {
        int cnt = 0;
        cnt = gravityManager.changeCount;
        Debug.Log("Uploading gravity flip count: " + cnt);

        string URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSe-hiIOkGqwkCmQ8dC5_93IlR7FMVcI_AV6SNpna58033PkLg/formResponse";
        WWWForm form = new WWWForm();
        form.AddField("entry.1010152915", cnt.ToString());
        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        yield return www.SendWebRequest();
    }
    
}
