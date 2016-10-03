using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Nereid
{
   namespace NanoGauges
   {
      public static class Extensions
      {
         public static double MaxAtmosphereAltitude(this CelestialBody body)
         {
            return body.atmosphereDepth;
         }

         public static double RadarAltitude(this Vessel vessel)
         {
            // just to be sure...
            if (vessel.mainBody == null) return 0.0;
            //
            if(vessel.mainBody.ocean)
            {
               //  ocean, return altitude over terrain if terrain is above sea level, altitude above sea level otherwise
               return Math.Min(vessel.altitude - vessel.terrainAltitude, vessel.altitude);
            }
            else
            {
               // no ocean, return altitude over terrain
               return vessel.altitude - vessel.terrainAltitude;
            }
         }

         public static bool IsDrill(this Part part)
         {
            List<BaseDrill> drills = part.FindModulesImplementing<BaseDrill>();
            if(drills==null) return false;
            return drills.Count > 0;
         }

         public static bool In(this int x, int a, int b) 
         {
            if (x >= a && x <= b) return true;
            return false;
         }

         public static String Limit(this String s, int maxlength)
         {
            if (s.Length <= maxlength) return s;
            return s.Substring(0, maxlength);
         }
      }
   }
}
