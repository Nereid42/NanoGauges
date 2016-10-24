using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nereid
{
   namespace NanoGauges
   {
      public class Airport
      {
         public readonly String name;
         public readonly Runway[] runways;

         public Airport(String name, params Runway[] runways)
         {
            this.name = name;
            this.runways = runways;
         }

         public Runway GetLandingRunwayForBearing(float bearing)
         {
            Runway result = null;
            float best = float.MaxValue;
            foreach(Runway rwy in runways)
            {
               float d = Math.Abs(rwy.heading - bearing); // WRONG !!
               if( d < best )
               {
                  result = rwy;
                  best = d;
               }
            }
            return result;
         }

      }
   }

}
