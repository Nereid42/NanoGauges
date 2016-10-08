using System;
using UnityEngine;
using NanoGaugesAdapter;

namespace Nereid
{
   namespace NanoGauges
   {
      class HotkeysWindow : AbstractWindow
      {
         public const int WIDTH = 560;
         public const int HEIGHT = 432;

         private static readonly GUIStyle STYLE_PROFILE_BUTTON = new GUIStyle(HighLogic.Skin.button);
         private static readonly GUIStyle STYLE_HOTKEY_BUTTON = new GUIStyle(HighLogic.Skin.button);


         static HotkeysWindow()
         {
            STYLE_PROFILE_BUTTON.fixedWidth = 160;
            STYLE_HOTKEY_BUTTON.fixedWidth = 50;
         }


         public HotkeysWindow()
            : base(Constants.WINDOW_ID_HOTKEYS, "Hotkeys")
         {
            SetSize(WIDTH, HEIGHT);
         }

         protected override void OnWindow(int id)
         {
            ProfileManager manager = NanoGauges.profileManager;
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset to defaults", HighLogic.Skin.button))
            {
               //               TODO
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
      }

   }
}
