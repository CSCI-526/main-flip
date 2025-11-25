using UnityEngine;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    public static InputManager _instance;
    public Dictionary<string, KeyCode> keyMappings;
    
    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InputManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("InputManager");
                    _instance = go.AddComponent<InputManager>();
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            keyMappings = new Dictionary<string, KeyCode>();
            keyMappings.Add("SwitchGravity", KeyCode.Space);
            keyMappings.Add("SwitchGravityAlt", KeyCode.Space);
            keyMappings.Add("SwitchMagnetic", KeyCode.LeftShift);
            keyMappings.Add("SwitchMagneticAlt", KeyCode.RightShift);
            keyMappings.Add("Respawn", KeyCode.R);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetKey(string actionName, KeyCode newKey)
    {
        if (keyMappings.ContainsKey(actionName))
        {
            keyMappings[actionName] = newKey;
        }
    }

    public void ClearKeys(string actionName)
    {
        if (keyMappings.ContainsKey(actionName))
        {
            keyMappings[actionName] = KeyCode.None;
        }
    }
}
