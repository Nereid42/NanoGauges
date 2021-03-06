﻿using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class TerminalVelocityGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/TV-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/TV-scale");
         private const double MAX_VT = 10000.0;
         private const double MIN_VT = -10000.0;

         private readonly VesselInspecteur inspecteur;

         public TerminalVelocityGauge(VesselInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_VT, SKIN, SCALE, true, 0.00085f)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Terminal\nVelocity";
         }

         public override string GetDescription()
         {
            return "\n\nCurrent deviation to terminal velocity. A value of 0 means optimal fuel burn. Not very usable at higher altitudes.";
         }

         protected override void AutomaticOnOff()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && vessel.mainBody != null && vessel.parts.Count > 0 && vessel.altitude < vessel.mainBody.MaxAtmosphereAltitude())
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
            float c = GetCenterOffset();
            float y = c;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && vessel.mainBody!=null)
            {
               CelestialBody body = vessel.mainBody;

               //double cw = 0.00765;
               double cw = 0.000135;
               double m = 0.05;
               double vt = GameUtils.TerminalVelocity(body, m, vessel.altitude, cw);

               if( double.IsNaN(vt) )
               {
                  NotInLimits();
                  return y;
               }
               //
               double dvt = Math.Abs(vessel.verticalSpeed) - vt;
               if (double.IsNaN(dvt))
               {
                  NotInLimits();
                  return y;
               }

               InLimits();

               if (dvt > MAX_VT) dvt = MAX_VT;
               if (dvt < MIN_VT) dvt = MIN_VT;

               if (dvt >= 0)
               {
                  y = c + 37.5f * (float)Math.Log10(1 + dvt) / 400.0f;
               }
               else
               {
                  y = c - 37.5f * (float)Math.Log10(1 - dvt) / 400.0f;
               }
   
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:ISP";
         }
      }
   }
}
