using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {

      public class Altimeter : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/ALT-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/ALT-scale");
         private const double MAX_ALTITUDE = 500000;
         private const double MIN_ALTITUDE = 0;

         public Altimeter()
            : base(Constants.WINDOW_ID_GAUGE_ALTIMETER, SKIN, SCALE)
         {
         }

         public override string GetName()
         {
            return "Altimeter";
         }

         public override string GetDescription()
         {
            return "Altimeter.";
         }

         protected override void AutomaticOnOff()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && FlightGlobals.ActiveVessel.parts.Count > 0)
            {
               On();
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
            if (vessel != null)
            {
               double alt = vessel.altitude;
               if(alt>0)
               {
                  if (alt > MAX_ALTITUDE)
                  {
                     alt = MAX_ALTITUDE;
                     OutOfLimits();
                  }
                  else if (alt < MIN_ALTITUDE)
                  {
                     alt = MIN_ALTITUDE;
                     OutOfLimits();
                  }
                  else
                  {
                     InLimits();
                  }
                  //
                  if (alt<10000)
                  {
                     y = b + 100f * ((float)alt / 10000f) / 400f;
                  }
                  else if (alt < 30000)
                  {
                     y = b + (100f + 100f * ((float)(alt-10000) / 20000f)) / 400f;
                  }
                  else
                  {
                     y = b + (200f + 100f * ((float)(alt-30000) / 70000f)) / 400f;
                  }
               }
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:ALT";
         }
      }
   }
}
