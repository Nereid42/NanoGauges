using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class IspDeltaGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/DISP-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/DISP-scale");
         private const double MAX_DISP = 5.0;
         private const double MIN_DISP = -5.0;

         private readonly EngineInspecteur inspecteur;

         public IspDeltaGauge(EngineInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_DISP, SKIN, SCALE, true, 0.00085f)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Isp Rate";
         }

         public override string GetDescription()
         {
            return "Current change per second of average specific impulse (Isp) for all running engines.";
         }

         protected override void AutomaticOnOff()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && vessel.parts.Count > 0 && inspecteur.GetTotalEnginesCount() > 0)
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
            float m = GetCenterOffset();
            float y = m;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null)
            {
               double disp = inspecteur.GetDeltaIspperSecond();
               if (disp>MAX_DISP)  disp = MAX_DISP;
               if (disp<MIN_DISP)  disp = MIN_DISP;
               if(disp>=0)
               {
                  y = (float)(m + 193.0f * Math.Log10(1+disp) / 400.0f);
               }
               else
               {
                  y = (float)(m - 193.0f * Math.Log10(1-disp) / 400.0f);
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
