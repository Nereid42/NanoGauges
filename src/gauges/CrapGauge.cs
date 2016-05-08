using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class CrapGauge : AbstractResourceGauge
      {
         private static Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/CRAP-skin");
         private static Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/CRAP-scale");

         private readonly ResourceInspecteur inspecteur;

         public CrapGauge(ResourceInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_CRAP, inspecteur, Resources.WASTE, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Crap";
         }

         public override string GetDescription()
         {
            return "Accumulated crap in percent.";
         }

         public override string ToString()
         {
            return "Gauge:CRAP";
         }
      }
   }
}
