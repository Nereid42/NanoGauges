using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class CO2Gauge : AbstractResourceGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/CO2-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/CO2-scale");

         private readonly ResourceInspecteur inspecteur;

         public CO2Gauge(ResourceInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_CO2, inspecteur, Resources.CO2, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "CO2";
         }

         public override string GetDescription()
         {
            return "Accumulated CO2 in percent.";
         }

         public override string ToString()
         {
            return "Gauge:CO2";
         }
      }
   }
}
