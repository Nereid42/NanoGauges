using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class PropellantPctGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/PROP-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/PROP-scale");
         private const float MAX_PCT = 100;
         private const float MIN_PCT = 0;

         private readonly EngineInspecteur inspecteur;

         public PropellantPctGauge(EngineInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_PROPELLANT, SKIN, SCALE, true, 0.0010f)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Prop. Req.";
         }

         public override string GetDescription()
         {
            return "Propellant requirements in percent for all running engines.";
         }

         protected override float GetScaleOffset()
         {
            float b = GetLowerOffset();
            float y = b;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null)
            {
               float pct = inspecteur.propReqPerRunningEngine;
               if (pct > MAX_PCT)
               {
                  pct = MAX_PCT;
                  OutOfLimits();
               }
               else if (pct < MIN_PCT)
               {
                  pct = MIN_PCT;
                  OutOfLimits();
               }
               else
               {
                  InLimits();
               }
               if (pct >= 0)
               {
                  y = b + 300.0f * pct / 100.0f / (float)SCALE_HEIGHT;
               }
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:PROP";
         }
      }
   }
}
