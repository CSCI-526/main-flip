using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    [Header("Debug")]
    public string regionId = "Zone";
    [SerializeField] private int deathCount = 0;
    [SerializeField] private bool playerInside = false;
    [SerializeField] private bool hazardsInactive = false;
    [SerializeField] private bool powerActive = false;
    [SerializeField] private float powerEndTime = 0f;

    private bool optionUnlocked = false;

    void Awake()
    {
        if (hintPanel)
            hintPanel.SetActive(false);
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
        if (powerActive && Time.time >= powerEndTime)
        {
            EndPowerWindow();
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

        hazardsInactive = true;
        foreach (var h in hazardsToToggle)
        {
            if (h != null) h.SetActive(false);
        }

        if (hintPanel)
            hintPanel.SetActive(true);

        Debug.Log($"[DeathRegion] {regionId} power window START, duration = {powerDuration}s");
    }

    private void ToggleHazards()
    {
        hazardsInactive = !hazardsInactive;

        foreach (var h in hazardsToToggle)
        {
            if (h != null) h.SetActive(!hazardsInactive);
        }

        Debug.Log($"[DeathRegion] {regionId} hazards now " + (hazardsInactive ? "INACTIVE" : "ACTIVE"));
    }

    private void EndPowerWindow()
    {
        powerActive = false;
        hazardsInactive = false;

        foreach (var h in hazardsToToggle)
        {
            if (h != null) h.SetActive(true);
        }

        deathCount = 0;
        optionUnlocked = false;

        if (hintPanel)
            hintPanel.SetActive(false);

        Debug.Log($"[DeathRegion] {regionId} power window END, hazards re-enabled, counter reset.");
    }

}
