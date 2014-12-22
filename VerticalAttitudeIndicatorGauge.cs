using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {

      public class VerticalAttitudeIndicatorGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/VAI-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/VAI-scale");
         private const double MAX_VAI = 90;
         private const double MIN_VAI = -90;


         public VerticalAttitudeIndicatorGauge()
            : base(Constants.WINDOW_ID_GAUGE_VAI, SKIN, SCALE)
         {
         }

         public override string GetName()
         {
            return "VAI";
         }

         public override string GetDescription()
         {
            return "Vertical Attitude Indicator. Vertical alignment for the nose of the vessel in relation to the horizon.";
         }

         protected override void AutomaticOnOff()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null)
            {
               if(!vessel.isEVA)
               {
                  On();
               }
               else
               {
                  Off();
               }
            }
            else
            {
               Off();
            }
         }

         protected override float GetScaleOffset()
         {
            float m = GetCenterOffset();
            float y = m;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && IsOn())
            {
               // credits: Mechjeb
               Vector3 forward = vessel.GetTransform().up;
               double vai = 90.0 - Vector3.Angle(forward, vessel.upAxis);

               if (vai > MAX_VAI) vai = MAX_VAI;
               if (vai < MIN_VAI) vai = MIN_VAI;

               if(vai>=0)
               {
                  y = m + 61.655f * (float)Math.Log10(1 + 3*vai) / 400.0f;
               }
               else
               {
                  y = m - 61.655f * (float)Math.Log10(1 - 3*vai) / 400.0f;
               }
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:VAI";
         }
      }
   }
}
