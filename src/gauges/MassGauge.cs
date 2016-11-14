using System;
using UnityEngine;
using KSP.IO;
using System.Diagnostics;

namespace Nereid
{
   namespace NanoGauges
   {

      public class MassGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/MASS-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/MASS-scale");
         private const double MAX_MASS = 5000;

         private readonly VesselInspecteur inspecteur;

         public MassGauge(VesselInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_MASS, SKIN, SCALE)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Mass of Vessel";
         }

         public override string GetDescription()
         {
            return "Current accelerationTotal mass of the current vessel including fuel.";
         }

         protected override float GetScaleOffset()
         {
            float b = GetLowerOffset();
            float y = b;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null)
            {
               double mass = inspecteur.GetTotalMass();

               if (mass > MAX_MASS)
               {
                  mass = MAX_MASS;
                  NotInLimits();
               }
               else
               {
                  InLimits();
               }

               if (mass >= 0)
               {
                  y = b + 81f * (float)Math.Log10(1 + mass) / 400.0f;
               }
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:MASS";
         }
      }
   }
}
