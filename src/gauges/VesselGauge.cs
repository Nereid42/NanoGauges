using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Nereid
{
   namespace NanoGauges
   {
      class VesselGauge : HorizontalTextGauge        
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/VESSEL-skin");
         private static readonly Texture2D BACK = Utils.GetTexture("Nereid/NanoGauges/Resource/VESSEL-back");


         public VesselGauge()
            : base(Constants.WINDOW_ID_GAUGE_VESSEL,SKIN,BACK)
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

         private String GetTextInEva(Vessel vessel)
         {
            List<ProtoCrewMember> crew = vessel.GetVesselCrew();
            if(crew.Count>0)
            {
               return crew.First().name;
            }
            return vessel.name;
         }

         protected override String GetText()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel == null) return null;
            if (vessel.isEVA) return GetTextInEva(vessel);
            if (vessel.vesselName != null && vessel.vesselName.Length > 0) return vessel.vesselName;
            if (vessel.protoVessel == null) return null;
            return vessel.protoVessel.vesselName;
         }
      }
   }
}
