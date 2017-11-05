using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {
      class AirfieldGauge : HorizontalTextGauge        
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/AIRFIELD-skin");
         private static readonly Texture2D BACK = Utils.GetTexture("Nereid/NanoGauges/Resource/AIRFIELD-back");


         public AirfieldGauge()
            : base(Constants.WINDOW_ID_GAUGE_AIRFIELD, SKIN,BACK)
         {
         }

         public override string GetName()
         {
            return "Airfield";
         }

         public override string GetDescription()
         {
            return "Selected airfield for ILS.";
         }

         protected override String GetText()
         {
            Airfield airfield = NavGlobals.destinationAirfield;
            if (airfield == null) return "- no airfield -";
            return airfield.name;
         }
      }
   }
}
