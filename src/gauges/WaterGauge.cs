using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class WaterGauge : AbstractResourceGauge
      {
         private static Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/H2O-skin");
         private static Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/H2O-scale");

         private readonly ResourceInspecteur inspecteur;

         public WaterGauge(ResourceInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_H2O, inspecteur, Resources.WATER, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Water";
         }

         public override string GetDescription()
         {
            return "Remaining water in percent.";
         }

         public override string ToString()
         {
            return "Gauge:H2O";
         }
      }
   }
}
