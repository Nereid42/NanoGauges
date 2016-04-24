using UnityEngine;
using System;
using System.Collections.Generic;

namespace Nereid
{
   namespace NanoGauges
   {
      public class DockingLayout : GaugeLayout
      {

         public DockingLayout(Gauges gauges, Configuration configuration)
            : base(gauges, configuration)
         {

         }


         public override void DoLayout(GaugeSet set)
         {
            Reset();

            AddToTopBlock(set, Constants.WINDOW_ID_GAUGE_SETS);
            AddToTopBlock(set, Constants.WINDOW_ID_GAUGE_INDICATOR);
            AddToTopBlock(set, Constants.WINDOW_ID_GAUGE_CAM);

            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_ORBIT);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_INCL);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_DTGT);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_VTGT);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_TIMETOPEA);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_TIMETOAPA);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_PEA);
            AddToLeftNavballBlock(set, Constants.WINDOW_ID_GAUGE_APA);


            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_FUEL);
            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_OXID);
            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_MONO);
            AddToRightNavballBlock(set, Constants.WINDOW_ID_GAUGE_AMP);

            // horizontal gauges
            int hDY = (int)(configuration.horizontalGaugeHeight * gaugeScaling) + Gauges.LAYOUT_GAP;
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_BIOME, MARGIN_X_TOP_LEFT_BLOCK, MARGIN_Y_TOP_LEFT_BLOCK + 0 * hDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_LATITUDE, MARGIN_X_TOP_LEFT_BLOCK, MARGIN_Y_TOP_LEFT_BLOCK + 1 * hDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_LONGITUDE, MARGIN_X_TOP_LEFT_BLOCK, MARGIN_Y_TOP_LEFT_BLOCK + 2 * hDY);
         }


         public override void EnableGauges(GaugeSet set)
         {
            foreach (int id in set)
            {
               SetGaugeEnabled(set, id, false);
            }
            //
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_BIOME, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_LATITUDE, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_LONGITUDE, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_SETS, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_INDICATOR, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_CAM, true);
            //
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_ORBIT, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_INCL, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_DTGT, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_VTGT, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_TIMETOPEA, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_TIMETOAPA, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_APA, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_PEA, true);
            //
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_FUEL, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_OXID, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_MONO, true);
            SetGaugeEnabled(set, Constants.WINDOW_ID_GAUGE_AMP, true);
         }

      }
   }
}
