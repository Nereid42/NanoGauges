using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class SeismicGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/SEISMIC-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/SEISMIC-scale");
         private static readonly double MAX_ACC = 500;
         private const double MIN_ACC = -500;

         private readonly SensorInspecteur inspecteur;

         public SeismicGauge(SensorInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_SEISMOMETER, SKIN, SCALE, true, 0.00085f)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Seismometer";
         }

         public override string GetDescription()
         {
            return "Average of all seismometer readings (if any) on the vessel.";
         }

         protected override void AutomaticOnOff()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null  && FlightGlobals.ActiveVessel.parts.Count > 0)
            {
               if(inspecteur.IsSeismicSensorEnabled())
               {
                  On();
                  return;
               }
            }
            Off();
         }

         protected override float GetScaleOffset()
         {
            float c = GetCenterOffset();
            float y = c;

            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null)
            {
               double acc = inspecteur.GetSeismic();
               if (acc > MAX_ACC)
               {
                  acc = MAX_ACC;
                  NotInLimits();
               }
               if (acc < MIN_ACC)
               {
                  acc = MIN_ACC;
                  NotInLimits();
               }
               else
               {
                  InLimits();
               }

               if (!double.IsNaN(acc))
               {
                  if (acc >= 0)
                  {
                     y = (float)(c + 55.5f * Math.Log10(1 + 10 * acc) / 400.0f);
                  }
                  else
                  {
                     y = (float)(c - 55.5f * Math.Log10(1 - 10 * acc) / 400.0f);
                  }
               }
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:SEISMIC";
         }
      }
   }
}
