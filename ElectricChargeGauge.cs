using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class ElectricChargeGauge : AbstractResourceGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/CHARGE-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/CHARGE-scale");

         private readonly ResourceInspecteur inspecteur;

         public ElectricChargeGauge(ResourceInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_CHARGE, inspecteur, Resources.ELECTRIC_CHARGE, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Electric Charge";
         }

         public override string GetDescription()
         {
            return "Remaining electric charge in percent.";
         }


         public override string ToString()
         {
            return "Gauge:CHARGE";
         }
      }
   }
}
