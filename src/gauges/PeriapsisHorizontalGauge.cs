using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {
      class PeriapsisHorizontalGauge : HorizontalDigitalGauge
      { 
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/PERIAPSIS-skin");


         public PeriapsisHorizontalGauge()
            : base(Constants.WINDOW_ID_GAUGE_PERIAPSIS, SKIN)
         {
         }

         public override string GetName()
         {
            return "Periapsis";
         }

         public override string GetDescription()
         {
            return "Periapsis of the vessel.";
         }

         protected override double GetValue()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if(vessel == null) return 0.0f;
            Orbit orbit = vessel.orbit;
            if(orbit == null) return 0.0f;
            return (orbit.PeA+500) / 1000.0;
         }

      }
   }
}
