using System;
using UnityEngine;
using NanoGaugesAdapter;

namespace Nereid
{
   namespace NanoGauges
   {
      class AboutWindow : AbstractWindow
      {
         public const int WIDTH = 350;
         public const int HEIGHT = 300;

         public AboutWindow()
            : base(Constants.WINDOW_ID_ABOUT, "About")
         {
            SetSize(WIDTH, HEIGHT);
         }

         protected override void OnWindow(int id)
         {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUILayout.Label("Nano Gauges - written by Nereid (A.Kolster)", HighLogic.Skin.label);
            GUILayout.Label("", HighLogic.Skin.label);
            GUILayout.Label("Original idea by bucky", HighLogic.Skin.label);
            GUILayout.Label("Trim indicators originaly done by dazoe", HighLogic.Skin.label);
            GUILayout.Label("Special thanks to cybutek and dain", HighLogic.Skin.label);

            GUILayout.EndVertical();
            if (GUILayout.Button("Close", HighLogic.Skin.button))
            {
               SetVisible(false);
            }
            GUILayout.EndHorizontal();
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
