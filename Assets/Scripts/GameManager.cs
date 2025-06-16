using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Dictionary<KeyCode, int> keyToButtonIndex = new();
    [SerializeField] private UiManager uiManager;

    void Start()
    {
        // 예: F1~F8 키 → 1~8번 버튼
        for (int i = 1; i <= 8; i++)
        {
            keyToButtonIndex.Add(KeyCode.F1 + (i - 1), i);
        }
    }
    void Update()
    {
        foreach (var kvp in keyToButtonIndex)
        {
            if (Input.GetKeyDown(kvp.Key))
            {
                uiManager.TriggerButtonByIndex(kvp.Value);
            }
        }
    }
    // 기존 OnKeyReceived도 필요 시 유지
    public void OnKeyReceived(byte vkCode, bool isDown)
    {
        KeyCode key = MapVKToKeyCode(vkCode);
        if (isDown && keyToButtonIndex.TryGetValue(key, out var index))
        {
            uiManager.TriggerButtonByIndex(index);
        }
    }
        
    private KeyCode MapVKToKeyCode(byte vkCode) {
        // Function keys
        if (vkCode >= 0x70 && vkCode <= 0x7B) // F1 ~ F12
            return KeyCode.F1 + (vkCode - 0x70);

        // Number keys (top row)
        if (vkCode >= 0x30 && vkCode <= 0x39) // '0' to '9'
            return KeyCode.Alpha0 + (vkCode - 0x30);

        // A ~ Z
        if (vkCode >= 0x41 && vkCode <= 0x5A) // 'A' to 'Z'
            return KeyCode.A + (vkCode - 0x41);

        // Numpad
        if (vkCode >= 0x60 && vkCode <= 0x69) // Numpad 0 ~ 9
            return KeyCode.Keypad0 + (vkCode - 0x60);

        // Arithmetic keys (numpad)
        return vkCode switch
        {
            0x6A => KeyCode.KeypadMultiply,
            0x6B => KeyCode.KeypadPlus,
            0x6D => KeyCode.KeypadMinus,
            0x6E => KeyCode.KeypadPeriod,
            0x6F => KeyCode.KeypadDivide,

            // Modifier keys
            0x10 => KeyCode.LeftShift,
            0xA0 => KeyCode.LeftShift,
            0xA1 => KeyCode.RightShift,
            0x11 => KeyCode.LeftControl,
            0xA2 => KeyCode.LeftControl,
            0xA3 => KeyCode.RightControl,
            0x12 => KeyCode.LeftAlt,
            0xA4 => KeyCode.LeftAlt,
            0xA5 => KeyCode.RightAlt,

            // Navigation & control keys
            0x08 => KeyCode.Backspace,
            0x09 => KeyCode.Tab,
            0x0D => KeyCode.Return,
            0x13 => KeyCode.Pause,
            0x14 => KeyCode.CapsLock,
            0x1B => KeyCode.Escape,
            0x20 => KeyCode.Space,
            0x21 => KeyCode.PageUp,
            0x22 => KeyCode.PageDown,
            0x23 => KeyCode.End,
            0x24 => KeyCode.Home,
            0x25 => KeyCode.LeftArrow,
            0x26 => KeyCode.UpArrow,
            0x27 => KeyCode.RightArrow,
            0x28 => KeyCode.DownArrow,
            0x2C => KeyCode.Print,
            0x2D => KeyCode.Insert,
            0x2E => KeyCode.Delete,
            
            // OEM/special characters
            0xBA => KeyCode.Semicolon,     // ;
            0xBB => KeyCode.Equals,        // =
            0xBC => KeyCode.Comma,         // ,
            0xBD => KeyCode.Minus,         // -
            0xBE => KeyCode.Period,        // .
            0xBF => KeyCode.Slash,         // /
            0xC0 => KeyCode.BackQuote,     // `
            0xDB => KeyCode.LeftBracket,   // [
            0xDC => KeyCode.Backslash,     // \
            0xDD => KeyCode.RightBracket,  // ]
            0xDE => KeyCode.Quote,         // '

            // Application-specific keys
            0x5B => KeyCode.LeftWindows,
            0x5C => KeyCode.RightWindows,
            0x5D => KeyCode.Menu,          // Application key

            // NumLock & ScrollLock
            0x90 => KeyCode.Numlock,
            0x91 => KeyCode.ScrollLock,

            _ => KeyCode.None,
        };
    }

    // public void OnKeyReceived(byte vkCode, bool isDown)
    // {
    //     // VK 코드를 Unity KeyCode로 매핑
    //     KeyCode key = MapVKToKeyCode(vkCode);
    //     if (isDown && keyBindings.TryGetValue(key, out var action))
    //     {
    //         action?.Invoke();
    //     }
    // }

}