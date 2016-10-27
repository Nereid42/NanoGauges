using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {

      public class IndicatedAirspeedGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/IAS-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/IAS-scale");
         private const double MIN_SPEED = 20;
         private const double MAX_SPEED = 800;

         public IndicatedAirspeedGauge()
            : base(Constants.WINDOW_ID_GAUGE_IAS, SKIN, SCALE, true, 0.022f)
         {
         }

         public override string GetName()
         {
            return "IAS";
         }

         public override string GetDescription()
         {
            return "Indicated airspeed. Shows the indicated airspeed measured by the pitot tubes of the vessel.";
         }

         protected override float GetScaleOffset()
         {
            float b = GetLowerOffset();
            float y = b;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null)
            {
               On();
               double v = vessel.indicatedAirSpeed;
               if (v > MAX_SPEED)
               {
                  v = MAX_SPEED;
                  NotInLimits();
               }
               else if (v < MIN_SPEED && v > 0)
               {
                  v = 0;
                  NotInLimits();
               }
               else
               {
                  InLimits();
               }
               if (v >= 0)
               {
                  y = b + 300.0f * (float)(v/600.0) / (float)SCALE_HEIGHT;
               }
            }
            else
            {
               Off();
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:IAS";
         }
      }
   }
}
