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
            AddToTopBlock(set, Constants.WINDOW_ID_GAUGE_ORBIT);
            AddToTopBlock(set, Constants.WINDOW_ID_GAUGE_INCL);
            AddToTopBlock(set, Constants.WINDOW_ID_GAUGE_PEA);
            AddToTopBlock(set, Constants.WINDOW_ID_GAUGE_APA);
            AddToTopBlock(set, Constants.WINDOW_ID_GAUGE_TIMETOPEA);
            AddToTopBlock(set, Constants.WINDOW_ID_GAUGE_TIMETOAPA);

            AddToLeftBlock(set, Constants.WINDOW_ID_GAUGE_DTGT);
            AddToLeftBlock(set, Constants.WINDOW_ID_GAUGE_VTGT);
            AddToLeftBlock(set, Constants.WINDOW_ID_GAUGE_IMPACT);
            AddToLeftBlock(set, Constants.WINDOW_ID_GAUGE_TEMP);
            AddToLeftBlock(set, Constants.WINDOW_ID_GAUGE_GRAVIMETER);
            AddToLeftBlock(set, Constants.WINDOW_ID_GAUGE_SEISMOMETER);
            AddToLeftBlock(set, Constants.WINDOW_ID_GAUGE_HEAT);
            AddToLeftBlock(set, Constants.WINDOW_ID_GAUGE_BAROMETER);
            AddToLeftBlock(set, Constants.WINDOW_ID_GAUGE_EXTTEMP);
            AddToLeftBlock(set, Constants.WINDOW_ID_GAUGE_ATMTEMP);
            AddToLeftBlock(set, Constants.WINDOW_ID_GAUGE_DRILLTEMP);

            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_G);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_MAXG);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_VACCL);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_HACCL);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_ACCL);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_ATM);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_ISPE);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_DISP);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_THRUST);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_TWR);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_MASS);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_AOA);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_VAI);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_VVI);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_OSPD);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_HSPD);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_MACH);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_SPD);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_VSI);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_ALTIMETER);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_VT);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_RADAR_ALTIMETER);


            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_FUEL);
            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_OXID);
            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_FLOW);
            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_MONO);
            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_SRB);
            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_XENON);
            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_CHARGE);
            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_AMP);
            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_AIRIN);
            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_AIRPCT);
            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_PROPELLANT);
            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_EVAMP);


            AddToRightBlock(set, Constants.WINDOW_ID_GAUGE_ORE);
            AddToRightBlock(set, Constants.WINDOW_ID_GAUGE_O2);
            AddToRightBlock(set, Constants.WINDOW_ID_GAUGE_CARBONDIOXIDE);
            AddToRightBlock(set, Constants.WINDOW_ID_GAUGE_FOOD);
            AddToRightBlock(set, Constants.WINDOW_ID_GAUGE_H2O);
            AddToRightBlock(set, Constants.WINDOW_ID_GAUGE_WH2O);
            AddToRightBlock(set, Constants.WINDOW_ID_GAUGE_KARBONITE);
            AddToRightBlock(set, Constants.WINDOW_ID_GAUGE_WASTE);
            AddToRightBlock(set, Constants.WINDOW_ID_GAUGE_KETHANE);
            AddToRightBlock(set, Constants.WINDOW_ID_GAUGE_KAIRIN);
            AddToRightBlock(set, Constants.WINDOW_ID_GAUGE_SHIELD);

            // horizontal gauges
            LayoutHorizontalGauges(set);
         }


         public override void EnableGauges(GaugeSet set)
         {
            foreach (int id in set)
            {
               SetGaugeEnabled(set, id, true);
            }
            // we do not want to show those gauges
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_ABLAT, false);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_Q, false);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_ORE, false);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_CARBONDIOXIDE, false);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_FOOD, false);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_H2O, false);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_KARBONITE, false);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_WASTE, false);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_KETHANE, false);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_KAIRIN, false);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_SHIELD, false);

         }

      }
   }
}
