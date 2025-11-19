using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class DeathHazardController : MonoBehaviour
{
    public static DeathHazardController currentRegion = null;

    [Header("Death Settings")]
    public int deathThreshold = 5;
    public float powerDuration = 30f;

    [Header("Hazards To Toggle")]
    public List<GameObject> hazardsToToggle; 

    [Header("UI")]
    public GameObject hintPanel;

    [Header("Flash Settings")]
    public float flashDuration = 0.5f;
    public float flashInterval = 0.1f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip countdownBeepClip; 

    [Header("Countdown UI")]
    public GameObject countdownPanel;
    public Slider countdownSlider;
    public TextMeshProUGUI countdownText;

    [Header("Debug")]
    public string regionId = "Zone";
    [SerializeField] private int deathCount = 0;
    [SerializeField] private bool playerInside = false;
    [SerializeField] private bool hazardsInactive = false;
    [SerializeField] private bool powerActive = false;
    [SerializeField] private float powerEndTime = 0f;

    private bool optionUnlocked = false;
    private float powerStartTime = 0f;
    private int lastBeepSecond = -1;
    private Coroutine flashCoroutine;

    void Awake()
    {
        if (hintPanel)
            hintPanel.SetActive(false);
        if (countdownPanel)
            countdownPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;
        currentRegion = this;

        if (optionUnlocked && !powerActive && hintPanel)
            hintPanel.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        if (currentRegion == this)
            currentRegion = null;

        if (hintPanel)
            hintPanel.SetActive(false);
    }

    void Update()
    {
        if (powerActive)
        {
            float remaining = powerEndTime - Time.time;
            if (remaining < 0f) remaining = 0f;

            if (countdownSlider)
            {
                float ratio = powerDuration > 0f ? remaining / powerDuration : 0f;
                countdownSlider.value = ratio;
            }

            int remainingInt = Mathf.CeilToInt(remaining);
            if (countdownText)
            {
                countdownText.text = Mathf.CeilToInt(remaining).ToString() + "s";
            }
            
            if (remainingInt <= 5 && remainingInt > 0)
            {
                if (remainingInt != lastBeepSecond)
                {
                    lastBeepSecond = remainingInt;
                    if (audioSource && countdownBeepClip)
                    {
                        audioSource.PlayOneShot(countdownBeepClip);
                    }
                }
            }

            if (Time.time >= powerEndTime)
            {
                EndPowerWindow();
            }
        }

        if (!playerInside) return;

        if (!powerActive)
        {
            if (optionUnlocked && Input.GetKeyDown(KeyCode.S))
            {
                StartPowerWindowAndTurnOffHazards();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                ToggleHazards();
            }
        }
    }

    public void RegisterDeathInRegion()
    {
        if (powerActive) return;

        deathCount++;
        Debug.Log($"[DeathRegion] {regionId} deathCount = {deathCount}");

        if (!optionUnlocked && deathCount >= deathThreshold)
        {
            optionUnlocked = true;

            if (playerInside && hintPanel)
                hintPanel.SetActive(true);
        }
    }

    private void StartPowerWindowAndTurnOffHazards()
    {
        powerActive = true;
        powerEndTime = Time.time + powerDuration;

        optionUnlocked = false;

        lastBeepSecond = -1;

        hazardsInactive = true;

        if (hintPanel)
            hintPanel.SetActive(false);

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }
        flashCoroutine = StartCoroutine(FlashAndDisableHazards());

        foreach (var h in hazardsToToggle)
        {
            if (h != null) h.SetActive(false);
        }

        if (countdownPanel)
            countdownPanel.SetActive(true);

        if (countdownSlider)
            countdownSlider.value = 1f;

        if (countdownText)
            countdownText.text = Mathf.CeilToInt(powerDuration).ToString() + "s";

        Debug.Log($"[DeathRegion] {regionId} power window START, duration = {powerDuration}s");
    }

    private void ToggleHazards()
    {
        hazardsInactive = !hazardsInactive;

        if (hazardsInactive)
        {
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
                flashCoroutine = null;
            }
            flashCoroutine = StartCoroutine(FlashAndDisableHazards());
        }
        else
        {
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
                flashCoroutine = null;
            }
        }

        foreach (var h in hazardsToToggle)
        {
            if (h != null) h.SetActive(!hazardsInactive);
        }

        Debug.Log($"[DeathRegion] {regionId} hazards now " + (hazardsInactive ? "INACTIVE" : "ACTIVE"));
    }

    private IEnumerator FlashAndDisableHazards()
    {
        var renderers = new List<SpriteRenderer>();
        foreach (var h in hazardsToToggle)
        {
            if (h == null) continue;
            renderers.AddRange(h.GetComponentsInChildren<SpriteRenderer>());
        }

        float elapsed = 0f;
        bool visible = true;

        while (elapsed < flashDuration)
        {
            visible = !visible;
            foreach (var r in renderers)
            {
                if (r != null)
                    r.enabled = visible;
            }
            elapsed += flashInterval;
            yield return new WaitForSeconds(flashInterval);
        }

        foreach (var r in renderers)
        {
            if (r != null)
                r.enabled = true;
        }

        foreach (var h in hazardsToToggle)
        {
            if (h != null)
                h.SetActive(false);
        }    
    }

    private void EndPowerWindow()
    {
        powerActive = false;
        hazardsInactive = false;

        lastBeepSecond = -1;

        foreach (var h in hazardsToToggle)
        {
            if (h != null) h.SetActive(true);
        }

        deathCount = 0;
        optionUnlocked = false;

        if (hintPanel)
            hintPanel.SetActive(false);

        if (countdownPanel)
            countdownPanel.SetActive(false);

        Debug.Log($"[DeathRegion] {regionId} power window END, hazards re-enabled, counter reset.");
    }

}
