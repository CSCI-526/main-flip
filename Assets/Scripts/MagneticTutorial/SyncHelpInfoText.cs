using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SyncHelpInfoText : MonoBehaviour
{
    private TMP_Text textComponent;
    private InputManager inputManager;

    void Start()
    {
        textComponent = GetComponent<TMP_Text>();
        inputManager = InputManager.Instance;
    }

    void Update()
    {
        string keyName = inputManager.keyMappings["SwitchMagnetic"].ToString();
        Debug.Log(keyName);
        if (keyName == "LeftShift" || keyName == "RightShift")
            keyName = "Shift";
        if (keyName == "LeftControl" || keyName == "RightControl")
            keyName = "Ctrl";
        if (keyName == "LeftAlt" || keyName == "RightAlt")
            keyName = "Alt";
        textComponent.text = "Press [" + keyName + "]\nto switch magnetic polarity";
    }
}
