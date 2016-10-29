using System;
using UnityEngine;
using NanoGaugesAdapter;

namespace Nereid
{
   namespace NanoGauges
   {
      class HotkeysWindow : AbstractWindow
      {
         public const int WIDTH = 630;
         public const int HEIGHT = 495;

         private static readonly GUIStyle STYLE_HOTKEY_BUTTON = new GUIStyle(HighLogic.Skin.button);
         private static readonly GUIStyle STYLE_NAME = new GUIStyle(HighLogic.Skin.label);
         private static readonly GUIStyle STYLE_LABEL = new GUIStyle(HighLogic.Skin.label);
         private static readonly GUIStyle STYLE_CHORD = new GUIStyle(HighLogic.Skin.label);
         private static readonly GUIStyle STYLE_SEPARATOR = new GUIStyle(HighLogic.Skin.label);

         static HotkeysWindow()
         {
            STYLE_HOTKEY_BUTTON.fixedWidth = 80;
            STYLE_HOTKEY_BUTTON.clipping = TextClipping.Clip;
            STYLE_NAME.fixedWidth = 85;
            STYLE_NAME.stretchWidth = false;
            STYLE_NAME.normal.textColor = Color.white;
            STYLE_CHORD.fixedWidth = 75;
            STYLE_CHORD.stretchWidth = false;
            STYLE_CHORD.clipping = TextClipping.Clip;
            STYLE_CHORD.normal.textColor = Color.green;
            STYLE_LABEL.normal.textColor = Color.white;
            STYLE_LABEL.stretchWidth = false;
            STYLE_LABEL.fixedWidth = 9;
            STYLE_LABEL.alignment = TextAnchor.MiddleCenter;
            STYLE_SEPARATOR.stretchWidth = true;
            STYLE_SEPARATOR.fixedHeight = 14;
         }

         public HotkeysWindow()
            : base(Constants.WINDOW_ID_HOTKEYS, "Hotkeys")
         {
            SetSize(WIDTH, HEIGHT);
         }

         protected override void OnWindow(int id)
         {
            HotkeyManager manager = NanoGauges.hotkeyManager;
            GUILayout.BeginVertical();
            //
            manager.enabled = GUILayout.Toggle(manager.enabled, "Hotkeys enabled");
            //
            GUILayout.Label("", STYLE_SEPARATOR);
            //
            GUILayout.BeginHorizontal();
            manager.SetEnabled(HotkeyManager.HOTKEY_MAIN, GUILayout.Toggle(manager.IsEnabled(HotkeyManager.HOTKEY_MAIN), ""));
            KeyCodeButton(KeyCode.LeftControl, "LEFT CTRL");
            KeyCodeButton(KeyCode.RightControl, "RIGHT CTRL");
            KeyCodeButton(KeyCode.LeftShift, "LEFT SHIFT");
            KeyCodeButton(KeyCode.RightShift, "RIGHT SHIFT");
            KeyCodeButton(KeyCode.LeftAlt, "LEFT ALT");
            KeyCodeButton(KeyCode.RightAlt, "RIGHT ALT");
            GUILayout.EndHorizontal();

            //
            GUILayout.Label("", STYLE_SEPARATOR);
            //

            //
            KeyCode main_hotkey = manager.GetKeyCode(HotkeyManager.HOTKEY_MAIN);

            GUILayout.BeginHorizontal();
            DrawHotkey("Standard", HotkeyManager.HOTKEY_SET_STANDARD, main_hotkey);
            GUILayout.FlexibleSpace();
            DrawHotkey("Next set", HotkeyManager.HOTKEY_NEXTSET);
            GUILayout.EndHorizontal();
            //
            GUILayout.BeginHorizontal();
            DrawHotkey("Launch", HotkeyManager.HOTKEY_SET_LAUNCH, main_hotkey);
            GUILayout.FlexibleSpace();
            DrawHotkey("Prev set", HotkeyManager.HOTKEY_PREVSET);
            GUILayout.EndHorizontal();
            //
            GUILayout.BeginHorizontal();
            DrawHotkey("Land", HotkeyManager.HOTKEY_SET_LAND, main_hotkey);
            GUILayout.FlexibleSpace();
            DrawHotkey("Hide gauges", HotkeyManager.HOTKEY_HIDE);
            GUILayout.EndHorizontal();
            //
            GUILayout.BeginHorizontal();
            DrawHotkey("Docking", HotkeyManager.HOTKEY_SET_DOCK, main_hotkey);
            GUILayout.FlexibleSpace();
            DrawHotkey("Hide gauges", HotkeyManager.HOTKEY_ALT_HIDE, main_hotkey);
            GUILayout.EndHorizontal();
            //
            GUILayout.BeginHorizontal();
            DrawHotkey("Orbit", HotkeyManager.HOTKEY_SET_ORBIT, main_hotkey);
            GUILayout.FlexibleSpace();
            DrawHotkey("Reset layout", HotkeyManager.HOTKEY_STANDARDLAYOUT, main_hotkey);
            GUILayout.EndHorizontal();
            //
            GUILayout.BeginHorizontal();
            DrawHotkey("Flight", HotkeyManager.HOTKEY_SET_FLIGHT, main_hotkey);
            GUILayout.FlexibleSpace();
            DrawHotkey("Auto layout", HotkeyManager.HOTKEY_AUTOLAYOUT, main_hotkey);
            GUILayout.EndHorizontal();
            //
            GUILayout.BeginHorizontal();
            DrawHotkey("Set 1", HotkeyManager.HOTKEY_SET_SET1, main_hotkey);
            GUILayout.FlexibleSpace();
            DrawHotkey("Enable all", HotkeyManager.HOTKEY_SET_ENABLE_ALL, main_hotkey);
            GUILayout.EndHorizontal();
            //
            GUILayout.BeginHorizontal();
            DrawHotkey("Set 2", HotkeyManager.HOTKEY_SET_SET2, main_hotkey);
            GUILayout.FlexibleSpace();
            DrawHotkey("Config window", HotkeyManager.HOTKEY_WINDOW_CONFIG, main_hotkey);
            GUILayout.EndHorizontal();
            //
            GUILayout.BeginHorizontal();
            DrawHotkey("Set 3", HotkeyManager.HOTKEY_SET_SET3, main_hotkey);
            GUILayout.FlexibleSpace();
            DrawHotkey("Lock Profile", HotkeyManager.HOTKEY_LOCK_PROFILE);
            GUILayout.EndHorizontal();
            //
            GUILayout.BeginHorizontal();
            DrawHotkey("Next Navpoint", HotkeyManager.HOTKEY_SELECT_NAV, main_hotkey);
            GUILayout.FlexibleSpace();
            DrawHotkey("Drag&Close", HotkeyManager.HOTKEY_CLOSE_AND_DRAG);
            GUILayout.EndHorizontal();
            //
            GUILayout.BeginHorizontal();
            DrawHotkey("Align. gauge", HotkeyManager.HOTKEY_ALIGNMENT_GAUGE, main_hotkey);
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset to defaults", HighLogic.Skin.button))
            {
               manager.SetDefaultHotkeys();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Save", HighLogic.Skin.button))
            {
               NanoGauges.configuration.Save();
            }
            if (GUILayout.Button("Close", HighLogic.Skin.button))
            {
               SetVisible(false);
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            //
            DragWindow();
         }


         public override int GetInitialWidth()
         {
            return WIDTH;
         }

         protected override int GetInitialHeight()
         {
            return HEIGHT;
         }

         private void DrawHotkey(String name, int id)
         {
            DrawHotkey(name, id, KeyCode.None);
         }

         private void DrawHotkey(String name, int id, KeyCode main_hotkey)
         {
            HotkeyManager manager = NanoGauges.hotkeyManager;

            Reference<bool> input = manager.GetInput(id);

            KeyCode keycode = manager.GetKeyCode(id);
            bool enabled = manager.IsEnabled(id);

            GUILayout.BeginHorizontal();

            enabled = GUILayout.Toggle(enabled," ");
            manager.SetEnabled(id, enabled);

            GUILayout.Label(name, STYLE_NAME);

            if(main_hotkey != KeyCode.None)
            {
               GUILayout.Label(main_hotkey.ToString().Limit(12), STYLE_CHORD);
               GUILayout.Label("+", STYLE_LABEL);
            }
            else 
            {
               GUILayout.Label("", STYLE_CHORD);
               GUILayout.Label("", STYLE_LABEL);
            }

            String text = input.value ? "press" : keycode.ToString().Limit(10);
            Utils.SetTextColor(STYLE_HOTKEY_BUTTON, input.value ? Constants.ORANGE : Color.white);
            if (GUILayout.Button(text, STYLE_HOTKEY_BUTTON))
            {
               if (input.value)
               {
                  keycode = KeyCode.None;
                  input.value = false;
               }
               else
               {
                  input.value = true;
               }               
            }
            if (input.value)
            {
               if (Input.anyKeyDown)
               {
                  // do not recognize this input as a hotkey
                  manager.ignoring = true;

                  input.value = false;
                  KeyCode[] keys = Utils.GetPressedKeys();
                  if (keys != null && keys.Length > 0)
                  {
                     keycode = keys[0];
                     if(HotkeyManager.ValidKeyCode(keys[0]))
                     {
                        manager.SetKeyCode(id, keycode);
                     }
                  }
               }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
         }

         private void KeyCodeButton(KeyCode code, String text)
         {
            KeyCode hotkey = NanoGauges.hotkeyManager.GetKeyCode(HotkeyManager.HOTKEY_MAIN);
            if (GUILayout.Toggle(hotkey == code, text, HighLogic.Skin.button))
            {
               if (hotkey != code)
               {
                  NanoGauges.hotkeyManager.SetKeyCode(HotkeyManager.HOTKEY_MAIN, code);
               }
            }
         }

      }

   }
}
