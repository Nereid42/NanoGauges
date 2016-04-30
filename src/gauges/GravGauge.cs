using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class GravGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/GRAV-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/GRAV-scale");
         private static readonly double MAX_GRAV = 100;
         private const double MIN_GRAV = 0;

         private readonly SensorInspecteur inspecteur;

         public GravGauge(SensorInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_GRAVIMETER, SKIN, SCALE, true, 0.00085f)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Gravimeter";
         }

         public override string GetDescription()
         {
            return "Average of all gravimeter readings (if any) on the vessel.";
         }

         protected override void AutomaticOnOff()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null  && FlightGlobals.ActiveVessel.parts.Count > 0)
            {
               if(inspecteur.IsGravSensorEnabled())
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
               double grav = inspecteur.GetGravity();
               if (grav > MAX_GRAV)
               {
                  grav = MAX_GRAV;
                  OutOfLimits();
               }
               if (grav < MIN_GRAV)
               {
                  grav = MAX_GRAV;
                  OutOfLimits();
               }
               else
               {
                  InLimits();
               }
               y = b + 148.5f * (float)Math.Log10(1 + grav) / 400.0f;
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:GRAV";
         }
      }
   }
}
