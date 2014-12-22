using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class SolidFuelGauge : AbstractResourceGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/SRB-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/SRB-scale");

         private readonly ResourceInspecteur inspecteur;

         public SolidFuelGauge(ResourceInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_SRB, inspecteur, Resources.SOLID_FUEL, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Solid Fuel";
         }

         public override string GetDescription()
         {
            return "Remaining solid fuel in percent.";
         }

         public override string ToString()
         {
            return "Gauge:SRB";
         }
      }
   }
}
