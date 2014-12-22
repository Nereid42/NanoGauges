using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class FuelGauge : AbstractResourceGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/FUEL-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/FUEL-scale");

         private readonly ResourceInspecteur inspecteur;

         public FuelGauge(ResourceInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_FUEL, inspecteur, Resources.LIQUID_FUEL, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Liquid Fuel";
         }

         public override string GetDescription()
         {
            return "Remaining liquid fuel in percent.";
         }

         public override string ToString()
         {
            return "Gauge:FUEL";
         }
      }
   }
}
