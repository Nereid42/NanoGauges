using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class FoodGauge : AbstractResourceGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/FOOD-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/FOOD-scale");

         private readonly ResourceInspecteur inspecteur;

         public FoodGauge(ResourceInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_FOOD, inspecteur, Resources.FOOD, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Food";
         }

         public override string GetDescription()
         {
            return "Remaining food in percent.";
         }


         public override string ToString()
         {
            return "Gauge:FOOD";
         }
      }
   }
}
