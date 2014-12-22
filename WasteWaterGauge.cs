using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class WasteWaterGauge : AbstractResourceGauge
      {
         private static Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/WH2O-skin");
         private static Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/WH2O-scale");

         private readonly ResourceInspecteur inspecteur;

         public WasteWaterGauge(ResourceInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_WH2O, inspecteur, Resources.WASTEWATER, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Wastewater";
         }

         public override string GetDescription()
         {
            return "Accumulated wastewater in percent.";
         }

         public override string ToString()
         {
            return "Gauge:WH2O";
         }
      }
   }
}
