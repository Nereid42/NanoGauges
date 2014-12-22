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
         public static double GetThrust(this Vessel vessel)
         {
            return GameUtils.GetThrust(vessel);
         }


         public static bool In(this int x, int a, int b) 
         {
            if (x >= a && x <= b) return true;
            return false;
         }
      }
   }
}
