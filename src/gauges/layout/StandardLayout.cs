using UnityEngine;
using System;
using System.Collections.Generic;

namespace Nereid
{
   namespace NanoGauges
   {
      public class StandardLayout : GaugeLayout
      {

         public StandardLayout(Configuration configuration)
            : base(configuration)
         {

         }

         public override void Reset(GaugeSet set)
         {
            GaugeSet.ID id = set.GetId();
            switch(id)
            {
               case GaugeSet.ID.STANDARD:
                  ResetStandardLayout(set);
                  return;
               case GaugeSet.ID.DOCK:
                  break;
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
                  Log.Warning("unknown gauge sewt ID for layout: "+id);
                  break;
            }
         }

         private void ResetStandardLayout(GaugeSet set)
         {
            Log.Info("reset of standard layout for set "+set);
            int LAYOUT_CELL_X = verticalGaugeWidth + Gauges.LAYOUT_GAP;
            int LAYOUT_CELL_Y = verticalGaugeHeight + Gauges.LAYOUT_GAP;

            int i = 0;

            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_SETS, TopBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_INDICATOR, TopBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_CAM, TopBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_ORBIT, TopBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_INCL, TopBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_PEA, TopBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_APA, TopBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_TIMETOPEA, TopBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_TIMETOAPA, TopBlock(i++));

            i = 0;
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_DTGT, LeftBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_VTGT, LeftBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_IMPACT, LeftBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_TEMP, LeftBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_GRAV, LeftBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_HEAT, LeftBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_EXTTEMP, LeftBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_ATMTEMP, LeftBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_DRILLTEMP, LeftBlock(i++));

            i = 0;
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_G, LeftNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_MAXG, LeftNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_VACCL, LeftNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_HACCL, LeftNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_ACCL, LeftNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_ATM, LeftNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_ISPE, LeftNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_DISP, LeftNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_TWR, LeftNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_THRUST, LeftNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_MASS, LeftNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_AOA, LeftNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_VAI, LeftNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_VVI, LeftNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_OSPD, LeftNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_HSPD, LeftNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_MACH, LeftNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_SPD, LeftNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_VSI, LeftNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_ALTIMETER, LeftNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_VT, LeftNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_RADAR_ALTIMETER, LeftNavballBlock(i++));


            i = 0;
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_FUEL, RightNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_OXID, RightNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_FLOW, RightNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_MONO, RightNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_SRB, RightNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_XENON, RightNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_CHARGE, RightNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_AMP, RightNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_AIRIN, RightNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_AIRPCT, RightNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_PROPELLANT, RightNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_EVAMP, RightNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_ABLAT, RightNavballBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_Q, RightNavballBlock(i++));


            i = 0;
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_ORE, RightBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_O2, RightBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_CO2, RightBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_FOOD, RightBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_H2O, RightBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_WH2O, RightBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_KARBONITE, RightBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_WASTE, RightBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_KETHANE, RightBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_KAIRIN, RightBlock(i++));
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_SHIELD, RightBlock(i++));



            // horizontal gauges
            int hDY = (int)(verticalGaugeWidth * gaugeScaling) + Gauges.LAYOUT_GAP;
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_BIOME, 10, 60 + 0 * hDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_LATITUDE, 10, 60 + 1 * hDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_LONGITUDE, 10, 60 + 2 * hDY);
         }

         public override void Enable(GaugeSet set)
         {
            foreach (int id in set)
            {
               set.SetGaugeEnabled(id, true);
            }
         }

         public override string ToString()
         {
            return "standard layout";
         }

      }
   }
}
