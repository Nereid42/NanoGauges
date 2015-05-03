using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {
      class LongitudeGauge : HorizontalTextGauge        
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/LONGITUDE-skin");
         private static readonly Texture2D BACK = Utils.GetTexture("Nereid/NanoGauges/Resource/LONGITUDE-back");

         public LongitudeGauge()
            : base(Constants.WINDOW_ID_GAUGE_LONGITUDE, SKIN, BACK)
         {
         }

         public override string GetName()
         {
            return "Longitude";
         }

         public override string GetDescription()
         {
            return "Current vessel longitude coordinates.";
         }

         protected override String GetText()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if(vessel!=null)
            {
               double lon = vessel.longitude % 360d;

               if (lon < -180d)
               {
                  lon += 360d;
               }
               if (lon >= 180)
               {
                  lon -= 360d;
               }


               if (lon >= 0)
               {
                  return "E " + lon.ToString("000.0000") + "°";
               }
               else
               {
                  return "W " + (-lon).ToString("000.0000") + "°";
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
