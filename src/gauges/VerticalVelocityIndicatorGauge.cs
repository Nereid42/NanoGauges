using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {

      public class VerticalVelocityIndicatorGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/VVI-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/VVI-scale");
         private const double MIN_SPEED = 10;
         private const double MAX_VVI = 90;
         private const double MIN_VVI = -90;


         public VerticalVelocityIndicatorGauge()
            : base(Constants.WINDOW_ID_GAUGE_VVI, SKIN, SCALE)
         {
         }

         public override string GetName()
         {
            return "VVI";
         }

         public override string GetDescription()
         {
            return "Vertical velocity Indicator. Vertical direction of the vessels movement in relation to the horizon.";
         }


         protected override void AutomaticOnOff()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null)
            {
               if (vessel.srfSpeed >= MIN_SPEED && !vessel.isEVA)
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
               double vvi = 90.0 - Vector3.Angle(vessel.srf_velocity, vessel.upAxis);

               if (vvi > MAX_VVI) vvi = MAX_VVI;
               if (vvi < MIN_VVI) vvi = MIN_VVI;

               if (vvi >= 0)
               {
                  y = m + 61.655f * (float)Math.Log10(1 + 3 * vvi) / 400.0f;
               }
               else
               {
                  y = m - 61.655f * (float)Math.Log10(1 - 3 * vvi) / 400.0f;
               }
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:VVI";
         }
      }
   }
}
