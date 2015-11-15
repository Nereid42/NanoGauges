using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class KarboniteGauge : AbstractResourceGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/KARB-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/KARB-scale");

         private readonly ResourceInspecteur inspecteur;

         public KarboniteGauge(ResourceInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_KARBONITE, inspecteur, Resources.KARBONITE, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Karbonite";
         }

         public override string GetDescription()
         {
            return "Loaded Karbonite in percent of total capacity.";
         }


         public override string ToString()
         {
            return "Gauge:KARB";
         }
      }
   }
}
