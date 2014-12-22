using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class AirIntakeGauge : AbstractResourceGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/AIR-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/AIR-scale");
         private const double MAX_AIR = 5000;

         private readonly ResourceInspecteur inspecteur;

         public AirIntakeGauge(ResourceInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_AIRIN, inspecteur, Resources.INTAKE_AIR, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Air Intake";
         }

         public override string GetDescription()
         {
            return "Current absolute air intake.";
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
               y = b + 150.0f * (float)Math.Log10(1 + air) / (float)SCALE_HEIGHT;
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:AIR";
         }
      }
   }
}
