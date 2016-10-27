using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class IspPerEngineGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/ISPE-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/ISPE-scale");
         private const double MAX_ISP = 2000.0;
         private const double MIN_ISP = 200.0;

         private readonly EngineInspecteur inspecteur;

         public IspPerEngineGauge(EngineInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_ISPE, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Isp / Engine";
         }

         public override string GetDescription()
         {
            return "Current specific impulse (Isp) per running engine.";
         }

         protected override void AutomaticOnOff()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && vessel.parts.Count > 0 && inspecteur.enginesRunningCount > 0)
            {
               On();
            }
            else
            {
               Off();
            }
         }

         private void IspOutOfLimits()
         {
           if (IsOn()) 
           {
              NotInLimits();
           }
           else 
           {
              InLimits();
           }
         }

         protected override float GetScaleOffset()
         {
            float b = GetLowerOffset();
            float y = b;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null)
            {
               double isp = inspecteur.engineIspPerRunningEngine;
               if (isp > MAX_ISP)
               {
                  isp = MAX_ISP;
                  IspOutOfLimits();
               }
               else if (isp < MIN_ISP)
               {
                  isp = MIN_ISP;
                  IspOutOfLimits();
               }
               else
               {
                  InLimits();
               }
               y = (float)(b + 7.075f * Math.Sqrt(isp - MIN_ISP) / 400.0f);
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:ISPE";
         }
      }
   }
}
