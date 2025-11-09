using UnityEngine;

public class BlockActivator : MonoBehaviour
{
    [Header("Trigger Settings")]
    public GameObject triggerObject;
    public Transform targetPoint;
    public float activationRadius = 0.1f;

    [Header("Object To Activate")]
    public GameObject targetToActivate;

    private bool hasActivated = false;

    void Update()
    {
        if (hasActivated) return;
        if (triggerObject == null || targetToActivate == null) return;

        Vector3 checkPosition = targetPoint ? targetPoint.position : transform.position;

        float dist = Vector2.Distance(triggerObject.transform.position, checkPosition);
        if (dist <= activationRadius)
        {
            targetToActivate.SetActive(true);
            hasActivated = true;
            Debug.Log($"{targetToActivate.name} activated when {triggerObject.name} reached the target point.");
        }
    }
}
