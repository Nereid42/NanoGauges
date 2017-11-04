using UnityEngine;
using System;
using System.Collections.Generic;

namespace Nereid
{
   namespace NanoGauges
   {
      public class FlightLayout : GaugeLayout
      {

         public FlightLayout(Gauges gauges, Configuration configuration)
            : base(gauges, configuration)
         {

         }


         public override void DoLayout(GaugeSet set)
         {
            Reset();

            AddToTopBlock(set, Constants.WINDOW_ID_GAUGE_SETS);
            AddToTopBlock(set, Constants.WINDOW_ID_GAUGE_INDICATOR);
            AddToTopBlock(set, Constants.WINDOW_ID_GAUGE_CAM);

            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_G);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_MAXG);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_ACCL);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_VACCL);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_HACCL);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_ATM);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_TWR);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_THRUST);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_ISPE);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_AOA);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_VAI);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_VVI);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_MACH);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_IAS);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_SPD);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_VSI);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_ALTIMETER);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_RADAR_ALTIMETER);


            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_FUEL);
            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_FLOW);
            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_AIRIN);
            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_AIRPCT);
            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_PROPELLANT);
            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_Q);

            // horizontal gauges
            LayoutHorizontalGauges(set);

         }


         public override void EnableGauges(GaugeSet set)
         {
            DisableAllgauges(set);
            //
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_SETS, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_INDICATOR, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_CAM, true);
            //
            EnableAllHorizontalGauges(set);
            //
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_MAXG, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_G, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_VACCL, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_HACCL, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_ACCL, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_ATM, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_ISPE, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_IAS, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_TWR, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_THRUST, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_AOA, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_VAI, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_VVI, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_MACH, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_SPD, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_VSI, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_ALTIMETER, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_RADAR_ALTIMETER, true);
            //
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_FUEL, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_FLOW, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_AIRIN, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_AIRPCT, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_PROPELLANT, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_Q, true);
         }

      }
   }
}
