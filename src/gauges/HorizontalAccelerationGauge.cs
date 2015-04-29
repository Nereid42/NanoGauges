using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class HorizontalAccelerationGauge : VerticalGauge
      {
         private static Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/HACCL-skin");
         private static Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/HACCL-scale");
         private const double MAX_VALUE = 500;
         private const double MIN_VALUE = -500;
         private const double MIN_SPEED = 1;


         private readonly AccelerationInspecteur inspecteur;

         public HorizontalAccelerationGauge(AccelerationInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_HACCL, SKIN, SCALE, true, 0.00075f)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Horizontal\nAcceleration";
         }

         public override string GetDescription()
         {
            return "\n\nCurrent horizontal acceleration in m/s^2.";
         }


         protected override float GetScaleOffset()
         {
            float c = GetCenterOffset();
            float y = c;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null)
            {
               // check for minimum speed
               if (vessel.srfSpeed < MIN_SPEED)
               {
                  OutOfLimits();
                  return y;
               }
               double acceleration = inspecteur.HorizontalAcceleration();
               if (!double.IsNaN(acceleration))
               {
                  if (acceleration > MAX_VALUE)
                  {
                     acceleration = MAX_VALUE;
                     OutOfLimits();
                  }
                  else if (acceleration < MIN_VALUE)
                  {
                     acceleration = MIN_VALUE;
                     OutOfLimits();
                  }
                  else
                  {
                     InLimits();
                  }

                  if(acceleration>=0)
                  {
                     y = (float)(c + 44.0f * Math.Log10(1 + 5 * acceleration) / 400.0f);
                  }
                  else
                  {
                     y = (float)(c - 44.0f * Math.Log10(1 - 5 * acceleration) / 400.0f);
                  }
               }
            }
            return y;
         }

         public override string ToString()
         {
            return "Gauge:HACCL";
         }
      }
   }
}
