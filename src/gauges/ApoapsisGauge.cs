using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {

      public class ApoapsisGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/APA-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/APA-scale");
         private const double MIN_APA = 5000;
         private const double MAX_APA = 1000000;

         public ApoapsisGauge()
            : base(Constants.WINDOW_ID_GAUGE_APA, SKIN, SCALE)
         {
         }

         public override string GetName()
         {
            return "Apoapsis";
         }

         public override string GetDescription()
         {
            return "Apoapsis of the current flight path.";
         }

         protected override void AutomaticOnOff()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && FlightGlobals.ActiveVessel.parts.Count > 0)
            {
               if (vessel.orbit.ApA >= MIN_APA)
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
               double apa = vessel.orbit.ApA;
               if (apa > MAX_APA) apa = MAX_APA;
               if (apa < MIN_APA) apa = MIN_APA;
               y = b + 130.5f * (float)Math.Log10(apa/5000) / 400.0f;
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:APA";
         }
      }
   }
}
