using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class WasteGauge : AbstractResourceGauge
      {
         private static Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/WASTE-skin");
         private static Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/WASTE-scale");

         private readonly ResourceInspecteur inspecteur;

         public WasteGauge(ResourceInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_WASTE, inspecteur, Resources.WASTE, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Waste";
         }

         public override string GetDescription()
         {
            return "Accumulated waste in percent.";
         }

         public override string ToString()
         {
            return "Gauge:WASTE";
         }
      }
   }
}
