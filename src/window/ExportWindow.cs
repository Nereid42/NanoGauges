using System;
using UnityEngine;
using NanoGaugesAdapter;

namespace Nereid
{
   namespace NanoGauges
   {
      class ExportWindow : AbstractWindow
      {
         public const int WIDTH = 290;
         public const int HEIGHT = 463;

         private static readonly GUIStyle STYLE_LABEL = new GUIStyle(HighLogic.Skin.label);
         private static readonly GUIStyle STYLE_TOGGLE_2_PER_ROW = new GUIStyle(HighLogic.Skin.toggle);

         private bool includeStatus = true;
         private bool includePosition = true;

         private readonly Exporter exporter = new Exporter();

         static ExportWindow()
         {
            STYLE_TOGGLE_2_PER_ROW.margin = new RectOffset(0, 150, 0, 0);
            STYLE_LABEL.stretchWidth = true;
         }


         public ExportWindow()
            : base(Constants.WINDOW_ID_EXPORT, "Export")
         {
            SetSize(WIDTH, HEIGHT);
         }

         protected override void OnWindow(int id)
         {
            GUILayout.BeginVertical();
            GUILayout.Label("Generic settings:", STYLE_LABEL);
            GUILayout.Label("Gauge properties:", STYLE_LABEL);
            GUILayout.BeginHorizontal();
            includePosition = GUILayout.Toggle(includePosition, "Position", STYLE_TOGGLE_2_PER_ROW);
            includeStatus   = GUILayout.Toggle(includeStatus,   "Status", STYLE_TOGGLE_2_PER_ROW);
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Button("Import", HighLogic.Skin.button);
            if (GUILayout.Button("Export", HighLogic.Skin.button))
            {
               exporter.Export();
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
