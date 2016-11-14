using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nereid
{
   namespace NanoGauges
   {
      public class Coords 
      {
         public double longitude { get; private set; }
         public double latitude { get; private set; }

         public static Coords Create(double longitude, double latitude)
         {
            return new Coords(longitude, latitude);
         }

         public Coords(double longitude, double latitude)
         {
            this.longitude = longitude;
            this.latitude = latitude;
         }

         public override String ToString()
         {
            return "(" + longitude.ToString("0.00") + "," + latitude.ToString("0.00") + ")";
         }

         public override bool Equals(System.Object right)
         {
            if (right == null) return false;
            Coords cmp = right as Coords;
            if (cmp == null) return false;
            return longitude.Equals(cmp.longitude) && latitude.Equals(cmp.latitude);
         }


         public override int GetHashCode()
         {
            return longitude.GetHashCode() + 5011 * longitude.GetHashCode(); // 5011 is prime
         }
      };
   }
}