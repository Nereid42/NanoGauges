using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nereid
{
   namespace NanoGauges
   {
      public static class NavGlobals
      {
         public static readonly Runway RUNWAY_090_SPACECENTER = new Runway("RWY 090 Space Center", Coords.Create(-74.7130, -0.0486), 65.75f, 90.0f);
         public static readonly Runway RUNWAY_270_SPACECENTER = new Runway("RWY 270 Space Center", Coords.Create(-74.5039, -0.0501), 65.75f, 270.0f);

         public static void Update()
         {
         }
      }
   }

}
