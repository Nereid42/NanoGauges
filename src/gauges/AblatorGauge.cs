using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class AblatorGauge : AbstractResourceGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/ABLAT-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/ABLAT-scale");

         private readonly ResourceInspecteur inspecteur;

         public AblatorGauge(ResourceInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_ABLAT, inspecteur, Resources.ABLATOR, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Ablative Shielding";
         }

         public override string GetDescription()
         {
            return "Remaining ablative shielding in percent.";
         }

         public override string ToString()
         {
            return "Gauge:SHIELD";
         }
      }
   }
}
