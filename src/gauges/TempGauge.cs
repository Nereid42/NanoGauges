using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class TempGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/TEMP-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/TEMP-scale");
         private static readonly double MAX_TEMP = 2000;
         private const double MIN_TEMP = -273;

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
            return "Average of all temperature readings (if any) on the vessel.";
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
            float m = GetOffset(250);
            float p50 = GetOffset(200);
            float n50 = GetOffset(300);
            float y = m; 
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && IsOn())
            {
               double temp = inspecteur.GetTemperature();
               if (temp > MAX_TEMP) temp = MAX_TEMP;
               if (temp < MIN_TEMP) temp = MIN_TEMP;
               if(temp<=50.0 && temp>=-50.0)
               {
                  y = m + (float)(temp/ 400.0f);
               }
               else if (temp>50)
               {
                  y = p50 + 88.5f * (float)Math.Log10(1 + (temp-50.0)/40.0) / 400.0f;
               }
               else
               {
                  y = n50 - 45.0f * (float)Math.Log10(1 - (temp + 50.0) / 20.0) / 400.0f;
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
