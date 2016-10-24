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
         public readonly float heading;

         public Runway(String name, Coords coords, float heading)
         {
            this.name = name;
            this.coords = coords;
            this.heading = heading;
         }

         public override string ToString()
         {
            return "Runway "+name+ " at "+coords+" ["+heading.ToString("000")+"]";
         }

      }
   }

}
