using UnityEngine;
using System;
using System.Collections.Generic;

namespace Nereid
{
   namespace NanoGauges
   {
      public class LandingLayout : GaugeLayout
      {

         public LandingLayout(Gauges gauges, Configuration configuration)
            : base(gauges, configuration)
         {

         }


         public override void DoLayout(GaugeSet set)
         {
            Reset();

            AddToTopBlock(set, Constants.WINDOW_ID_GAUGE_SETS);
            AddToTopBlock(set, Constants.WINDOW_ID_GAUGE_INDICATOR);
            AddToTopBlock(set, Constants.WINDOW_ID_GAUGE_CAM);
            AddToTopBlock(set, Constants.WINDOW_ID_GAUGE_IMPACT);

           AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_G);
           AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_MAXG);
           AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_VACCL);
           AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_HACCL);
           AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_ACCL);
           AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_ATM);
           AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_TWR);
           AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_THRUST);
           AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_AOA);
           AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_VAI);
           AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_VVI);
           AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_SPD);
           AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_VSI);
           AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_HSPD);
           AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_ALTIMETER);
           AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_RADAR_ALTIMETER);


           AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_FUEL);
           AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_OXID);
           AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_FLOW);
           AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_AIRIN);
           AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_AIRPCT);
           AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_PROPELLANT);

           // horizontal gauges
           int hDY = (int)(configuration.horizontalGaugeHeight * gaugeScaling) + Gauges.LAYOUT_GAP;
           set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_BIOME, 10, 60 + 0 * hDY);
           set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_LATITUDE, 10, 60 + 1 * hDY);
           set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_LONGITUDE, 10, 60 + 2 * hDY);

         }


         public override void EnableGauges(GaugeSet set)
         {
            foreach (int id in set)
            {
               set.SetGaugeEnabled(id, false);
            }
            //
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_SETS, true);
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_INDICATOR, true);
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_CAM, true);
            //
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_BIOME, true);
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_LATITUDE, true);
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_LONGITUDE, true);
            //
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_MAXG, true);
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_G, true);
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_VACCL, true);
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_HACCL, true);
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_ACCL, true);
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_ATM, true);
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_TWR, true);
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_THRUST, true);
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_AOA, true);
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_VAI, true);
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_VVI, true);
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_SPD, true);
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_VSI, true);
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_HSPD, true);
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_ALTIMETER, true);
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_RADAR_ALTIMETER, true);
            //
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_FUEL, true);
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_FLOW, true);
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_OXID, true);
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_AIRIN, true);
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_AIRPCT, true);
            set.SetGaugeEnabled(Constants.WINDOW_ID_GAUGE_PROPELLANT, true);
         }

      }
   }
}
