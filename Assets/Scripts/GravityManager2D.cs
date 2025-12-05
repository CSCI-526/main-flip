// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.Events;

// public class GlobalGravity2D : MonoBehaviour
// {
//     [Header("Gravity")]
//     [Min(0f)] public float gravityMagnitude = 3f;   
//     public bool startUpwards = true;               
//     public bool resetVerticalVelocityOnFlip = true; 

//     [Header("Targets")]
//     public List<Rigidbody2D> targets = new List<Rigidbody2D>(); 

//     float currentSign; 

//     public float forceFieldSwitchEnergy = 1.0f;
//     public float forceFieldSwitchMaxEnergy = 1.0f;
//     public float forceFieldSwitchEnergyRechargeSpeed = 0.1f;
//     public Image energyBar;
//     float lastSwitchTime = -Mathf.Infinity;

//     private InputManager inputManager;
//     private KeyBindUI keyBindUI;

//     void Awake()
//     {
//         inputManager = InputManager.Instance;
//         keyBindUI = GetComponent<KeyBindUI>();
//         currentSign = startUpwards ? -1f : 1f;
//         ApplyGravityScaleToAll(true);
//     }

//     void Update()
//     {
//         if (keyBindUI != null && keyBindUI.isRebinding)
//             return;

//         if (LevelManager.Instance.isRespawning)
//             return;

//         //if (Input.GetKeyDown(KeyCode.Space))
//         if (inputManager != null && Input.GetKeyDown(inputManager.keyMappings["SwitchGravity"]))
//         {
//             OperationAnalytics.Instance?.RegisterOperation();
//             if (forceFieldSwitchEnergy >= 1.0f)
//             {
//                 currentSign *= -1f;   
//                 ApplyGravityScaleToAll(false);
//             }
//         }

//         if (forceFieldSwitchEnergy < forceFieldSwitchMaxEnergy)
//         {
//             forceFieldSwitchEnergy += Time.deltaTime * forceFieldSwitchEnergyRechargeSpeed;
//             if (forceFieldSwitchEnergy > forceFieldSwitchMaxEnergy)
//                 forceFieldSwitchEnergy = forceFieldSwitchMaxEnergy;
//         }

//         if (energyBar)
//         {
//             energyBar.fillAmount = forceFieldSwitchEnergy / forceFieldSwitchMaxEnergy;
//         }
//     }

//     void ApplyGravityScaleToAll(bool ignoreEnergy = false)
//     {
//         if (!ignoreEnergy && forceFieldSwitchEnergy < 1.0f)
//             return;
        
//         float g = currentSign * Mathf.Abs(gravityMagnitude);

//         for (int i = 0; i < targets.Count; i++)
//         {
//             var rb = targets[i];
//             if (!rb) continue;

//             if (resetVerticalVelocityOnFlip)
//                 rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

//             rb.gravityScale = g;
//         }

//         if (!ignoreEnergy) {
//             forceFieldSwitchEnergy -= 1.0f;
//             lastSwitchTime = Time.time;
//             if (ActionSwitchTracker.Instance != null)
//                 ActionSwitchTracker.Instance.RecordGravityFlip();
//         }
//     }

//     public void AddTarget(Rigidbody2D rb)
//     {
//         if (rb && !targets.Contains(rb))
//         {
//             targets.Add(rb);
//             rb.gravityScale = currentSign * Mathf.Abs(gravityMagnitude);
//         }
//     }

//     public void RemoveTarget(Rigidbody2D rb)
//     {
//         if (rb) targets.Remove(rb);
//     }

//     public void switchGravity()
//     {
//         currentSign *= -1f;   
//         ApplyGravityScaleToAll(false);
//     }
// }



using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GlobalGravity2D : MonoBehaviour
{
    [Header("Gravity")]
    [Min(0f)] public float gravityMagnitude = 3f;   
    public bool startUpwards = true;               
    public bool resetVerticalVelocityOnFlip = true; 

    [Header("Targets")]
    public List<Rigidbody2D> targets = new List<Rigidbody2D>(); 

    float currentSign; 

    public float forceFieldSwitchEnergy = 1.0f;
    public float forceFieldSwitchMaxEnergy = 1.0f;
    public float forceFieldSwitchEnergyRechargeSpeed = 0.1f;
    public Image energyBar;
    float lastSwitchTime = -Mathf.Infinity;

    private InputManager inputManager;
    private KeyBindUI keyBindUI;

    // ---------------------------
    // AUDIO (added)
    // ---------------------------
    [Header("Audio")]
    public AudioClip gravityFlipSound;
    private AudioSource audioSource;
    // ---------------------------


    void Awake()
    {
        inputManager = InputManager.Instance;
        keyBindUI = FindObjectOfType<KeyBindUI>(true);
        currentSign = startUpwards ? -1f : 1f;
        ApplyGravityScaleToAll(true);

        // ---------------------------
        // AUDIO INIT (added)
        // ---------------------------
        audioSource = gameObject.AddComponent<AudioSource>();
        // ---------------------------
    }

    void Update()
    {
        if (keyBindUI != null && keyBindUI.isRebinding)
            return;

        if (LevelManager.Instance.isUncontrolable)
            return;

        //if (Input.GetKeyDown(KeyCode.Space))
        if (inputManager != null && (Input.GetKeyDown(inputManager.keyMappings["SwitchGravity"]) || Input.GetKeyDown(inputManager.keyMappings["SwitchGravityAlt"])))
        {
            OperationAnalytics.Instance?.RegisterOperation();
            if (forceFieldSwitchEnergy >= 1.0f)
            {
                currentSign *= -1f;

                // ---------------------------
                // PLAY GRAVITY FLIP SOUND (added)
                // ---------------------------
                if (gravityFlipSound)
                    audioSource.PlayOneShot(gravityFlipSound);
                // ---------------------------

                ApplyGravityScaleToAll(false);
            }
        }

        if (forceFieldSwitchEnergy < forceFieldSwitchMaxEnergy)
        {
            forceFieldSwitchEnergy += Time.deltaTime * forceFieldSwitchEnergyRechargeSpeed;
            if (forceFieldSwitchEnergy > forceFieldSwitchMaxEnergy)
                forceFieldSwitchEnergy = forceFieldSwitchMaxEnergy;
        }

        if (energyBar)
        {
            energyBar.fillAmount = forceFieldSwitchEnergy / forceFieldSwitchMaxEnergy;
        }
    }

    void ApplyGravityScaleToAll(bool ignoreEnergy = false)
    {
        if (!ignoreEnergy && forceFieldSwitchEnergy < 1.0f)
            return;
        
        float g = currentSign * Mathf.Abs(gravityMagnitude);

        for (int i = 0; i < targets.Count; i++)
        {
            var rb = targets[i];
            if (!rb) continue;

            if (resetVerticalVelocityOnFlip)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

            rb.gravityScale = g;
        }

        if (!ignoreEnergy) {
            forceFieldSwitchEnergy -= 1.0f;
            lastSwitchTime = Time.time;
            if (ActionSwitchTracker.Instance != null)
                ActionSwitchTracker.Instance.RecordGravityFlip();
        }
    }

    public void AddTarget(Rigidbody2D rb)
    {
        if (rb && !targets.Contains(rb))
        {
            targets.Add(rb);
            rb.gravityScale = currentSign * Mathf.Abs(gravityMagnitude);
        }
    }

    public void RemoveTarget(Rigidbody2D rb)
    {
        if (rb) targets.Remove(rb);
    }

    public void switchGravity()
    {
        if (LevelManager.Instance.isUncontrolable)
            return;

        currentSign *= -1f;

        // ---------------------------
        // PLAY GRAVITY FLIP SOUND (added)
        // ---------------------------
        if (gravityFlipSound)
            audioSource.PlayOneShot(gravityFlipSound);
        // ---------------------------

        ApplyGravityScaleToAll(false);
    }
}
