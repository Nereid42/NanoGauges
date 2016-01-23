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
         private readonly GaugeLayout docking;
         private readonly GaugeLayout orbiting;
         private readonly GaugeLayout set1;
         private readonly GaugeLayout set2;
         private readonly GaugeLayout set3;

         public DefaultLayout(Gauges gauges, Configuration configuration)
            : base(gauges, configuration)
         {
            standard = new StandardLayout(gauges,configuration);
            launch = new LaunchLayout(gauges,configuration);
            flight = new FlightLayout(gauges, configuration);
            landing = new LandingLayout(gauges, configuration);
            docking= new DockingLayout(gauges, configuration);
            orbiting = new OrbitingLayout(gauges, configuration);
            set1 = new ClusterLayout(gauges, configuration);
            set2 = new ClusterLayout(gauges, configuration);
            set3 = new ClusterLayout(gauges, configuration);
         }

         public override void DoLayout(GaugeSet set)
         {
            GaugeSet.ID id = set.GetId();
            switch(id)
            {
               case GaugeSet.ID.STANDARD:
                  standard.DoLayout(set);
                  return;
               case GaugeSet.ID.LAUNCH:
                  launch.DoLayout(set);
                  return;
               case GaugeSet.ID.FLIGHT:
                  flight.DoLayout(set);
                  return;
               case GaugeSet.ID.LAND:
                  landing.DoLayout(set);
                  return;
               case GaugeSet.ID.DOCK:
                  docking.DoLayout(set);
                  return;
               case GaugeSet.ID.ORBIT:
                  orbiting.DoLayout(set);
                  return;
               case GaugeSet.ID.SET1:
                  set1.DoLayout(set);
                  return;
               case GaugeSet.ID.SET2:
                  set2.DoLayout(set);
                  return;
               case GaugeSet.ID.SET3:
                  set3.DoLayout(set);
                  return;
               default:
                  Log.Warning("unknown gauge sewt ID for layout: "+id);
                  break;
            }
         }



         public override void EnableGauges(GaugeSet set)
         {
            GaugeSet.ID id = set.GetId();
            switch (id)
            {
               case GaugeSet.ID.STANDARD:
                  standard.EnableGauges(set);
                  return;
               case GaugeSet.ID.LAUNCH:
                  launch.EnableGauges(set);
                  return;
               case GaugeSet.ID.FLIGHT:
                  flight.EnableGauges(set);
                  return;
               case GaugeSet.ID.LAND:
                  landing.EnableGauges(set);
                  return;
               case GaugeSet.ID.DOCK:
                  docking.EnableGauges(set);
                  return;
               case GaugeSet.ID.ORBIT:
                  orbiting.EnableGauges(set);
                  return;
               case GaugeSet.ID.SET1:
                  set1.EnableGauges(set);
                  return;
               case GaugeSet.ID.SET2:
                  set2.EnableGauges(set);
                  return;
               case GaugeSet.ID.SET3:
                  set3.EnableGauges(set);
                  return;
               default:
                  Log.Warning("unknown gauge sewt ID for layout: " + id);
                  break;
            }

         }

      }
   }
}
