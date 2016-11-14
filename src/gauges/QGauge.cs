using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {

      public class QGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/Q-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/Q-scale");
         private const double MIN_Q = 0;
         private const double MAX_Q = 100000;

         public QGauge()
            : base(Constants.WINDOW_ID_GAUGE_Q, SKIN, SCALE)
         {
         }

         protected override void AutomaticOnOff()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && FlightGlobals.ActiveVessel.parts.Count > 0)
            {
                On();
                return;
            }
            Off();
         }

         public override string GetName()
         {
            return "Q - Dynamic\nPressure";
         }

         public override string GetDescription()
         {
            return "\n\nDynamic Pressure.";
         }

         protected override float GetScaleOffset()
         {
            float b = GetLowerOffset();
            float y = b;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && !vessel.isEVA)
            {
               On();
               double q = vessel.dynamicPressurekPa * 1000;
               if (q > MAX_Q) q = MAX_Q;
               if (q < MIN_Q) q = MIN_Q;
               y = b + 60.0f * (float)Math.Log10(1+q) / 400.0f;
            }
            else
            {
               Off();
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
