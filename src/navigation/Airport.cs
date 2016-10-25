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
         public readonly Coords coords;

         public Airport(String name, params Runway[] runways)
         {
            this.name = name;
            this.runways = runways;
            this.coords = CenterOfRunways(runways);
         }

         public Runway GetLandingRunwayForBearing(double bearing)
         {
            Runway result = null;
            double best = float.MaxValue;
            foreach(Runway rwy in runways)
            {
               double d = Math.Abs(rwy.heading - bearing); // WRONG !!
               if( d < best )
               {
                  result = rwy;
                  best = d;
               }
            }
            return result;
         }

         private Coords CenterOfRunways(Runway[] runways)
         {
            double lonCenter = 0;
            double latCenter = 0;
            foreach(Runway rwy in runways)
            {
               lonCenter += rwy.coords.longitude;
               latCenter += rwy.coords.latitude;
            }
            return new Coords(lonCenter / runways.Length, latCenter / runways.Length);
         }

         public override string ToString()
         {
            return "Airport " + name+ " at "+coords;
         }

      }
   }

}
