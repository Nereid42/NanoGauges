using System;
using UnityEngine;

namespace Nereid
{
   namespace NanoGauges
   {

      public class VsiGauge : VerticalGauge
      {
         private static Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/VSI-skin");
         private static Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/VSI-scale");
         private static double MAX_SPEED = 10000;
         private static double MIN_SPEED = -10000;

         public VsiGauge()
            : base(Constants.WINDOW_ID_GAUGE_VSI, SKIN, SCALE)
         {
         }

         public override string GetName()
         {
            return "Vertical Speed";
         }

         public override string GetDescription()
         {
            return "Vertical Speed Indicator. Measures rate of climb or descent.";
         }

         protected override float GetScaleOffset()
         {
            // center 0 vsi
            float m = GetCenterOffset();
            float y = m;

            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null)
            {
               double v = vessel.verticalSpeed;
               if (v > MAX_SPEED)
               {
                  v = MAX_SPEED;
                  NotInLimits();
               }
               else if (v < MIN_SPEED)
               {
                  v = MIN_SPEED;
                  NotInLimits();
               }
               else
               {
                  InLimits();
               }

               if (v >= 0)
               {
                  y = m + 37.5f * (float)Math.Log10(1 + v) / 400.0f;
               }
               else
               {
                  y = m - 37.5f * (float)Math.Log10(1 - v) / 400.0f;
               }
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:VSI";
         }
      }
   }
}
