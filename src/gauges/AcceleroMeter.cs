using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class AcceleroMeter : VerticalGauge
      {
         private static Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/G-skin");
         private static Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/G-scale");
         private const double MAX_G = 10;


         public AcceleroMeter()
            : base(Constants.WINDOW_ID_GAUGE_G, SKIN, SCALE)
         {
         }

         public override string GetName()
         {
            return "Gee force";
         }

         public override string GetDescription()
         {
            return "Current gee force.";
         }


         protected override float GetScaleOffset()
         {
            float b = GetLowerOffset();
            float y = b;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null)
            {
               double g = vessel.geeForce;
               if(!double.IsNaN(g))
               {
                  if (g > MAX_G) g = MAX_G;
                  if (g < 0) g = 0;
                  y = b + 30.0f * (float)g / 400.0f;
               }
            }
            return y;
         }

         public override string ToString()
         {
            return "Gauge:G";
         }
      }
   }
}
