using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class OxidizerGauge : AbstractResourceGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/OXID-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/OXID-scale");

         private readonly ResourceInspecteur inspecteur;

         public OxidizerGauge(ResourceInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_OXID, inspecteur,Resources.OXIDIZER, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Oxidizer";
         }

         public override string GetDescription()
         {
            return "Remaining oxidizer in percent.";
         }

         public override string ToString()
         {
            return "Gauge:OXID";
         }
      }
   }
}
