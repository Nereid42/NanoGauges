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

         public Runway(String name, Coords coords, float elevation, float heading, float glideslope = 3.0f)
         {
            this.name = name;
            this.coords = coords;
            this.elevation = elevation;
            this.heading = heading;
            this.glideslope = glideslope;
            this.slopeTangens = (float)Math.Tan(Utils.DegreeToRadians(glideslope));
            this.To = heading;
            this.From = NavUtils.InverseHeading(To);
         }

         public override string ToString()
         {
            return "Runway "+name+ " at "+coords+" "+elevation+"m ["+heading.ToString("000")+"]\\"+glideslope+"°";
         }

      }
   }

}
