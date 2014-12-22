using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {

      public class AngleOfAttackGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/AOA-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/AOA-scale");
         private const double MIN_SPEED = 10;


         public AngleOfAttackGauge()
            : base(Constants.WINDOW_ID_GAUGE_AOA, SKIN, SCALE)
         {
         }

         public override string GetName()
         {
            return "Angle of Attack";
         }

         public override string GetDescription()
         {
            return "The angle of attack of the vessel. The AOA is the angle between forward velocity and the pointing nose of the vessel.";
         }

         protected override void AutomaticOnOff()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null)
            {
               if(vessel.srfSpeed>=MIN_SPEED && !vessel.isEVA)
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
            float b = GetLowerOffset();
            float y = b;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && IsOn())
            {
               // credits: Mechjeb
               Vector3 forward = vessel.GetTransform().up;
               double aoa = Vector3.Angle(forward, vessel.srf_velocity);
               y = b + 133.85f * (float)Math.Log10(1 + aoa) / 400.0f;
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:AOA";
         }
      }
   }
}
