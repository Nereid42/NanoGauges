using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nereid
{
   namespace NanoGauges
   {
      public static class GameUtils
      {

         public static double GetThrust(Vessel vessel)
         {
            if(vessel==null) return 0.0;
            double result = 0.0;
            foreach(Part part in vessel.Parts)
            {
               foreach (ModuleEnginesFX engine in part.Modules.OfType<ModuleEnginesFX>())
               {
                  result += engine.CalculateThrust();
               }
               foreach (ModuleEngines engine in part.Modules.OfType<ModuleEngines>())
               {
                  result += engine.CalculateThrust();
               }
            }
            return result;
         }
      }
   }
}