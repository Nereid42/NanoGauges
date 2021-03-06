﻿using System;
using UnityEngine;
using NanoGaugesAdapter;

namespace Nereid
{
   namespace NanoGauges
   {
      class ProfilesWindow : AbstractWindow
      {
         public const int WIDTH = 350;
         public const int HEIGHT = 525;

         private static readonly GUIStyle STYLE_PROFILE_BUTTON = new GUIStyle(HighLogic.Skin.button);
         private static readonly GUIStyle STYLE_HOTKEY_BUTTON = new GUIStyle(HighLogic.Skin.button);

         private bool hotkeyInput1 = false;
         private bool hotkeyInput2 = false;
         private bool hotkeyInput3 = false;


         static ProfilesWindow()
         {
            STYLE_PROFILE_BUTTON.fixedWidth = 160;
            STYLE_HOTKEY_BUTTON.fixedWidth = 80;
            STYLE_HOTKEY_BUTTON.clipping = TextClipping.Clip;
            STYLE_HOTKEY_BUTTON.normal.textColor = Color.white;
         }


         public ProfilesWindow()
            : base(Constants.WINDOW_ID_PROFILES, "Profiles")
         {
            SetSize(WIDTH, HEIGHT);
         }

         protected override void OnWindow(int id)
         {
            ProfileManager manager = NanoGauges.profileManager;
            GUILayout.BeginVertical();
            NanoGauges.profileManager.enabled = GUILayout.Toggle(manager.enabled, "Profiles enabled", HighLogic.Skin.toggle);
            DrawProfile(manager.LAUNCH);
            DrawProfile(manager.FLIGHT);
            DrawProfile(manager.ORBIT);
            DrawProfile(manager.ESCAPE);
            DrawProfile(manager.ORBIT_TO_SUBORBITAL);
            DrawProfile(manager.FLIGHT_TO_SUBORBITAL);
            DrawProfile(manager.OTHER_TO_SUBORBITAL);
            DrawProfile(manager.LANDED);
            DrawProfile(manager.DOCKED);
            DrawProfile(manager.DOCKING);
            DrawProfile(manager.EVA);
            manager.Hotkey1 = DrawHotKeyProfile(manager.Hotkey1, manager.HOTKEY1, ref hotkeyInput1);
            manager.Hotkey2 = DrawHotKeyProfile(manager.Hotkey2, manager.HOTKEY2, ref hotkeyInput2);
            manager.Hotkey3 = DrawHotKeyProfile(manager.Hotkey3, manager.HOTKEY3, ref hotkeyInput3);
            //
            GUILayout.FlexibleSpace();
            //
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset to defaults", HighLogic.Skin.button))
            {
               manager.ResetDefaults();
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

         private void ChangeProfile(Profile profile)
         {
            ProfileBehaviour behaviour = profile.GetBehaviour();
            if (Event.current.button == 0)
            {
               // left mouse button
               Log.Detail("choosing next behaviour for profile "+profile.name);
               profile.SetBehaviour(behaviour.next);
            }
            else
            {
               // not left mouse button
               Log.Detail("choosing previous behaviour for profile " + profile.name);
               profile.SetBehaviour(behaviour.prev);
            }
         }

         private void DrawProfile(Profile profile)
         {
            GUILayout.BeginHorizontal();
            profile.enabled = GUILayout.Toggle(profile.enabled,"",HighLogic.Skin.toggle);
            GUILayout.Label(profile.name, GUI.skin.label);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(profile.GetBehaviour().GetName(), STYLE_PROFILE_BUTTON))
            {
               ChangeProfile(profile);
            }
            GUILayout.EndHorizontal();
         }


         private KeyCode DrawHotKeyButton(KeyCode keycode, ref bool input)
         {
            String text = input ? "press" : keycode.ToString().Limit(10);
            Utils.SetTextColor(STYLE_HOTKEY_BUTTON, input ? Constants.ORANGE : Color.white);
            if (GUILayout.Button(text, STYLE_HOTKEY_BUTTON))
            {
               if(input)
               {
                  keycode = KeyCode.None;
                  input = false;
               }
               else
               {
                  input = true;
               }               
            }
            if (input)
            {
               if (Input.anyKeyDown)
               {
                  input = false;
                  KeyCode[] keys = Utils.GetPressedKeys();
                  if (keys != null && keys.Length > 0)
                  {
                     if(HotkeyManager.ValidKeyCode(keys[0]))
                     {
                        keycode = keys[0];
                     }
                  }
                  // do not recognize this input as a hotkey
                  NanoGauges.profileManager.IgnoreHotkeyInFrame();
               }
            }

            return keycode;
         }

         private KeyCode DrawHotKeyProfile(KeyCode keycode, Profile profile, ref bool input)
         {
            GUILayout.BeginHorizontal();
            profile.enabled = GUILayout.Toggle(profile.enabled, "", HighLogic.Skin.toggle);
            GUILayout.Label("Hotkey", GUI.skin.label);
            GUILayout.FlexibleSpace();
            //
            keycode = DrawHotKeyButton(keycode, ref input);
            //
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(profile.GetBehaviour().GetName(), STYLE_PROFILE_BUTTON))
            {
               ChangeProfile(profile);
            }
            GUILayout.EndHorizontal();
            return keycode;
         }

         public override int GetInitialWidth()
         {
            return WIDTH;
         }

         protected override int GetInitialHeight()
         {
            return HEIGHT;
         }
      }

   }
}
