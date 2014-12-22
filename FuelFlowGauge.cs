using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class FuelFlowGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/FLOW-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/LOG2000-scale");
         private const double MAX_RATE = 2000;

         private readonly ResourceInspecteur inspecteur;

         public FuelFlowGauge(ResourceInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_FLOW, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Liquid Fuel Flow";
         }

         public override string GetDescription()
         {
            return "Current rate of liquid fuel usage.";
         }

         protected override float GetScaleOffset()
         {
            float b = GetLowerOffset();
            float y = b;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null)
            {
               double rate = Math.Abs(inspecteur.GetRate(Resources.LIQUID_FUEL));
               if (rate > MAX_RATE) rate = MAX_RATE;
               y = (float)(b + 91.0f * Math.Log10(1 + rate) / 400.0f);
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:FLOW";
         }
      }
   }
}
