using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NanoGaugesAdapter;

namespace Nereid
{
   namespace NanoGauges
   {
      public class ClusterLayout : GaugeLayout
      {
         public ClusterLayout(Gauges gauges, Configuration configuration)
            : base(gauges, configuration)
         {
         }

         public override void DoLayout(GaugeSet set)
         {
            Reset();

            int LAYOUT_CELL_X = verticalGaugeWidth + Gauges.LAYOUT_GAP;
            int LAYOUT_CELL_Y = verticalGaugeHeight + Gauges.LAYOUT_GAP;
            int LAYOUT_RANGE_X = 3 * LAYOUT_CELL_X / 2;
            int LAYOUT_RANGE_Y = 3 * LAYOUT_CELL_Y / 2;


            int x0 = Screen.width - LAYOUT_CELL_X;
            int y0 = Screen.height - (int)(670 * gaugeScaling);
            int vDX = LAYOUT_CELL_X;
            int vDY = LAYOUT_CELL_Y;


            AddToTopBlock(set, Constants.WINDOW_ID_GAUGE_SETS);
            AddToTopBlock(set, Constants.WINDOW_ID_GAUGE_INDICATOR);
            AddToTopBlock(set, Constants.WINDOW_ID_GAUGE_CAM);

            int n = 0;
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_Q, x0 - 11 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_TEMP, x0 - 10 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_GRAVIMETER, x0 - 9 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_BAROMETER, x0 - 8 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_SEISMOMETER, x0 - 7 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_SHIELD, x0 - 6 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_IAS, x0 - 5 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_GLIDE, x0 - 4 * vDX, y0 + n * vDY);
            n = 1;
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_KARBONITE, x0 - 11 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_O2, x0 - 10 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_CO2, x0 - 9 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_CARBONDIOXIDE, x0 - 8 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_H2O, x0 - 7 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_WH2O, x0 - 6 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_WASTE, x0 - 5 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_FOOD, x0 - 4 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_ORE, x0 - 3 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_KETHANE, x0 - 2 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_KAIRIN, x0 - 1 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_CRAP, x0 - 0 * vDX, y0 + n * vDY);
            //
            //
            n = 2;
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_ORBIT, x0 - 11 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_INCL, x0 - 10 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_TIMETOAPA, x0 - 9 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_TIMETOPEA, x0 - 8 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_PEA, x0 - 7 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_APA, x0 - 6 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_IMPACT, x0 - 5 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_ABLAT, x0 - 4 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_HEAT, x0 - 3 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_EXTTEMP, x0 - 2 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_ATMTEMP, x0 - 1 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_DRILLTEMP, x0 - 0 * vDX, y0 + n * vDY);

            n = 3;
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_THRUST, x0 - 11 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_TWR, x0 - 10 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_ISPE, x0 - 9 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_DISP, x0 - 8 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_MASS, x0 - 7 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_AOA, x0 - 6 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_VVI, x0 - 5 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_VAI, x0 - 4 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_DTGT, x0 - 3 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_VTGT, x0 - 2 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_G, x0 - 1 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_MAXG, x0 - 0 * vDX, y0 + n * vDY);


            n = 4;
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_FUEL, x0 - 11 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_OXID, x0 - 10 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_FLOW, x0 - 9 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_SRB, x0 - 8 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_MONO, x0 - 7 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_CHARGE, x0 - 6 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_AMP, x0 - 5 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_XENON, x0 - 4 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_AIRIN, x0 - 3 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_AIRPCT, x0 - 2 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_PROPELLANT, x0 - 1 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_EVAMP, x0 - 0 * vDX, y0 + n * vDY);

            n = 5;
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_SPD, x0 - 11 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_VSI, x0 - 10 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_HSPD, x0 - 9 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_OSPD, x0 - 8 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_MACH, x0 - 7 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_ACCL, x0 - 6 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_HACCL, x0 - 5 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_VACCL, x0 - 4 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_VT, x0 - 3 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_ALTIMETER, x0 - 2 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_RADAR_ALTIMETER, x0 - 1 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_ATM, x0 - 0 * vDX, y0 + n * vDY);

            // horizontal gauges
            LayoutHorizontalGauges(set);
         }

         public override void EnableGauges(GaugeSet set)
         {
            EnableAllgauges(set);
         }
      }
   }
}
