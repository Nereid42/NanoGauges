using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;

namespace Nereid
{
   namespace NanoGauges
   {
      public class HotkeyManager
      {
         private const String CONFIGFILE_MARK = "HOTKEYS";

         private class Hotkey
         {
            public KeyCode code { get; set; }
            public bool enabled { get; set; }
            // this will be set to true, if the hotkey defined (pressed) by the user in the config window
            public Reference<bool> input = new Reference<bool>();

            public Hotkey(KeyCode code, bool enabled)
            {
               this.code = code;
               this.enabled = enabled;
               this.input.value = false;
            }

            public Hotkey(KeyCode code)
               : this(code, true)
            {
            }

            public override String ToString()
            {
               return code.ToString() + " (" + (enabled?"enabled":"disabled") + ")";
            }
         }

         public const int HOTKEY_MAIN    = 0;
         public const int HOTKEY_CLOSE_AND_DRAG = 1;
         public const int HOTKEY_HIDE = 2;
         public const int HOTKEY_NEXTSET = 3;
         public const int HOTKEY_PREVSET = 4;
         public const int HOTKEY_SET_STANDARD = 5;
         public const int HOTKEY_SET_LAUNCH = 6;
         public const int HOTKEY_SET_LAND = 7;
         public const int HOTKEY_SET_DOCK = 8;
         public const int HOTKEY_SET_ORBIT = 9;
         public const int HOTKEY_SET_FLIGHT = 10;
         public const int HOTKEY_SET_SET1 = 11;
         public const int HOTKEY_SET_SET2 = 12;
         public const int HOTKEY_SET_SET3 = 13;
         public const int HOTKEY_SET_ENABLE_ALL = 14;
         public const int HOTKEY_WINDOW_CONFIG = 15;
         public const int HOTKEY_STANDARDLAYOUT = 16;
         public const int HOTKEY_AUTOLAYOUT = 17;
         public const int HOTKEY_ALT_HIDE = 18;
         public const int HOTKEY_ALIGNMENT_GAUGE = 19;
         private const int HOTKEY_COUNT = 20;

         private Hotkey[] hotkeys;

         public bool enabled { get; set; }

         // temporary ignore hotkeys
         public bool ignoring { get; set; }

         public HotkeyManager()
         {
            hotkeys = new Hotkey[Count()];
            enabled = true;
            SetDefaultHotkeys();
         }

         public int Count()
         {
            return (int)HOTKEY_COUNT;
         }

         public void SetDefaultHotkeys()
         {
            hotkeys[HOTKEY_MAIN] = new Hotkey(KeyCode.RightControl);
            hotkeys[HOTKEY_CLOSE_AND_DRAG] = new Hotkey(KeyCode.RightControl);
            hotkeys[HOTKEY_HIDE] = new Hotkey(KeyCode.KeypadEnter);
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
            hotkeys[HOTKEY_STANDARDLAYOUT] = new Hotkey(KeyCode.Backspace);
            hotkeys[HOTKEY_AUTOLAYOUT] = new Hotkey(KeyCode.KeypadMultiply);
            hotkeys[HOTKEY_ALT_HIDE] = new Hotkey(KeyCode.KeypadDivide);
            hotkeys[HOTKEY_ALIGNMENT_GAUGE] = new Hotkey(KeyCode.Tab);
         }

         public bool GetKey(int id)
         {
            if (id >= Count() || id < 0) return false;
            if (!enabled || ignoring) return false;
            Hotkey hotkey = hotkeys[id];
            //
            if (hotkey.enabled)
            {
               return Input.GetKey(hotkey.code);
            }
            return false;
         }

         public bool GetKeyDown(int id)
         {
            if (id >= Count() || id <0) return false;
            if (!enabled || ignoring) return false;
            Hotkey hotkey = hotkeys[id];
            //
            if (hotkey.enabled)
            {
               return Input.GetKeyDown(hotkey.code);
            }
            return false;
         }

         public void SetKeyCode(int id, KeyCode code)
         {
            if (id >= Count() || id < 0) return;
            Hotkey hotkey = hotkeys[id];
            if (code == hotkey.code) return;
            Log.Info("set keycode for hotkey " + id + " to " + (int)code);
            hotkey.code = code;
         }

         public void SetEnabled(int id, bool enabled)
         {
            if (id >= Count() || id < 0) return;
            Hotkey hotkey = hotkeys[id];
            if (enabled == hotkey.enabled) return;
            Log.Info("set hotkey " + id + " to " + (enabled ? "enabled" : "disabled"));
            hotkey.enabled = enabled;
         }

         public bool IsEnabled(int id)
         {
            if (id >= Count() || id < 0) return false;
            Hotkey hotkey = hotkeys[id];
            return hotkey.enabled;
         }

         public KeyCode GetKeyCode(int id)
         {
            if (id >= Count() || id < 0) return 0;
            Hotkey hotkey = hotkeys[id];
            return hotkey.code;
         }

         public Reference<bool> GetInput(int id)
         {
            Hotkey hotkey = hotkeys[id];
            return hotkey.input;
         }

         private void WriteHotkey(BinaryWriter writer, Hotkey hotkey)
         {
            writer.Write((UInt16)hotkey.code);
            writer.Write(hotkey.enabled);
            Log.Detail("hotkey written: " + hotkey);
         }

         private void ReadHotkey(BinaryReader reader, Hotkey hotkey)
         {
            hotkey.code = (KeyCode)reader.ReadInt16();
            hotkey.enabled = reader.ReadBoolean();
            Log.Detail("hotkey read: " + hotkey);
         }

         public void Read(BinaryReader reader)
         {
            Log.Info("reading hotkeys");
            //
            try
            {
               String marker = reader.ReadString();
               if (marker != CONFIGFILE_MARK)
               {
                  throw new IOException("config file structure corrupt: missing hotkey marker");
               }
               Log.Info("hotkey marker successfully read from config");
               //
               this.enabled = reader.ReadBoolean();
               //
               foreach (Hotkey hotkey in hotkeys)
               {
                  ReadHotkey(reader, hotkey);
               }
            }
            catch(IOException e)
            {
               Log.Warning("error reading hotkeys from config");
               throw e;
            }
         }

         public void Write(BinaryWriter writer)
         {
            Log.Info("writing hotkeys");
            //
            // marker
            writer.Write(CONFIGFILE_MARK);
            //
            writer.Write(enabled);
            //
            foreach (Hotkey hotkey in hotkeys)
            {
               WriteHotkey(writer, hotkey);
            }
         }

         public static bool ValidKeyCode(KeyCode keycode)
         {
            switch (keycode)
            {
               case KeyCode.Mouse0:
               case KeyCode.Mouse1:
               case KeyCode.Mouse2:
               case KeyCode.Mouse3:
               case KeyCode.Mouse4:
               case KeyCode.Mouse5:
               case KeyCode.Mouse6:
               case KeyCode.Escape: return false;
               default:
                  return true;
            }
         }
      }
   }
}
