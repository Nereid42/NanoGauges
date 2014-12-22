using System;
using UnityEngine;
using NanoGaugesAdapter;

namespace Nereid
{
   namespace NanoGauges
   {
      class AboutWindow : AbstractWindow
      {
         private IButton toolbarButton;
         private String toolbarButtonTextureOn;
         private String toolbarButtonTextureOff;

         public AboutWindow()
            : base(Constants.WINDOW_ID_ABOUT, "About")
         {
            SetSize(350, 300);
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

         protected override void OnOpen()
         {
            base.OnOpen();
            if (toolbarButton != null)
            {
               toolbarButton.TexturePath = toolbarButtonTextureOn;
            }
         }

         protected override void OnClose()
         {
            base.OnClose();
            if (toolbarButton != null)
            {
               toolbarButton.TexturePath = toolbarButtonTextureOff;
            }
         }

         public override int GetInitialWidth()
         {
            return 350;
         }

         protected override int GetInitialHeight()
         {
            return 300;
         }

         public void registerToolbarButton(IButton button, String textureOn, String textureOff)
         {
            this.toolbarButton = button;
            this.toolbarButtonTextureOn = textureOn;
            this.toolbarButtonTextureOff = textureOff;
         }
      }

   }
}
