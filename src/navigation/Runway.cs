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
         public readonly float elevation;
         public readonly float heading;
         public readonly float glideslope;
         public readonly float slopeTangens;

         public Runway(String name, Coords coords, float elevation, float heading, float glideslope = 3.0f)
         {
            this.name = name;
            this.coords = coords;
            this.elevation = elevation;
            this.heading = heading;
            this.glideslope = glideslope;
            this.slopeTangens = (float)Math.Tan(Utils.DegreeToRadians(glideslope));
         }

         public override string ToString()
         {
            return "Runway "+name+ " at "+coords+" "+elevation+"m ["+heading.ToString("000")+"]\\"+glideslope+"°";
         }

      }
   }

}
