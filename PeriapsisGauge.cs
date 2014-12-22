using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {

      public class PeriapsisGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/PEA-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/PEA-scale");
         private const double MIN_PEA = 5000;
         private const double MAX_PEA = 1000000;

         public PeriapsisGauge()
            : base(Constants.WINDOW_ID_GAUGE_PEA, SKIN, SCALE)
         {
         }

         public override string GetName()
         {
            return "Periapsis";
         }

         public override string GetDescription()
         {
            return "Periapsis of the current flight path.";
         }


         protected override void AutomaticOnOff()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && FlightGlobals.ActiveVessel.parts.Count > 0)
            {
               if (vessel.orbit.PeA >= MIN_PEA)
               {
                  On();
                  return;
               }
            }
            Off();
         }

         protected override float GetScaleOffset()
         {
            float b = GetLowerOffset();
            float y = GetUpperOffset();
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null)
            {
               double pea = vessel.orbit.PeA;
               if (pea > MAX_PEA) pea = MAX_PEA;
               if (pea < MIN_PEA) pea = MIN_PEA;
               y = b + 130.5f * (float)Math.Log10(pea/5000) / 400.0f;
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:PEA";
         }
      }
   }
}
