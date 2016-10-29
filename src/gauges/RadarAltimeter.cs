using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {

      public class RadarAltimeter : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/RALT-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/RALT-scale");
         private const double MIN_ALTITUDE = 0.0001;
         private const double MAX_ALTITUDE = 10000;

         public RadarAltimeter()
            : base(Constants.WINDOW_ID_GAUGE_RADAR_ALTIMETER, SKIN, SCALE)
         {
         }

         public override string GetName()
         {
            return "Radar Altimeter";
         }

         public override string GetDescription()
         {
            return "Real altitude above terrain. Please not that a water surface is not detected by this gauge.";
         }

         protected override void AutomaticOnOff()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && FlightGlobals.ActiveVessel.parts.Count > 0 && !vessel.isEVA)
            {
               double alt = vessel.altitude - vessel.terrainAltitude;
               //
               if (alt > MIN_ALTITUDE)
               {
                  if (alt < MAX_ALTITUDE)
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
               double alt = vessel.RadarAltitude(); 
               if(alt>0)
               {
                  if (alt > MAX_ALTITUDE) alt = MAX_ALTITUDE;
                  y = b + 2*37.5f * (float)Math.Log10(1 + alt) / 400.0f;
               }
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:RALT";
         }
      }
   }
}
