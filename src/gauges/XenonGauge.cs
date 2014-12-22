using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class XenonGauge : AbstractResourceGauge
      {
         private static Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/XENON-skin");
         private static Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/XENON-scale");

         private readonly ResourceInspecteur inspecteur;

         public XenonGauge(ResourceInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_XENON, inspecteur, Resources.XENON_GAS, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Xenon";
         }

         public override string GetDescription()
         {
            return "Remaining xenon in percent.";
         }

         public override string ToString()
         {
            return "Gauge:XENON";
         }
      }
   }
}
