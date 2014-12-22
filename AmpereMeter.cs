using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class AmpereMeter : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/AMP-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/AMP-scale");
         private const double MAX_RATE = 1000;

         private readonly ResourceInspecteur inspecteur;

         public AmpereMeter(ResourceInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_AMP, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Ampere";
         }

         public override string GetDescription()
         {
            return "Current usage of electric charge.";
         }

         protected override float GetScaleOffset()
         {
            float m = GetCenterOffset();
            float y = m;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null)
            {
               double rate = inspecteur.GetRate(Resources.ELECTRIC_CHARGE);
               if (rate > MAX_RATE) rate = MAX_RATE;
               if (rate < -MAX_RATE) rate = -MAX_RATE;
               if(rate>0)
               {
                  y = (float)(m + 50.0f * Math.Log10(1 + rate) / 400.0f);
               }
               else
               {
                  y = (float)(m - 50.0f * Math.Log10(1 - rate) / 400.0f);
               }
               //Log.Test("angular: "+vessel.angularVelocity.ToString("0.00"));
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:AMP";
         }
      }
   }
}
