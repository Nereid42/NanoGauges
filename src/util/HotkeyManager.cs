using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Nereid
{
   namespace NanoGauges
   {
      public class HotkeyManager
      {
         private class Hotkey
         {
            public KeyCode code { get; set; }
            public bool enabled { get; set; }

            public Hotkey(KeyCode code, bool enabled)
            {
               this.code = code;
               this.enabled = enabled;
            }

            public Hotkey(KeyCode code)
            {
               this.code = code;
               this.enabled = true;
            }
         }

         public const int HOTKEY_MAIN    = 0;
         public const int HOTKEY_HIDE    = 1;
         public const int HOTKEY_NEXTSET = 2;
         public const int HOTKEY_PREVSET = 3;
         public const int HOTKEY_SET_STANDARD = 4;
         public const int HOTKEY_SET_LAUNCH = 5;
         public const int HOTKEY_SET_LAND = 6;
         public const int HOTKEY_SET_DOCK = 7;
         public const int HOTKEY_SET_ORBIT = 8;
         public const int HOTKEY_SET_FLIGHT = 9;
         public const int HOTKEY_SET_SET1 = 10;
         public const int HOTKEY_SET_SET2 = 11;
         public const int HOTKEY_SET_SET3 = 12;
         public const int HOTKEY_SET_ENABLE_ALL = 13;
         public const int HOTKEY_WINDOW_CONFIG = 14;
         public const int HOTKEY_LAYOUT = 15;
         private const int HOTKEY_COUNT = 16;

         private Hotkey[] hotkeys;

         public HotkeyManager()
         {
            hotkeys = new Hotkey[Count()];
            SetDefaultHotkeys();
         }

         public int Count()
         {
            return (int)HOTKEY_COUNT;
         }

         public void SetDefaultHotkeys()
         {
            hotkeys[HOTKEY_MAIN] = new Hotkey(KeyCode.RightControl);
            hotkeys[HOTKEY_HIDE] = new Hotkey(KeyCode.Numlock);
            hotkeys[HOTKEY_NEXTSET] = new Hotkey(KeyCode.PageDown);
            hotkeys[HOTKEY_PREVSET] = new Hotkey(KeyCode.PageUp);
            hotkeys[HOTKEY_SET_STANDARD] = new Hotkey(KeyCode.Keypad1);
            hotkeys[HOTKEY_SET_LAUNCH] = new Hotkey(KeyCode.Keypad2);
            hotkeys[HOTKEY_SET_LAND] = new Hotkey(KeyCode.Keypad3);
            hotkeys[HOTKEY_SET_DOCK] = new Hotkey(KeyCode.Keypad4);
            hotkeys[HOTKEY_SET_ORBIT] = new Hotkey(KeyCode.Keypad5);
            hotkeys[HOTKEY_SET_FLIGHT] = new Hotkey(KeyCode.Keypad6);
            hotkeys[HOTKEY_SET_SET1] = new Hotkey(KeyCode.Keypad7);
            hotkeys[HOTKEY_SET_SET2] = new Hotkey(KeyCode.Keypad8);
            hotkeys[HOTKEY_SET_SET3] = new Hotkey(KeyCode.Keypad9);
            hotkeys[HOTKEY_SET_ENABLE_ALL] = new Hotkey(KeyCode.Keypad0);
            hotkeys[HOTKEY_WINDOW_CONFIG] = new Hotkey(KeyCode.KeypadEnter);
            hotkeys[HOTKEY_LAYOUT] = new Hotkey(KeyCode.Backspace);
         }

         public bool GetKey(int id)
         {
            if (id >= Count() || id < 0) return false;
            Hotkey hotkey = hotkeys[id];
            if (hotkey.enabled)
            {
               return Input.GetKey(hotkey.code);
            }
            return false;
         }

         public bool GetKeyDown(int id)
         {
            if (id >= Count() || id <0) return false;
            Hotkey hotkey = hotkeys[id];
            if(hotkey.enabled)
            {
               return Input.GetKeyDown(hotkey.code);
            }
            return false;
         }

         public void SetKeyCode(int id, KeyCode code)
         {
            if (id >= Count() || id < 0) return;
            Log.Info("set keycode for hotkey "+id+" to "+(int)code);
            Hotkey hotkey = hotkeys[id];
            hotkey.code = code;
         }

         public void SetEnabled(int id, bool enabled)
         {
            if (id >= Count() || id < 0) return;
            Log.Info("set hotkey " + id + " to " + (enabled?"enabled":"disabled"));
            Hotkey hotkey = hotkeys[id];
            hotkey.enabled = enabled;
         }

         public KeyCode GetKeyCode(int id)
         {
            if (id >= Count() || id < 0) return 0;
            Hotkey hotkey = hotkeys[id];
            return hotkey.code;
         }
      }
   }
}
