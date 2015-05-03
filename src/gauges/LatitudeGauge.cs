using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {
      class LatitudeGauge : HorizontalTextGauge        
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/LATITUDE-skin");
         private static readonly Texture2D BACK = Utils.GetTexture("Nereid/NanoGauges/Resource/LATITUDE-back");

         public LatitudeGauge()
            : base(Constants.WINDOW_ID_GAUGE_LATITUDE, SKIN, BACK)
         {
         }

         public override string GetName()
         {
            return "Latitude";
         }

         public override string GetDescription()
         {
            return "Current vessel latitude coordinates.";
         }

         protected override String GetText()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if(vessel!=null)
            {
               double lat = vessel.latitude;
               if(lat>=0)
               {
                  return "N " + lat.ToString("000.0000") + "°";
               }
               else
               {
                  return "S " + (-lat).ToString("000.0000") + "°";
               }
            }
            else
            {
               return "N/A";
            }
         }

      }
   }
}
