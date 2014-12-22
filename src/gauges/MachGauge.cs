using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class MachGauge : VerticalGauge
      {
         private static Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/MACH-skin");
         private static Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/MACH-scale");
         private const double MAX_MACH = 10;


         public MachGauge()
            : base(Constants.WINDOW_ID_GAUGE_MACH, SKIN, SCALE)
         {
         }

         public override string GetName()
         {
            return "Mach";
         }

         public override string GetDescription()
         {
            return "Current mach number (requires Ferram Aerospace Research to work).";
         }

         protected override void AutomaticOnOff()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && vessel.parts.Count > 0 && vessel.atmDensity>0.0)
            {
               On();
            }
            else
            {
               Off();
            }
         }

         private double GetMachNumber(Vessel vessel)
         {
            if(NanoGauges.farAdapter.IsInstalled())
            {
               return NanoGauges.farAdapter.GetMachNumber(vessel.mainBody, vessel.altitude, vessel.srf_velocity);
            }
            return NanoGauges.farAdapter.ApproximateMachNumber(vessel.mainBody, vessel.atmDensity, vessel.altitude, vessel.srf_velocity);
         }

         protected override float GetScaleOffset()
         {
            float b = GetLowerOffset();
            float y = b;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && IsOn())
            {
               double mach = GetMachNumber(vessel);
               if(!double.IsNaN(mach))
               {
                  if (mach > MAX_MACH) mach = MAX_MACH;
                  if (mach < 0) mach = 0;
                  y = b + 30.0f * (float)mach / 400.0f;
               }
            }
            return y;
         }

         public override string ToString()
         {
            return "Gauge:MACH";
         }
      }
   }
}
