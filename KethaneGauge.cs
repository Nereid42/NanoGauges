using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class KethaneGauge : AbstractResourceGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/KET-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/KET-scale");

         private readonly ResourceInspecteur inspecteur;

         public KethaneGauge(ResourceInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_KETHANE, inspecteur, Resources.KETHANE, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Kethane";
         }

         public override string GetDescription()
         {
            return "Remaining kethane in percent.";
         }

         public override string ToString()
         {
            return "Gauge:KET";
         }
      }
   }
}
