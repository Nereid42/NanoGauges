using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class KethaneAirIntakeGauge : AbstractResourceGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/KAIR-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/KAIR-scale");
         private const double MAX_AIR = 5000;

         private readonly ResourceInspecteur inspecteur;

         public KethaneAirIntakeGauge(ResourceInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_KAIRIN, inspecteur, Resources.KINTAKE_AIR, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Kethane Air Intake";
         }

         public override string GetDescription()
         {
            return "Current kethane air intake.";
         }

         protected override float GetScaleOffset()
         {
            float b = GetLowerOffset();
            float y = b;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null)
            {
               double air = inspecteur.GetAmount(Resources.INTAKE_AIR);

               if (air > MAX_AIR) air = MAX_AIR;
               if(air<0) air=0;
               y = b + 150.0f * (float)Math.Log10(1+air) / 400.0f;
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:KAIR";
         }
      }
   }
}
