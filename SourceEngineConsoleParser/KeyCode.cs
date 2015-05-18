using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;

namespace SourceEngineConsoleParser
{
    public enum KeyCode : ushort
    {
        #region Numpad
        [Description("KP_INS")]
        NUMPAD0 = 0x60,
        [Description("KP_END")]
        NUMPAD1 = 0x61,
        [Description("KP_KP_DOWNARROW")]
        NUMPAD2 = 0x62,
        [Description("KP_PGDN")]
        NUMPAD3 = 0x63,
        [Description("KP_LEFTARROW")]
        NUMPAD4 = 100,
        [Description("KP_5")]
        NUMPAD5 = 0x65,
        [Description("KP_RIGHTARROW")]
        NUMPAD6 = 0x66,
        [Description("KP_HOME")]
        NUMPAD7 = 0x67,
        [Description("KP_UPARROW")]
        NUMPAD8 = 0x68,
        [Description("KP_PGUP")]
        NUMPAD9 = 0x69,
        [Description("KP_PLUS")]
        ADD = 0x6b,
        [Description("KP_MULTIPLY")]
        MULTIPLY = 0x6a,
        [Description("KP_SLASH")]
        DIVIDE = 0x6f,
        [Description("KP_MINUS")]
        SUBTRACT = 0x6d,
        #endregion

        #region Fkeys
        [Description("F1")]
        F1 = 0x70,
        [Description("F2")]
        F2 = 0x71,
        [Description("F3")]
        F3 = 0x72,
        [Description("F4")]
        F4 = 0x73,
        [Description("F5")]
        F5 = 0x74,
        [Description("F6")]
        F6 = 0x75,
        [Description("F7")]
        F7 = 0x76,
        [Description("F8")]
        F8 = 0x77,
        [Description("F9")]
        F9 = 120,
        [Description("F10")]
        F10 = 0x79,
        [Description("F11")]
        F11 = 0x7a,
        [Description("F12")]
        F12 = 0x7b,
        #endregion

        #region KEYS

        [Description("0")]
        KEY_0 = 0x30,
        [Description("1")]
        KEY_1 = 0x31,
        [Description("2")]
        KEY_2 = 50,
        [Description("3")]
        KEY_3 = 0x33,
        [Description("4")]
        KEY_4 = 0x34,
        [Description("5")]
        KEY_5 = 0x35,
        [Description("6")]
        KEY_6 = 0x36,
        [Description("7")]
        KEY_7 = 0x37,
        [Description("8")]
        KEY_8 = 0x38,
        [Description("9")]
        KEY_9 = 0x39,
        [Description("A")]
        KEY_A = 0x41,
        [Description("B")]
        KEY_B = 0x42,
        [Description("C")]
        KEY_C = 0x43,
        [Description("D")]
        KEY_D = 0x44,
        [Description("E")]
        KEY_E = 0x45,
        [Description("F")]
        KEY_F = 70,
        [Description("G")]
        KEY_G = 0x47,
        [Description("H")]
        KEY_H = 0x48,
        [Description("I")]
        KEY_I = 0x49,
        [Description("J")]
        KEY_J = 0x4a,
        [Description("K")]
        KEY_K = 0x4b,
        [Description("L")]
        KEY_L = 0x4c,
        [Description("M")]
        KEY_M = 0x4d,
        [Description("N")]
        KEY_N = 0x4e,
        [Description("O")]
        KEY_O = 0x4f,
        [Description("P")]
        KEY_P = 80,
        [Description("Q")]
        KEY_Q = 0x51,
        [Description("R")]
        KEY_R = 0x52,
        [Description("S")]
        KEY_S = 0x53,
        [Description("T")]
        KEY_T = 0x54,
        [Description("U")]
        KEY_U = 0x55,
        [Description("V")]
        KEY_V = 0x56,
        [Description("W")]
        KEY_W = 0x57,
        [Description("X")]
        KEY_X = 0x58,
        [Description("Y")]
        KEY_Y = 0x59,
        [Description("Z")]
        KEY_Z = 90,        
        [Description("BACKSPACE")]
        BACKSPACE = 8,
        [Description("CAPSLOCK")]
        CAPSLOCK = 20,
        [Description("CTRL")]
        CONTROL = 0x11,
        [Description("ALT")]
        ALT = 18,
        [Description("DEL")]
        DELETE = 0x2e,
        [Description("DOWNARROW")]
        DOWN = 40,
        [Description("END")]
        END = 0x23,
        [Description("ESCAPE")]
        ESC = 0x1b,
        [Description("HOME")]
        HOME = 0x24,
        [Description("INS")]
        INSERT = 0x2d,
        [Description("CTRL")]
        LCONTROL = 0xa2,
        [Description("LEFTARROW")]
        LEFT = 0x25,
        [Description("SHIFT")]
        LSHIFT = 160,
        [Description("PGDN")]
        PAGEDOWN = 0x22,
        [Description("NUMLOCK")]
        NUMLOCK = 0x90,
        [Description("PGUP")]
        PAGE_UP = 0x21,
        [Description("ENTER")]
        ENTER = 13,
        [Description("RIGHTARROW")]
        RIGHT = 0x27,
        [Description("SHIFT")]
        SHIFT = 0x10,
        [Description("SPACEBAR")]
        SPACEBAR = 0x20,
        [Description("TAB")]
        TAB = 9,
        [Description("UPARROW")]
        UP = 0x26,
        #endregion
    }

    public static class KeyUtilities
    {
        public static KeyCode GetKeyCodeFromDescription(String desc)
        {
            return Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().FirstOrDefault(v => v.GetDescription() == desc);
        }

        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }
    }

}
