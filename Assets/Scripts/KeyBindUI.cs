using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class KeyBindUI : MonoBehaviour
{
    [System.Serializable]
    public class KeyUIItem
    {
        public string actionName;
        public TMP_Text keyDisplayText;
        public Button rebindButton;
        public TMP_Text conflictDisplayText;
    }
    public List<KeyUIItem> keyItems = new List<KeyUIItem>();
    private string currentActionToRebind = null;
    private bool isWaitingForKey = false;
    public bool isRebinding = false;
    public GameObject pausePanel;

    void Start()
    {
        UpdateAllKeyTexts();
    }

    void Update()
    {
        if (!isWaitingForKey)
            return;
    }

    void OnGUI()
    {
        if (isWaitingForKey && currentActionToRebind != null)
        {
            Event e = Event.current;
            if (e.isKey && e.type == EventType.KeyDown)
            {
                if (e.keyCode == KeyCode.Escape)
                {
                    isWaitingForKey = false;
                    StartCoroutine(StopRebinding());
                    return;
                }

                foreach (var actionName in InputManager.Instance.keyMappings.Keys)
                {
                    if (!actionName.Contains(currentActionToRebind)) {
                        if (InputManager.Instance.keyMappings[actionName] == e.keyCode)
                        {
                            var item = keyItems.Find(x => x.actionName == actionName);
                            StartCoroutine(ShowConflictMessage(keyItems.Find(x => x.actionName == currentActionToRebind).conflictDisplayText.gameObject));
                            isWaitingForKey = false;
                            StartCoroutine(StopRebinding());
                            return;
                        }
                    } 
                }

                InputManager.Instance.SetKey(currentActionToRebind, e.keyCode);
                if (e.keyCode == KeyCode.LeftShift)
                {
                    InputManager.Instance.SetKey(currentActionToRebind+"Alt", KeyCode.RightShift);
                }
                else if (e.keyCode == KeyCode.RightShift)
                {
                    InputManager.Instance.SetKey(currentActionToRebind+"Alt", KeyCode.LeftShift);
                }
                else
                {
                    InputManager.Instance.SetKey(currentActionToRebind+"Alt", e.keyCode);
                }
                Debug.Log($"Function {currentActionToRebind} changed to {e.keyCode}");

                UpdateKeyText(currentActionToRebind, e.keyCode);
                isWaitingForKey = false;
                StartCoroutine(StopRebinding());
            }
        }
    }

    public void StartRebinding(string actionName)
    {
        if (isWaitingForKey) return;

        currentActionToRebind = actionName;
        isWaitingForKey = true;
        isRebinding = true;

        var item = keyItems.Find(x => x.actionName == actionName);
        if (item != null)
        {
            item.keyDisplayText.text = "Press any key..."; 
        }
    }

    private IEnumerator StopRebinding()
    {
        yield return null;
        yield return null;
        yield return null;
        currentActionToRebind = null;
        isRebinding = false;
        UpdateAllKeyTexts();
    }

    private IEnumerator ShowConflictMessage(GameObject messageObject)
    {
        messageObject.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        messageObject.SetActive(false);
    }

    private void UpdateAllKeyTexts()
    {
        foreach (var item in keyItems)
        {
            if (InputManager.Instance.keyMappings.TryGetValue(item.actionName, out KeyCode key))
            {
                if (key == KeyCode.None)
                {
                    item.keyDisplayText.text = "Unbound";
                }
                else if (key == KeyCode.LeftShift || key == KeyCode.RightShift)
                {
                    item.keyDisplayText.text = "Shift";
                }
                else {
                    item.keyDisplayText.text = key.ToString();
                }
            }
        }
    }

    private void UpdateKeyText(string actionName, KeyCode newKey)
    {
        var item = keyItems.Find(x => x.actionName == actionName);
        if (item != null)
        {
            if (newKey == KeyCode.None)
            {
                item.keyDisplayText.text = "Unbound";
            }
            else if (newKey == KeyCode.LeftShift || newKey == KeyCode.RightShift)
            {
                item.keyDisplayText.text = "Shift";
            }
            else {
                item.keyDisplayText.text = newKey.ToString();
            }
        }
    }

    public void Show()
    {   
        pausePanel.SetActive(false);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void Reset()
    {
        InputManager.Instance.Reset();
        UpdateAllKeyTexts();
    }
}
