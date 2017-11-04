using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nereid
{
   namespace NanoGauges
   {
      public class Airfield
      {
         // generates ids
         private static int sequence = 1;

         public readonly int id;
         public readonly String name;
         public readonly Runway[] runways;
         public readonly Coords coords;

         public Airfield(String name, params Runway[] runways)
         {
            this.id = sequence++;
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
            if (runways.Length == 0) return new Coords(0, 0);
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

         public override bool Equals(object obj)
         {
            Airfield airfield = obj as Airfield;
            if (airfield == null) return false;
            return id == airfield.id;
         }

         public override int GetHashCode()
         {
            return id;
         }
      }
   }

}
