using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class CarbonDioxideGauge : AbstractResourceGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/CO2-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/CO2-scale");

         private readonly ResourceInspecteur inspecteur;

         public CarbonDioxideGauge(ResourceInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_CO2, inspecteur, Resources.CARBONDIOXIDE, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Carbon Dioxide";
         }

         public override string GetDescription()
         {
            return "Accumulated carbon dioxide in percent.";
         }

         public override string ToString()
         {
            return "Gauge:CO2";
         }
      }
   }
}
