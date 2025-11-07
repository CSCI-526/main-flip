using UnityEngine;
using UnityEngine.SceneManagement;

public class Gate : MonoBehaviour
{

    [Header("Colliders")]
    public BoxCollider2D triggerZone;

    [Header("Gate States")]
    public GameObject openedGate;
    public LevelCompleteUI levelCompleteUI;
    [SerializeField] private bool skipLevelCompleteUI = false;


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
            if (skipLevelCompleteUI)
                {
                    int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
                    if (nextIndex < SceneManager.sceneCountInBuildSettings)
                        SceneManager.LoadScene(nextIndex);
                }
                else
                {
                    levelCompleteUI.Show();
                }
            //StartCoroutine(UploadMetric());
            //LevelAnalytics.Instance?.OnLevelCompleted();
            LevelAnalytics.Instance?.MarkGateReached();
        }
    }
    
}
