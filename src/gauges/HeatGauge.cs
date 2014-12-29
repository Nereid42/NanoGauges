using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class HeatGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/HEAT-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/HEAT-scale");
         private static readonly double MAX_TEMP = 8000;
         private const double MIN_TEMP = -273;

         private readonly VesselInspecteur inspecteur;

         public HeatGauge(VesselInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_HEAT, SKIN, SCALE,true, 0.0004f)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Heatshield\nTemp";
         }

         public override string GetDescription()
         {
            return "\n\nTemperature of the heatshield on the vessel.";
         }

         protected override void AutomaticOnOff()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null  && FlightGlobals.ActiveVessel.parts.Count > 0)
            {
               if(inspecteur.IsHeatshieldInstalled())
               {
                  On();
                  return;
               }
            }
            Off();
         }

         protected override float GetScaleOffset()
         {
            float m = GetOffset(300);
            float y = m; 
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && IsOn())
            {
               double temp = inspecteur.GetHeatshieldTemp();
               if (temp > MAX_TEMP)
               {
                  temp = MAX_TEMP;
                  OutOfLimits();
               }
               else
               {
                  InLimits();
               }
               if (temp < MIN_TEMP) temp = MIN_TEMP;
               if(temp>=0.0)
               {
                  y = m + 108.5f * (float)Math.Log10(1 + temp/40.0) / 400.0f;
               }
               else
               {
                  y = m - 55.5f * (float)Math.Log10(1 - temp/40.0) / 400.0f;
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
