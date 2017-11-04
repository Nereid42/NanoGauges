using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nereid
{
   namespace NanoGauges
   {
      public class Runway
      {
         public readonly String name;
         public readonly Coords coords;
         public readonly double elevation;
         public readonly double heading;
         public readonly double glideslope;
         public readonly double slopeTangens;
         public readonly double From;
         public readonly double To;
         public readonly bool HasILS;
         public readonly bool HasDME;
         public readonly double ILSCone; // angle of ILS from center glideslope in degrees
         public readonly double ILSRange; // range of ILS in meter

         public Runway( String name, Coords coords, float elevation, float heading, float glideslope = 3.0f)
         {
            this.name = name;
            this.coords = coords;
            this.elevation = elevation;
            this.heading = heading;
            this.glideslope = glideslope;
            this.slopeTangens = (float)Math.Tan(Utils.DegreeToRadians(glideslope));
            this.To = heading;
            this.From = NavUtils.InverseHeading(To);
            //
            // currently constant
            this.HasILS = true;
            this.HasDME = true;
            this.ILSCone = 10.0;
            this.ILSRange = 80000.0;
         }

         public override string ToString()
         {
            return "Runway "+name+ " at "+coords+" "+elevation+"m ["+heading.ToString("000")+"]\\"+glideslope+"°";
         }

      }
   }

}
