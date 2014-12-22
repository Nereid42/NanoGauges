using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nereid
{
   namespace NanoGauges
   {
      public class Pair<T, U>
      {
         public T first { get; set; }
         public U second { get; set; }

         public Pair()
         {
         }

         public Pair(T first, U second)
         {
            this.first = first;
            this.second = second;
         }

         public override String ToString()
         {
            return "(" + first.ToString() + "," + second.ToString() + ")";
         }

         public override bool Equals(System.Object right)
         {
            if (right == null) return false;
            Pair<T, U> cmp = right as Pair<T, U>;
            if (cmp == null) return false;
            return first.Equals(cmp.first) && second.Equals(cmp.second);
         }


         public override int GetHashCode()
         {
            return first.GetHashCode() + 7 * second.GetHashCode();
         }
      };
   }
}