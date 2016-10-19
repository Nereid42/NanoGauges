using UnityEngine;
using System;
using System.Collections.Generic;

namespace Nereid
{
   namespace NanoGauges
   {
      public class StandardLayout : GaugeLayout
      {

         public StandardLayout(Gauges gauges, Configuration configuration)
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
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_MASS);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_THRUST);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_TWR);

            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_HSPD);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_MACH);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_SPD);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_VSI);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_ALTIMETER);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_RADAR_ALTIMETER);


            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_FUEL);
            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_OXID);
            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_FLOW);
            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_MONO);
            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_CHARGE);
            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_AMP);
            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_PROPELLANT);
            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_EVAMP);


            // horizontal gauges
            LayoutHorizontalGauges(set);
         }


         public override void EnableGauges(GaugeSet set)
         {
            DisableAllgauges(set);
            //
            EnableAllHorizontalTextGauges(set);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_SETS, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_INDICATOR, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_CAM, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_G, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_MASS, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_THRUST, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_TWR, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_HSPD, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_MACH, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_SPD, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_VSI, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_ALTIMETER, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_RADAR_ALTIMETER, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_FUEL, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_OXID, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_FLOW, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_MONO, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_CHARGE, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_AMP, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_PROPELLANT, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_EVAMP, true);
         }

      }
   }
}
