using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class MonopropellantGauge : AbstractResourceGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/MONO-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/MONO-scale");

         private readonly ResourceInspecteur inspecteur;

         public MonopropellantGauge(ResourceInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_MONO, inspecteur, Resources.MONOPROPELLANT, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }


         public override string GetName()
         {
            return "Monopropellant";
         }

         public override string GetDescription()
         {
            return "Remaining monopropellant in percent.";
         }

         public override string ToString()
         {
            return "Gauge:MONO";
         }
      }
   }
}
