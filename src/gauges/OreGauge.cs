using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class OreGauge : AbstractResourceGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/ORE-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/ORE-scale");

         private readonly ResourceInspecteur inspecteur;

         public OreGauge(ResourceInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_ORE, inspecteur, Resources.ORE, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Ore";
         }

         public override string GetDescription()
         {
            return "Stored ore in percent of total capacity";
         }

         public override string ToString()
         {
            return "Gauge:ORE";
         }
      }
   }
}
