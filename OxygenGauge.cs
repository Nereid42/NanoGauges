using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class OxygenGauge : AbstractResourceGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/O2-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/O2-scale");

         private readonly ResourceInspecteur inspecteur;

         public OxygenGauge(ResourceInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_O2, inspecteur, Resources.OXYGEN, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Oxygen";
         }

         public override string GetDescription()
         {
            return "Remaining oxygen in percent.";
         }

         public override string ToString()
         {
            return "Gauge:O2";
         }
      }
   }
}
