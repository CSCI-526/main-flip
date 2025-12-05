using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SyncImage : MonoBehaviour
{
    public static readonly Dictionary<KeyCode, string> action2image_frame1 = new Dictionary<KeyCode, string> {
        [KeyCode.A] = "Keys/press_A_frame1.drawio",
        [KeyCode.B] = "Keys/press_B_frame1.drawio",
        [KeyCode.C] = "Keys/press_C_frame1.drawio",
        [KeyCode.D] = "Keys/press_D_frame1.drawio",
        [KeyCode.E] = "Keys/press_E_frame1.drawio",
        [KeyCode.F] = "Keys/press_F_frame1.drawio",
        [KeyCode.G] = "Keys/press_G_frame1.drawio",
        [KeyCode.H] = "Keys/press_H_frame1.drawio",
        [KeyCode.I] = "Keys/press_I_frame1.drawio",
        [KeyCode.J] = "Keys/press_J_frame1.drawio",
        [KeyCode.K] = "Keys/press_K_frame1.drawio",
        [KeyCode.L] = "Keys/press_L_frame1.drawio",
        [KeyCode.M] = "Keys/press_M_frame1.drawio",
        [KeyCode.N] = "Keys/press_N_frame1.drawio",
        [KeyCode.O] = "Keys/press_O_frame1.drawio",
        [KeyCode.P] = "Keys/press_P_frame1.drawio",
        [KeyCode.Q] = "Keys/press_Q_frame1.drawio",
        //[KeyCode.R] = "Keys/press_R_frame1.drawio",
        //[KeyCode.S] = "Keys/press_S_frame1.drawio",
        [KeyCode.T] = "Keys/press_T_frame1.drawio",
        [KeyCode.U] = "Keys/press_U_frame1.drawio",
        [KeyCode.V] = "Keys/press_V_frame1.drawio",
        [KeyCode.W] = "Keys/press_W_frame1.drawio",
        [KeyCode.X] = "Keys/press_X_frame1.drawio",
        [KeyCode.Y] = "Keys/press_Y_frame1.drawio",
        [KeyCode.Z] = "Keys/press_Z_frame1.drawio",

        [KeyCode.Alpha1] = "Keys/press_1_frame1.drawio",
        [KeyCode.Alpha2] = "Keys/press_2_frame1.drawio",
        [KeyCode.Alpha3] = "Keys/press_3_frame1.drawio",
        [KeyCode.Alpha4] = "Keys/press_4_frame1.drawio",
        [KeyCode.Alpha5] = "Keys/press_5_frame1.drawio",
        [KeyCode.Alpha6] = "Keys/press_6_frame1.drawio",
        [KeyCode.Alpha7] = "Keys/press_7_frame1.drawio",
        [KeyCode.Alpha8] = "Keys/press_8_frame1.drawio",
        [KeyCode.Alpha9] = "Keys/press_9_frame1.drawio",
        [KeyCode.Alpha0] = "Keys/press_0_frame1.drawio",
        
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
        //[KeyCode.Return] = "Keys/press_return_frame1.drawio",
        
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

    public static readonly Dictionary<KeyCode, string> action2image_frame2 = new Dictionary<KeyCode, string> {
        [KeyCode.A] = "Keys/press_A_frame2.drawio",
        [KeyCode.B] = "Keys/press_B_frame2.drawio",
        [KeyCode.C] = "Keys/press_C_frame2.drawio",
        [KeyCode.D] = "Keys/press_D_frame2.drawio",
        [KeyCode.E] = "Keys/press_E_frame2.drawio",
        [KeyCode.F] = "Keys/press_F_frame2.drawio",
        [KeyCode.G] = "Keys/press_G_frame2.drawio",
        [KeyCode.H] = "Keys/press_H_frame2.drawio",
        [KeyCode.I] = "Keys/press_I_frame2.drawio",
        [KeyCode.J] = "Keys/press_J_frame2.drawio",
        [KeyCode.K] = "Keys/press_K_frame2.drawio",
        [KeyCode.L] = "Keys/press_L_frame2.drawio",
        [KeyCode.M] = "Keys/press_M_frame2.drawio",
        [KeyCode.N] = "Keys/press_N_frame2.drawio",
        [KeyCode.O] = "Keys/press_O_frame2.drawio",
        [KeyCode.P] = "Keys/press_P_frame2.drawio",
        [KeyCode.Q] = "Keys/press_Q_frame2.drawio",
        //[KeyCode.R] = "Keys/press_R_frame2.drawio",
        //[KeyCode.S] = "Keys/press_S_frame2.drawio",
        [KeyCode.T] = "Keys/press_T_frame2.drawio",
        [KeyCode.U] = "Keys/press_U_frame2.drawio",
        [KeyCode.V] = "Keys/press_V_frame2.drawio",
        [KeyCode.W] = "Keys/press_W_frame2.drawio",
        [KeyCode.X] = "Keys/press_X_frame2.drawio",
        [KeyCode.Y] = "Keys/press_Y_frame2.drawio",
        [KeyCode.Z] = "Keys/press_Z_frame2.drawio",

        [KeyCode.Alpha1] = "Keys/press_1_frame2.drawio",
        [KeyCode.Alpha2] = "Keys/press_2_frame2.drawio",
        [KeyCode.Alpha3] = "Keys/press_3_frame2.drawio",
        [KeyCode.Alpha4] = "Keys/press_4_frame2.drawio",
        [KeyCode.Alpha5] = "Keys/press_5_frame2.drawio",
        [KeyCode.Alpha6] = "Keys/press_6_frame2.drawio",
        [KeyCode.Alpha7] = "Keys/press_7_frame2.drawio",
        [KeyCode.Alpha8] = "Keys/press_8_frame2.drawio",
        [KeyCode.Alpha9] = "Keys/press_9_frame2.drawio",
        [KeyCode.Alpha0] = "Keys/press_0_frame2.drawio",
        
        [KeyCode.BackQuote] = "Keys/press_backquote_frame2.drawio",
        [KeyCode.Minus] = "Keys/press_minus_frame2.drawio",
        [KeyCode.Equals] = "Keys/press_equals_frame2.drawio",
        [KeyCode.Backspace] = "Keys/press_backspace_frame2.drawio",
        
        [KeyCode.Tab] = "Keys/press_tab_frame2.drawio",
        [KeyCode.LeftBracket] = "Keys/press_leftbracket_frame2.drawio",
        [KeyCode.RightBracket] = "Keys/press_rightbracket_frame2.drawio",
        [KeyCode.Backslash] = "Keys/press_backslash_frame2.drawio",
        
        [KeyCode.Semicolon] = "Keys/press_semicolon_frame2.drawio",
        [KeyCode.Quote] = "Keys/press_quote_frame2.drawio",
        //[KeyCode.Return] = "Keys/press_return_frame2.drawio",
        
        [KeyCode.LeftShift] = "Keys/press_shift_frame2.drawio",
        [KeyCode.Comma] = "Keys/press_comma_frame2.drawio",
        [KeyCode.Period] = "Keys/press_period_frame2.drawio",
        [KeyCode.Slash] = "Keys/press_slash_frame2.drawio",
        [KeyCode.RightShift] = "Keys/press_shift_frame2.drawio",

        [KeyCode.LeftControl] = "Keys/press_ctrl_frame2.drawio",
        [KeyCode.LeftAlt] = "Keys/press_alt_frame2.drawio",
        [KeyCode.Space] = "Keys/press_space_frame2.drawio",
        [KeyCode.RightAlt] = "Keys/press_alt_frame2.drawio",
        [KeyCode.RightControl] = "Keys/press_ctrl_frame2.drawio",

        [KeyCode.LeftArrow] = "Keys/press_leftarrow_frame2.drawio",
        [KeyCode.RightArrow] = "Keys/press_rightarrow_frame2.drawio",
        [KeyCode.UpArrow] = "Keys/press_uparrow_frame2.drawio",
        [KeyCode.DownArrow] = "Keys/press_downarrow_frame2.drawio",
    };

    public static readonly Dictionary<bool, Dictionary<KeyCode, string>> frame2image = new Dictionary<bool, Dictionary<KeyCode, string>> {
        [true] = action2image_frame1,
        [false] = action2image_frame2,
    };

    public string actionName;
    public bool use_frame1 = true;
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
            if (frame2image[use_frame1].ContainsKey(key))
            {
                string path = frame2image[use_frame1][key];
                if (path.Split('/')[1] == keyImage.sprite.name)
                    return;

                //Sprite sprite = Resources.Load<Sprite>(path);
                Texture2D texture = Resources.Load<Texture2D>(path);
                if (texture != null)
                {
                    keyImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
                else
                {
                    Debug.LogError("Could not find sprite at path " + path);
                }
            }
        }
    }
}
