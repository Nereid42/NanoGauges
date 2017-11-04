using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {
      class TooltipWindow : AbstractWindow
      {
         private const int TOOLTIP_WIDTH = 125;

         private static readonly GUIStyle STYLE_TEXT = new GUIStyle(HighLogic.Skin.label);

         private String title;
         private string text;

         static TooltipWindow()
         {
            STYLE_TEXT.fontSize = 10;
            STYLE_TEXT.normal.textColor = Color.white;
            STYLE_TEXT.alignment = TextAnchor.MiddleLeft;
            STYLE_TEXT.stretchWidth = true;
            STYLE_TEXT.stretchHeight = true;
            STYLE_TEXT.wordWrap=true;
         }

         private readonly AbstractGauge gauge;

         public TooltipWindow(AbstractGauge gauge)
            : base(Constants.WINDOW_ID_TOOLTIP, gauge.GetName())
         {
            this.gauge = gauge;
            SetSize(TOOLTIP_WIDTH, gauge.GetHeight());
            this.title = gauge.GetName();
            this.text = gauge.GetDescription();
         }

         private void AlignToGauge()
         {
            if (gauge != null)
            {
               int x = gauge.GetX();
               int y = gauge.GetY();
               if (x + TOOLTIP_WIDTH + 10 < Screen.width)
               {
                  SetPosition(x + gauge.GetWidth(), y);
               }
               else
               {
                  SetPosition(x - TOOLTIP_WIDTH, y);
               }
            }
         }

         protected override void OnWindow(int id)
         {
            if (IsVisible())
            {
               GUILayout.BeginVertical();
               GUILayout.Label(text, STYLE_TEXT);
               GUILayout.FlexibleSpace();
               GUILayout.EndVertical();
               //
               AlignToGauge();
            }
         }

         public override void SetVisible(bool visible)
         {
            AlignToGauge();
            base.SetVisible(visible);
         }

         public override int GetInitialWidth()
         {
            return TOOLTIP_WIDTH;
         }

         public void UpdateText()
         {
            this.title = gauge.GetName();
            this.text = gauge.GetDescription();
         }
      }
   }
}
