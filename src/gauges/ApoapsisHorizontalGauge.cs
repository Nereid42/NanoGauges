using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {
      class ApoapsisHorizontalGauge : HorizontalDigitalGauge
      { 
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/APOAPSIS-skin");


         public ApoapsisHorizontalGauge()
            : base(Constants.WINDOW_ID_GAUGE_APOAPSIS, SKIN)
         {
         }

         public override string GetName()
         {
            return "Apoapsis";
         }

         public override string GetDescription()
         {
            return "Apoapsis of the vessel.";
         }

         protected override double GetValue()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if(vessel == null) return 0.0f;
            Orbit orbit = vessel.orbit;
            if(orbit == null) return 0.0f;
            return (orbit.ApA+500) / 1000.0;
         }

      }
   }
}
