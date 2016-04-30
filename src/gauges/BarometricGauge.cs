using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class BarometricGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/BARO-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/BARO-scale");
         private static readonly double MAX_PRES = 2000;
         private const double MIN_PRES = 0;

         private readonly SensorInspecteur inspecteur;

         public BarometricGauge(SensorInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_BAROMETER, SKIN, SCALE, true, 0.00025f)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Barometer";
         }

         public override string GetDescription()
         {
            return "\nThe atmospheric pressure.";
         }

         protected override void AutomaticOnOff()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && FlightGlobals.ActiveVessel.parts.Count > 0)
            {
               if (inspecteur.IsPressureSensorEnabled())
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
            float y = b;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && IsOn())
            {
               double pressure = inspecteur.GetPressure();
               if (pressure > MAX_PRES)
               {
                  pressure = MAX_PRES;
                  OutOfLimits();
               }
               if (pressure < MIN_PRES)
               {
                  pressure = MIN_PRES;
                  OutOfLimits();
               }
               else
               {
                  InLimits();
               }
               y = b + 75.0f * (float)Math.Log10(1 + pressure) / 400.0f;
            }

            return y;
         }


         public override string ToString()
         {
            return "Gauge:BAROMETER";
         }
      }
   }
}
