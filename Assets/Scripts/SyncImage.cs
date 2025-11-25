using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SyncImage : MonoBehaviour
{
    public static readonly Dictionary<KeyCode, string> action2image = new Dictionary<KeyCode, string> {
        [KeyCode.BackQuote] = "Keys/press_backquote_frame1.drawio",
        [KeyCode.Minus] = "Keys/press_minus_frame1.drawio",
        [KeyCode.Equals] = "Keys/press_equals_frame1.drawio",
        [KeyCode.Backspace] = "Keys/press_backspace_frame1.drawio",
        
        [KeyCode.Tab] = "Keys/press_tab_frame1.drawio",
        [KeyCode.LeftBracket] = "Keys/press_leftbracket_frame1.drawio",
        [KeyCode.RightBracket] = "Keys/press_rightbracket_frame1.drawio",
        [KeyCode.Backslash] = "Keys/press_backslash_frame1.drawio",
        
        [KeyCode.Semicolon] = "Keys/press_semicolon_frame1.drawio",
        [KeyCode.Quote] = "Keys/press_quote_frame1.drawio",
        [KeyCode.Return] = "Keys/press_return_frame1.drawio",
        
        [KeyCode.LeftShift] = "Keys/press_shift_frame1.drawio",
        [KeyCode.Comma] = "Keys/press_comma_frame1.drawio",
        [KeyCode.Period] = "Keys/press_period_frame1.drawio",
        [KeyCode.Slash] = "Keys/press_slash_frame1.drawio",
        [KeyCode.RightShift] = "Keys/press_shift_frame1.drawio",

        [KeyCode.LeftControl] = "Keys/press_ctrl_frame1.drawio",
        [KeyCode.LeftAlt] = "Keys/press_alt_frame1.drawio",
        [KeyCode.Space] = "Keys/press_space_frame1.drawio",
        [KeyCode.RightAlt] = "Keys/press_alt_frame1.drawio",
        [KeyCode.RightControl] = "Keys/press_ctrl_frame1.drawio",

        [KeyCode.LeftArrow] = "Keys/press_leftarrow_frame1.drawio",
        [KeyCode.RightArrow] = "Keys/press_rightarrow_frame1.drawio",
        [KeyCode.UpArrow] = "Keys/press_uparrow_frame1.drawio",
        [KeyCode.DownArrow] = "Keys/press_downarrow_frame1.drawio",
    };
    public string actionName;
    private Image keyImage;
    private InputManager inputManager;

    void Start()
    {
        keyImage = GetComponent<Image>();
        inputManager = InputManager.Instance;
    }

    void Update()
    {
        if (inputManager.keyMappings.ContainsKey(actionName))
        {
            KeyCode key = inputManager.keyMappings[actionName];
            if (action2image.ContainsKey(key))
            {
                string path = action2image[key];
                if (path.Split('/')[1] == keyImage.sprite.name)
                    return;

                Sprite sprite = Resources.Load<Sprite>(path);
                if (sprite != null)
                {
                    keyImage.sprite = sprite;
                }
                else
                {
                    Debug.LogError("Could not find sprite at path " + path);
                }
            }
        }
    }
}
