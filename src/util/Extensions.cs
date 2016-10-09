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

         // STANDARD=0, LAUNCH=1, LAND=2, DOCK=3, ORBIT=4, FLIGHT=5, SET1=101, SET2=102, SET3=103
         public static GaugeSet.ID increment( this GaugeSet.ID id )
         {
            switch(id)
            {
               case GaugeSet.ID.STANDARD: return GaugeSet.ID.LAUNCH;
               case GaugeSet.ID.LAUNCH: return GaugeSet.ID.LAND;
               case GaugeSet.ID.LAND: return GaugeSet.ID.DOCK;
               case GaugeSet.ID.DOCK: return GaugeSet.ID.ORBIT;
               case GaugeSet.ID.ORBIT: return GaugeSet.ID.FLIGHT;
               case GaugeSet.ID.FLIGHT: return GaugeSet.ID.SET1;
               case GaugeSet.ID.SET1: return GaugeSet.ID.SET2;
               case GaugeSet.ID.SET2: return GaugeSet.ID.SET3;
               case GaugeSet.ID.SET3: return GaugeSet.ID.STANDARD;
               default: return GaugeSet.ID.CLIPBOARD;
            }            
         }

         public static GaugeSet.ID decrement(this GaugeSet.ID id)
         {
            switch (id)
            {
               case GaugeSet.ID.STANDARD: return GaugeSet.ID.SET3;
               case GaugeSet.ID.LAUNCH: return GaugeSet.ID.STANDARD;
               case GaugeSet.ID.LAND: return GaugeSet.ID.LAUNCH;
               case GaugeSet.ID.DOCK: return GaugeSet.ID.LAND;
               case GaugeSet.ID.ORBIT: return GaugeSet.ID.DOCK;
               case GaugeSet.ID.FLIGHT: return GaugeSet.ID.ORBIT;
               case GaugeSet.ID.SET1: return GaugeSet.ID.FLIGHT;
               case GaugeSet.ID.SET2: return GaugeSet.ID.SET1;
               case GaugeSet.ID.SET3: return GaugeSet.ID.SET2;
               default: return GaugeSet.ID.CLIPBOARD;
            }
         }
      }
   }
}
