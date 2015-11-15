using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class ThrustGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/THRUST-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/THRUST-scale");
         private const double MAX_THRUST = 100000;

         private readonly EngineInspecteur inspecteur;

         public ThrustGauge(EngineInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_THRUST, SKIN, SCALE,true,0.00085f)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Thrust";
         }

         public override string GetDescription()
         {
            return "Current thrust of all engines.";
         }

         protected override float GetScaleOffset()
         {
            float b = GetLowerOffset();
            float y = b;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null)
            {
               double thrust = inspecteur.engineTotalThrust;
               if (thrust > MAX_THRUST)
               {
                  thrust = MAX_THRUST;
                  OutOfLimits();
               }
               else
               {
                  InLimits();
               }
               y = (float)(b + 60.0f * Math.Log10(1 + thrust) / 400.0f);
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:THRUST";
         }
      }
   }
}
