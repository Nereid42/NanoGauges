using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {
      class VesselGauge : HorizontalTextGauge        
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/VESSEL-skin");
         private static readonly Texture2D BACK = Utils.GetTexture("Nereid/NanoGauges/Resource/VESSEL-back");


         public VesselGauge()
            : base(Constants.WINDOW_ID_GAUGE_BIOME,SKIN,BACK)
         {
         }

         public override string GetName()
         {
            return "Vessel";
         }

         public override string GetDescription()
         {
            return "Name of the controlled vessel.";
         }

         protected override String GetText()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel == null) return null;
            if (vessel.protoVessel == null) return null;
            return vessel.protoVessel.vesselName;            
         }
      }
   }
}
