using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {

      public class AtmosphereGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/ATM-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/ATM-scale");
         private static double MAX_ATM = 50;

         public AtmosphereGauge()
            : base(Constants.WINDOW_ID_GAUGE_ATM, SKIN, SCALE,true, 0.001f)
         {
         }

         public override string GetName()
         {
            return "Atmosphere";
         }

         public override string GetDescription()
         {
            return "Atmospheric density.";
         }

         protected override float GetScaleOffset()
         {
            float b = GetLowerOffset();
            float y = b;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null)
            {
               double atm = vessel.atmDensity;
               if (atm > MAX_ATM)
               {
                  atm = MAX_ATM;
                  NotInLimits();
               }
               else
               {
                  if(vessel.mainBody!=null && vessel.altitude<=vessel.mainBody.MaxAtmosphereAltitude())
                  {
                     InLimits();
                  }
                  else
                  {
                     NotInLimits();
                  }
               }
               if (atm < 0) atm = 0;
               if (atm >= 0 && atm<= 1.0)
               {
                  y = b + 144.025f * (float)Math.Log10(1 + 10 * atm) / 400.0f;
               }
               else if (atm > 1)
               {
                  float m = GetCenterOffset();
                  y = m + 88.29f * (float)Math.Log10(1 + (atm-1)) / 400.0f;
               }
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:ATM";
         }
      }
   }
}
