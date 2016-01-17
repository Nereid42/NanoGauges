using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class TempGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/TEMP-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/KELVIN-scale");
         private static readonly double MAX_TEMP = 2500;
         private const double MIN_TEMP = 0;

         private readonly SensorInspecteur inspecteur;

         public TempGauge(SensorInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_TEMP, SKIN, SCALE, true, 0.00085f)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Temp";
         }

         public override string GetDescription()
         {
            return "Average of all temperature readings (if any) on the vessel in Kelvin.";
         }

         protected override void AutomaticOnOff()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null  && FlightGlobals.ActiveVessel.parts.Count > 0)
            {
               if(inspecteur.IsTempSensorEnabled())
               {
                  On();
                  return;
               }
            }
            Off();
         }

         protected override float GetScaleOffset()
         {
            float k0 = GetOffset(0);
            float k250 = GetOffset(250);
            float k350 = GetOffset(150);
            float y = k0;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && IsOn())
            {
               double temp = inspecteur.GetTemperature();
               if (temp > MAX_TEMP) temp = MAX_TEMP;
               if (temp < MIN_TEMP) temp = MIN_TEMP;
               if (temp <= 250.0)
               {
                  y = k250 - 140.2110808f * (float)Math.Log10(1 + (250 - temp) / 60.0) / 400.0f;
               }
               else if (temp>250 && temp<350)
               {
                  y = k250 + (float)(temp - 250.0f) / 400.0f;
               }
               else
               {
                  y = k350 + 48.5f * (float)Math.Log10(1 + (temp-350) / 40.0) / 400.0f;
               }
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:TEMP";
         }
      }
   }
}
