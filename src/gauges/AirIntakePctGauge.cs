using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class AirIntakePctGauge : AbstractResourceGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/RAIR-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/RAIR-scale");

         private readonly ResourceInspecteur inspecteur;

         public AirIntakePctGauge(ResourceInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_AIRPCT, inspecteur, Resources.INTAKE_AIR, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Air Intake";
         }

         public override string GetDescription()
         {
            return "Current air intake in percent of maximum intake.";
         }

         public override string ToString()
         {
            return "Gauge:RAIR";
         }
      }
   }
}
