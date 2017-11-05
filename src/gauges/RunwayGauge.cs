using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {
      class RunwayGauge : HorizontalTextGauge        
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/RUNWAY-skin");
         private static readonly Texture2D BACK = Utils.GetTexture("Nereid/NanoGauges/Resource/RUNWAY-back");


         public RunwayGauge()
            : base(Constants.WINDOW_ID_GAUGE_RUNWAY, SKIN,BACK)
         {
         }

         public override string GetName()
         {
            return "Runway";
         }

         public override string GetDescription()
         {
            return "Selected runway for ILS.";
         }

         protected override String GetText()
         {
            Airfield airfield = NavGlobals.destinationAirfield;
            if (airfield == null) return "- no airfield -";
            Runway runway = NavGlobals.landingRunway;
            if (runway == null) return "- no runway -";
            return airfield.code +" " + runway.name;
         }
      }
   }
}
