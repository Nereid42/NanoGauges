using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class EvaMonopropellantGauge : AbstractResourceGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/EVAMP-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/EVAMP-scale");

         private readonly ResourceInspecteur inspecteur;

         public EvaMonopropellantGauge(ResourceInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_EVAMP, inspecteur, Resources.EVA_PROPELLANT, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "EVA\nMonopropellant";
         }

         public override string GetDescription()
         {
            return "\n\nRemaining eva monopropellant in percent.";
         }

         public override string ToString()
         {
            return "Gauge:EVAMP";
         }
      }
   }
}
