using System;
using System.Collections.Generic;


/// @brief 키테이블 딕셔너리
public static class KeyTables {
    public static readonly Dictionary<string, KeyTable> KeyTableDictionary = new Dictionary<string, KeyTable>();

    public static readonly KeyTable[] keyTables = {
        new KeyTable("A", 0x1C, 0xF01C, 0x1E, (byte) 'A'),
        new KeyTable("B", 0x32, 0xF032, 0x30, (byte) 'B'),
        new KeyTable("C", 0x21, 0xF021, 0x2E, (byte) 'C'),
        new KeyTable("D", 0x23, 0xF023, 0x20, (byte) 'D'),
        new KeyTable("E", 0x24, 0xF024, 0x12, (byte) 'E'),
        new KeyTable("F", 0x2B, 0xF02B, 0x21, (byte) 'F'),
        new KeyTable("G", 0x34, 0xF034, 0x22, (byte) 'G'),
        new KeyTable("H", 0x33, 0xF033, 0x23, (byte) 'H'),
        new KeyTable("I", 0x43, 0xF043, 0x17, (byte) 'I'),
        new KeyTable("J", 0x3B, 0xF03B, 0x24, (byte) 'J'),
        new KeyTable("K", 0x42, 0xF042, 0x25, (byte) 'K'),
        new KeyTable("L", 0x4B, 0xF04B, 0x26, (byte) 'L'),
        new KeyTable("M", 0x3A, 0xF03A, 0x32, (byte) 'M'),
        new KeyTable("N", 0x31, 0xF031, 0x31, (byte) 'N'),
        new KeyTable("O", 0x44, 0xF044, 0x18, (byte) 'O'),
        new KeyTable("P", 0x4D, 0xF04D, 0x19, (byte) 'P'),
        new KeyTable("Q", 0x15, 0xF015, 0x10, (byte) 'Q'),
        new KeyTable("R", 0x2D, 0xF02D, 0x13, (byte) 'R'),
        new KeyTable("S", 0x1B, 0xF01B, 0x1F, (byte) 'S'),
        new KeyTable("T", 0x2C, 0xF02C, 0x14, (byte) 'T'),
        new KeyTable("U", 0x3C, 0xF03C, 0x16, (byte) 'U'),
        new KeyTable("V", 0x2A, 0xF02A, 0x2F, (byte) 'V'),
        new KeyTable("W", 0x1D, 0xF01D, 0x11, (byte) 'W'),
        new KeyTable("X", 0x22, 0xF022, 0x2D, (byte) 'X'),
        new KeyTable("Y", 0x35, 0xF035, 0x15, (byte) 'Y'),
        new KeyTable("Z", 0x1A, 0xF01A, 0x2C, (byte) 'Z'),
        new KeyTable("Alpha0", 0x45, 0xF045, 0x0B, (byte) '0'),
        new KeyTable("Alpha1", 0x16, 0xF016, 0x02, (byte) '1'),
        new KeyTable("Alpha2", 0x1E, 0xF01E, 0x03, (byte) '2'),
        new KeyTable("Alpha3", 0x26, 0xF026, 0x04, (byte) '3'),
        new KeyTable("Alpha4", 0x25, 0xF025, 0x05, (byte) '4'),
        new KeyTable("Alpha5", 0x2E, 0xF02E, 0x06, (byte) '5'),
        new KeyTable("Alpha6", 0x36, 0xF036, 0x07, (byte) '6'),
        new KeyTable("Alpha7", 0x3D, 0xF03D, 0x08, (byte) '7'),
        new KeyTable("Alpha8", 0x3E, 0xF03E, 0x09, (byte) '8'),
        new KeyTable("Alpha9", 0x46, 0xF046, 0x0A, (byte) '9'),
        new KeyTable("BackQuote", 0x0E, 0xF00E, 0x29, 192),
        new KeyTable("Minus", 0x4E, 0xF04E, 0x0C, 189),
        new KeyTable("Equals", 0x55, 0xF055, 0x0D, 187),
        new KeyTable("Backslash", 0x5D, 0xF05D, 0x2B, 220),
        new KeyTable("Backspace", 0x66, 0xF066, 0x0E, 0x08),
        new KeyTable("Space", 0x29, 0xF029, 0x39, 0x20),
        new KeyTable("Tab", 0x0D, 0xF00D, 0x0F, 0x09),
        new KeyTable("CapsLock", 0x58, 0xF058, 0x3A, 0x14),
        new KeyTable("LeftShift", 0x12, 0xF012, 0x2A, 0xA0),
        new KeyTable("LeftControl", 0x14, 0xF014, 0x1D, 0xA2),
        //new KeyTable("LeftWindows", 0xE01F, 0xE0F01F, 0xDB, 0x5B),    
        new KeyTable("LeftAlt", 0x11, 0xF011, 0x38, 0xA4),
        new KeyTable("RightShift", 0x59, 0xF059, 0x36, 0xA1),
        new KeyTable("RightControl", 0xE014, 0xE0F014, 0x9D, 0xA3),
        //new KeyTable("RightWindows", 0xE027, 0xE0F027, 0xDC, 0x5C),
        new KeyTable("RightAlt", 0xE011, 0xE0F011, 0xB8, 0x15), // 한영변환으로 바인딩
        new KeyTable("APPS", 0xE02F, 0xE0F02F, 0xDD, 0x5D),
        new KeyTable("Return", 0x5A, 0xF05A, 0x1C, 0x0D),
        new KeyTable("Escape", 0x76, 0xF076, 0x01, 0x1B),
        new KeyTable("F1", 0x05, 0xF005, 0x3B,0x70),
        new KeyTable("F2", 0x06, 0xF006, 0x3C, 0x71),
        new KeyTable("F3", 0x04, 0xF004, 0x3D, 0x72),
        new KeyTable("F4", 0x0C, 0xF00C, 0x3E, 0x73),
        new KeyTable("F5", 0x03, 0xF003, 0x3F, 0x74),
        new KeyTable("F6", 0x0B, 0xF00B, 0x40, 0x75),
        new KeyTable("F7", 0x83, 0xF083, 0x41, 0x76),
        new KeyTable("F8", 0x0A, 0xF00A, 0x42, 0x77),
        new KeyTable("F9", 0x01, 0xF001, 0x43, 0x78),
        new KeyTable("F10", 0x09, 0xF009, 0x44, 0x79),
        new KeyTable("F11", 0x78, 0xF078, 0x57, 0x7A),
        new KeyTable("F12", 0x07, 0xF007, 0x58, 0x7B),
        new KeyTable("Print", 0xE012, 0xE0F012, 0xB7, 0x2C),
        new KeyTable("ScrollLock", 0x7E, 0xF07E, 0x46, 0x91),
        new KeyTable("Pause", 0xE11D45, 0xE19DC5, 0x45, 0x13),
        new KeyTable("LeftBracket", 0x54, 0xF054, 0x1A, 219),
        new KeyTable("Insert", 0xE070, 0xE0F070, 0xD2, 0x2D),
        new KeyTable("Home", 0xE06C, 0xE0F06C, 0xC7, 0x24),
        new KeyTable("PageUp", 0xE07D, 0xE0F07D, 0xC9, 0x21),
        new KeyTable("Delete", 0xE071, 0xE0F071, 0xD3, 0x2E),
        new KeyTable("End", 0xE069, 0xE0F069, 0xCF, 0x23),
        new KeyTable("PageDown", 0xE07A, 0xE0F07A, 0xD1,0x22),
        new KeyTable("UpArrow", 0xE075, 0xE0F075, 0xC8, 0x26),
        new KeyTable("LeftArrow", 0xE06B, 0xE0F06B, 0xCB, 0x25),
        new KeyTable("DownArrow", 0xE072, 0xE0F072, 0xD0, 0x28),
        new KeyTable("RightArrow", 0xE074, 0xE0F074, 0xCD, 0x27),
        new KeyTable("Numlock", 0x77, 0xF077, 0x45, 0x90),
        new KeyTable("KeypadDivide", 0xE04A, 0xE0F04A, 0xB5, 0x6F),
        new KeyTable("KeypadMultiply", 0x7C, 0xF07C, 0x37, 0x6A),
        new KeyTable("KeypadMinus", 0x7B, 0xF07B, 0x4A, 0x6D),
        new KeyTable("KeypadPlus", 0x79, 0xF079, 0x4E, 0x6B),
        new KeyTable("KeypadEnter", 0xE05A, 0xE0F05A, 0x9C, 0x0D),
        new KeyTable("KeypadPeriod", 0x71, 0xF071, 0x53, 0x6E),
        new KeyTable("Keypad0", 0x70, 0xF070, 0x52, 0x60),
        new KeyTable("Keypad1", 0x69, 0xF069, 0x4F, 0x61),
        new KeyTable("Keypad2", 0x72, 0xF072, 0x50, 0x62),
        new KeyTable("Keypad3", 0x7A, 0xF07A, 0x51, 0x63),
        new KeyTable("Keypad4", 0x6B, 0xF06B, 0x4B, 0x64),
        new KeyTable("Keypad5", 0x73, 0xF073, 0x4C, 0x65),
        new KeyTable("Keypad6", 0x74, 0xF074, 0x4D, 0x66),
        new KeyTable("Keypad7", 0x6C, 0xF06C, 0x47, 0x67),
        new KeyTable("Keypad8", 0x75, 0xF075, 0x48, 0x68),
        new KeyTable("Keypad9", 0x7D, 0xF07D, 0x49, 0x69),
        new KeyTable("RightBracket", 0x5B, 0xF05B, 0x1B, 221),
        new KeyTable("Semicolon", 0x4C, 0xF04C, 0x27, 186),
        new KeyTable("Comma", 0x41, 0xF041, 0x33, 188),
        new KeyTable("Period", 0x49, 0xF049, 0x34, 190),
        new KeyTable("Slash", 0x4A, 0xF04A, 0x35, 191),
        new KeyTable("enemy_align", 0x60, 0xF060, 0xE0, 0x24),
        new KeyTable("cbr", 0xA7, 0xF0A7, 0xE1),
        new KeyTable("enemy", 0xA8, 0xF0A8, 0xE2),
        new KeyTable("nextw", 0xAF, 0xF0AF, 0xE3),
        new KeyTable("4wayup", 0xC5, 0xF0C5, 0xE4, 0x26),
        new KeyTable("4waydown", 0xC6, 0xF0C6, 0xE5, 0x28),
        new KeyTable("4wayleft", 0xC7, 0xF0C7, 0xE6, 0x25),
        new KeyTable("4wayright", 0xC8, 0xF0C8, 0xE7, 0x27),
        //new KeyTable("change", 0x90, 0xF090, 0xE8,0x15),
        //new KeyTable("change", 0xE011, 0xE0F011, 0xE8, 0x15),
        // new KeyTable("Hangul", 0xE011, 0xE0F011, 0xF2, 0x15), // 가상키 0x15
        new KeyTable("Quote", 0x52, 0xF052, 0x28, 0xDE),
        new KeyTable("LessThan", 0x41, 0xF041, 0x33, 226),
        new KeyTable("GreaterThan", 0x49, 0xF049, 0x34, 227),

    };

    static KeyTables() {
        InitializeKeyTables();
    }

    static void InitializeKeyTables() {
        foreach (var keyTable in keyTables) {
            KeyTableDictionary[keyTable.name] = keyTable;
        }
    }
}

/// @brief 키테이블 사용값 , 키보드의 내부 인자's
public class KeyTable {
    public string name;
    public long make_val;
    public long break_val;
    public byte scan_key;
    public byte os_vk_key;
    public byte[] make_str = new byte[8];
    public byte[] break_str = new byte[8];
    public int make_str_len;
    public int break_str_len;

    public KeyTable(string name, long make_val, long break_val, byte scan_key, byte os_vk_key = 0) {
        this.name = name;
        this.make_val = make_val;
        this.break_val = break_val;
        this.scan_key = scan_key;
        this.os_vk_key = os_vk_key;

        this.make_str_len = MakeKeyString(this.make_str, this.make_val);
        this.break_str_len = MakeKeyString(this.break_str, this.break_val);
    }

    /// @brief 입력된 long 값을 바이트 배열로 변환하여 키 문자열을 make
    /// @param[in] dest 변환된 키 문자열을 저장할 바이트 배열
    /// @param[in] input 변환할 long 값
    /// @return 변환된 키 문자열의 길이
    private int MakeKeyString(byte[] dest, long input) {
        byte[] temp = new byte[8];
        int len = 0;

        BitConverter.GetBytes(input).CopyTo(temp, 0);
        Array.Clear(dest, 0, dest.Length);

        for (int i = 0; i < temp.Length; i++) {
            if (temp[i] == 0x00) break;
            dest[i] = temp[i];
            len++;
        }

        if (len == 0) return 0;

        int j = 0;
        for (int i = len; i > 0; i--) {
            dest[j] = temp[i - 1];
            j++;
        }

        return len;
    }
}
