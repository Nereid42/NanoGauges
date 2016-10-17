using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nereid
{
   namespace NanoGauges
   {
      public class AlignmentGauge : AbstractClosableGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/ALIGN");

         private Rect skinBounds;

         public AlignmentGauge()
            : base(Constants.WINDOW_ID_GAUGE_ALIGNMENT)
         {
            SetPosition(0,0);
         }

         protected override void OnWindow(int id)
         {
            // skin
            GUI.DrawTexture(skinBounds, SKIN);
         }

         public override string GetName()
         {
            return "Aligment";
         }

         public override string GetDescription()
         {
            return "Helps to align all gauges";
         }

         public override sealed int GetWidth()
         {
            return NanoGauges.configuration.verticalGaugeWidth;
         }

         public override sealed int GetHeight()
         {
            return NanoGauges.configuration.verticalGaugeHeight;
         }

         public override void OnGaugeScalingChanged()
         {
            // change dimensions of window
            bounds.width = NanoGauges.configuration.verticalGaugeWidth;
            bounds.height = NanoGauges.configuration.verticalGaugeHeight;
            //
            //change dimensions of skin and scale
            skinBounds.width = NanoGauges.configuration.verticalGaugeWidth;
            skinBounds.height = NanoGauges.configuration.verticalGaugeHeight;
         }

         public override void On()
         {
            // do nothing
         }

         public override void Off()
         {
            // do nothing
         }

         public override bool IsOn()
         {
            return true;
         }

         public override void InLimits()
         {
            // do nothing
         }

         public override void OutOfLimits()
         {
            // do nothing
         }

         public override bool IsInLimits()
         {
            return true;
         }

         public override void Reset()
         {
            // do nothing
         }
      }

   }
}
