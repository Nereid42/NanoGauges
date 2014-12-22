using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class ShieldGauge : AbstractResourceGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/SHIELD-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/SHIELD-scale");

         private readonly ResourceInspecteur inspecteur;

         public ShieldGauge(ResourceInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_SHIELD, inspecteur, Resources.ABLATIVE_SHIELDING, SKIN, SCALE)
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
