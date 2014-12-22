using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {

      public class OrbitalVelocityGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/OSPD-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/OSPD-scale");
         private const double MAX_SPEED = 10000;

         public OrbitalVelocityGauge()
            : base(Constants.WINDOW_ID_GAUGE_OSPD, SKIN, SCALE)
         {
         }

         public override string GetName()
         {
            return "Orbital Speed";
         }

         public override string GetDescription()
         {
            return "Current orbital speed of the vessel.";
         }

         protected override float GetScaleOffset()
         {
            float b = GetLowerOffset();
            float y = b;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null)
            {
               double v = vessel.obt_speed;
               if (v > MAX_SPEED) v = MAX_SPEED;
               if (v >= 0)
               {
                  y = b + 75.0f * (float)Math.Log10(1 + v) / 400.0f;
               }
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:OSPD";
         }
      }
   }
}
