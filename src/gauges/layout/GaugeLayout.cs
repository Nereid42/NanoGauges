using UnityEngine;
using System;
using System.Collections.Generic;



namespace Nereid
{
   namespace NanoGauges
   {
      public abstract class GaugeLayout
      {
         protected const int COLUMNS_TOP_BLOCK = 10;
         protected const int COLUMNS_LEFT_BLOCK = 3;
         protected const int COLUMNS_RIGHT_BLOCK = 3;
         protected const int ROWS_LEFT_NAVBALL_BLOCK = 2;
         protected const int ROWS_RIGHT_NAVBALL_BLOCK = 2;

         protected const int MARGIN_Y_TOP_BLOCK = 10;
         protected const int MARGIN_X_TOP_BLOCK = 160;

         protected const int MARGIN_Y_LEFT_BLOCK = 210;
         protected const int MARGIN_X_LEFT_BLOCK = 60;
         protected const int MARGIN_Y_RIGHT_BLOCK = 150;
         protected const int MARGIN_X_RIGHT_BLOCK = 10;

         protected const int MARGIN_Y_LEFT_NAVBALL_BLOCK = 10;
         protected const int MARGIN_X_LEFT_NAVBALL_BLOCK = 110;
         protected const int MARGIN_Y_RIGHT_NAVBALL_BLOCK = MARGIN_Y_LEFT_NAVBALL_BLOCK;
         protected const int MARGIN_X_RIGHT_NAVBALL_BLOCK = 160;


         protected Configuration configuration { get; private set; }
         protected int verticalGaugeWidth { get; private set; }
         protected int verticalGaugeHeight { get; private set; }
         protected double gaugeScaling { get; private set; }
         protected int horizontalGaugeWidth { get; private set; }
         protected readonly int centerX = Screen.width / 2;
         protected readonly int centerY = Screen.height / 2;

         protected GaugeLayout(Configuration configuration)
         {
            this.configuration = configuration;
            this.verticalGaugeWidth = configuration.verticalGaugeWidth;
            this.verticalGaugeHeight = configuration.verticalGaugeHeight;
            this.gaugeScaling = configuration.gaugeScaling;
            this.horizontalGaugeWidth = configuration.horizontalGaugeWidth;
         }

         private int XLeftBlock(int index)
         {
            return MARGIN_X_LEFT_BLOCK + (verticalGaugeWidth + Gauges.LAYOUT_GAP) * (index % COLUMNS_LEFT_BLOCK);
         }

         private int YLeftBlock(int index)
         {
            return MARGIN_Y_LEFT_BLOCK + (verticalGaugeHeight + Gauges.LAYOUT_GAP) * (index / COLUMNS_LEFT_BLOCK);
         }

         protected Pair<int, int> LeftBlock(int index)
         {
            return new Pair<int, int>(XLeftBlock(index), YLeftBlock(index));
         }

         private int XRightBlock(int index)
         {
            return (Screen.width - (verticalGaugeWidth + Gauges.LAYOUT_GAP) * COLUMNS_RIGHT_BLOCK - MARGIN_X_RIGHT_BLOCK) + (verticalGaugeWidth + Gauges.LAYOUT_GAP) * (index % COLUMNS_RIGHT_BLOCK);
         }

         private int YRightBlock(int index)
         {
            return MARGIN_Y_RIGHT_BLOCK + (verticalGaugeHeight + Gauges.LAYOUT_GAP) * (index / COLUMNS_RIGHT_BLOCK);
         }

         protected Pair<int, int> RightBlock(int index)
         {
            return new Pair<int, int>(XRightBlock(index), YRightBlock(index));
         }

         private int XTopBlock(int index)
         {
            return MARGIN_X_TOP_BLOCK + (verticalGaugeWidth + Gauges.LAYOUT_GAP) * (index % COLUMNS_TOP_BLOCK);
         }

         private int YTopBlock(int index)
         {
            return MARGIN_Y_TOP_BLOCK + (verticalGaugeHeight + Gauges.LAYOUT_GAP) * (index / COLUMNS_TOP_BLOCK);
         }

         protected Pair<int, int> TopBlock(int index)
         {
            return new Pair<int, int>(XTopBlock(index), YTopBlock(index));
         }

         private int XLeftNavballBlock(int index)
         {
            return centerX - MARGIN_X_LEFT_NAVBALL_BLOCK - (verticalGaugeWidth + Gauges.LAYOUT_GAP) * (index / ROWS_LEFT_NAVBALL_BLOCK + 1);
         }

         private int YLeftNavballBlock(int index)
         {
            return Screen.height - MARGIN_Y_LEFT_NAVBALL_BLOCK - (verticalGaugeHeight + Gauges.LAYOUT_GAP) * (index % ROWS_LEFT_NAVBALL_BLOCK + 1);
         }

         protected Pair<int, int> LeftNavballBlock(int index)
         {
            return new Pair<int, int>(XLeftNavballBlock(index), YLeftNavballBlock(index));
         }

         private int XRightNavballBlock(int index)
         {
            return centerX + MARGIN_X_RIGHT_NAVBALL_BLOCK + (verticalGaugeWidth + Gauges.LAYOUT_GAP) * (index / ROWS_LEFT_NAVBALL_BLOCK);
         }

         private int YRightNavballBlock(int index)
         {
            return Screen.height - MARGIN_Y_LEFT_NAVBALL_BLOCK - (verticalGaugeHeight + Gauges.LAYOUT_GAP) * (index % ROWS_LEFT_NAVBALL_BLOCK + 1);
         }

         protected Pair<int, int> RightNavballBlock(int index)
         {
            return new Pair<int, int>(XRightNavballBlock(index), YRightNavballBlock(index));
         }

         public abstract void Reset(GaugeSet set);

         public abstract void Enable(GaugeSet set);
      }


   }
}
