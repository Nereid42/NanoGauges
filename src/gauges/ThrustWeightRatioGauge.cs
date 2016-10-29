using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class ThrustWeightRatioGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/TWR-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/TWR-scale");
         private const double MAX_TWR = 10;
         
         private readonly EngineInspecteur inspecteur;

         public ThrustWeightRatioGauge(EngineInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_TWR, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Thrust-Weight-\nRatio";
         }

         public override string GetDescription()
         {
            return "\n\nThe ratio of thrust compared to weight. A value greater 1.0 allows a vessel to liftoff.";
         }

         protected override void AutomaticOnOff()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && FlightGlobals.ActiveVessel.parts.Count > 0)
            {
               if (vessel.mainBody != null)
               {
                  On();
               }
               else
               {
                  Off();
               }
            }
            else
            {
               Off();
            }
         }

         protected override float GetScaleOffset()
         {
            float b = GetLowerOffset();
            float y = b;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && !vessel.isEVA)
            {
               if(vessel.mainBody!=null)
               {
                  double thrust = inspecteur.engineTotalThrust;
                  double g = vessel.mainBody.GeeASL*Constants.GEE_KERBIN;
                  double m = vessel.GetTotalMass(); 
                  if(m>0)
                  {
                     double twr = thrust / (m * g);
                     if (twr > MAX_TWR) twr = MAX_TWR;
                     if (twr < 0) twr = 0;
                     y = (float)(b + 149.5f * Math.Log10(1 + 10 * twr) / 400.0f);
                  }
               }
            }
            else
            {
               Off();
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:TWR";
         }
      }
   }
}
