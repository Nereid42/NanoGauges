using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {

      public class VelocityGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/SPD-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/SPD-scale");
         private const double MAX_SPEED = 10000;

         public VelocityGauge()
            : base(Constants.WINDOW_ID_GAUGE_SPD, SKIN, SCALE)
         {
         }

         public override string GetName()
         {
            return "Speed";
         }

         public override string GetDescription()
         {
            return "Current surface speed of the vessel";
         }

         protected override float GetScaleOffset()
         {
            float b = GetLowerOffset();
            float y = b;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null)
            {
               double v = vessel.srfSpeed;
               if (v > MAX_SPEED) v = MAX_SPEED;
               if (v >= 0)
               {
                  y = b + 2*37.5f * (float)Math.Log10(1 + v) / 400.0f;
               }
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:SPD";
         }
      }
   }
}
