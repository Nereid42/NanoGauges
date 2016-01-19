using UnityEngine;
using System;
using System.Collections.Generic;

namespace Nereid
{
   namespace NanoGauges
   {
      public class DefaultLayout : GaugeLayout
      {
         private readonly GaugeLayout standard;
         private readonly GaugeLayout launch;
         private readonly GaugeLayout flight;
         private readonly GaugeLayout landing;

         public DefaultLayout(Gauges gauges, Configuration configuration)
            : base(gauges, configuration)
         {
            standard = new StandardLayout(gauges,configuration);
            launch = new LaunchLayout(gauges,configuration);
            flight = new FlightLayout(gauges, configuration);
            landing = new LandingLayout(gauges, configuration);
         }

         protected override void DoLayout(GaugeSet set)
         {
            Log.Info("DoLayout for "+set);
            GaugeSet.ID id = set.GetId();
            switch(id)
            {
               case GaugeSet.ID.STANDARD:
                  standard.Reset(set);
                  return;
               case GaugeSet.ID.LAUNCH:
                  launch.Reset(set);
                  return;
               case GaugeSet.ID.FLIGHT:
                  flight.Reset(set);
                  return;
               case GaugeSet.ID.LAND:
                  landing.Reset(set);
                  return;
               case GaugeSet.ID.DOCK:
               case GaugeSet.ID.ORBIT:
               case GaugeSet.ID.SET1:
               case GaugeSet.ID.SET2:
               case GaugeSet.ID.SET3:
               default:
                  Log.Warning("unknown gauge sewt ID for layout: "+id);
                  break;
            }
         }



         public override void Enable(GaugeSet set)
         {
            GaugeSet.ID id = set.GetId();
            switch (id)
            {
               case GaugeSet.ID.STANDARD:
                  standard.Enable(set);
                  return;
               case GaugeSet.ID.DOCK:
                  launch.Enable(set);
                  return;
               case GaugeSet.ID.FLIGHT:
                  break;
               case GaugeSet.ID.ORBIT:
                  break;
               case GaugeSet.ID.LAND:
                  break;
               case GaugeSet.ID.LAUNCH:
                  break;
               case GaugeSet.ID.SET1:
                  break;
               case GaugeSet.ID.SET2:
                  break;
               case GaugeSet.ID.SET3:
                  break;
               default:
                  Log.Warning("unknown gauge sewt ID for layout: " + id);
                  break;
            }

         }

         public override string ToString()
         {
            return "default layout";
         }

      }
   }
}
