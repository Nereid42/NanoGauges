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
         protected const int MARGIN_X_TOP_BLOCK = 210;

         protected const int MARGIN_X_TOP_LEFT_BLOCK = 10;
         protected const int MARGIN_Y_TOP_LEFT_BLOCK = 90;


         protected const int MARGIN_X_LEFT_BLOCK = 60;
         protected const int MARGIN_Y_LEFT_BLOCK = 250;
         protected const int MARGIN_X_RIGHT_BLOCK = 50;
         protected const int MARGIN_Y_RIGHT_BLOCK = 150;

         protected const int MARGIN_Y_LEFT_NAVBALL_BLOCK = 12;
         protected const int MARGIN_X_LEFT_NAVBALL_BLOCK = 200;
         protected const int MARGIN_Y_RIGHT_NAVBALL_BLOCK = MARGIN_Y_LEFT_NAVBALL_BLOCK;
         protected const int MARGIN_X_RIGHT_NAVBALL_BLOCK = 220;


         protected Gauges gauges { get; private set; }
         protected Configuration configuration { get; private set; }
         protected int verticalGaugeWidth { get; private set; }
         protected int verticalGaugeHeight { get; private set; }
         protected double gaugeScaling { get; private set; }
         protected int horizontalGaugeWidth { get; private set; }
         protected int horizontalGaugeHeight { get; private set; }
         protected readonly int centerX = Screen.width / 2;
         protected readonly int centerY = Screen.height / 2;

         // gauges counter/indexes
         int topBlockIndex = 0;
         int leftBlockIndex = 0;
         int rightBlockIndex = 0;
         int leftNavBlockIndex = 0;
         int rightNavBlockIndex = 0;
         int spareIndex = 0;
         int spareRow = 0;

         protected GaugeLayout(Gauges gauges, Configuration configuration)
         {
            this.gauges = gauges;
            this.configuration = configuration;
            this.verticalGaugeWidth = configuration.verticalGaugeWidth;
            this.verticalGaugeHeight = configuration.verticalGaugeHeight;
            this.gaugeScaling = configuration.gaugeScaling;
            this.horizontalGaugeWidth = configuration.horizontalGaugeWidth;
            this.horizontalGaugeHeight = configuration.horizontalGaugeHeight;
         }

         private int XLeftBlock(int index)
         {
            return MARGIN_X_LEFT_BLOCK + (verticalGaugeWidth + Gauges.LAYOUT_GAP) * (index % COLUMNS_LEFT_BLOCK);
         }

         private int YLeftBlock(int index)
         {
            return MARGIN_Y_LEFT_BLOCK + (verticalGaugeHeight + Gauges.LAYOUT_GAP) * (index / COLUMNS_LEFT_BLOCK);
         }

         private Pair<int, int> LeftBlock(int index)
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

         private Pair<int, int> RightBlock(int index)
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

         private Pair<int, int> TopBlock(int index)
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

         private Pair<int, int> LeftNavballBlock(int index)
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

         private Pair<int, int> RightNavballBlock(int index)
         {
            return new Pair<int, int>(XRightNavballBlock(index), YRightNavballBlock(index));
         }

         protected void SetGaugePosition(GaugeSet set, int windowId, Pair<int, int> position)
         {
            set.SetWindowPosition(windowId, position);
         }

         protected void SetGaugeEnabled(GaugeSet set, int windowId, bool enabled)
         {
            if (gauges.ContainsId(windowId))
            {
               set.SetGaugeEnabled(windowId, enabled);
               if (set.IsCurrentGaugeSet())
               {
                  gauges.SetGaugeEnabled(windowId, enabled);
               }
            }
         }

         protected void AddToRightNavballBlock(GaugeSet set, int windowId)
         {
            if(gauges.ContainsId(windowId))
            {
               SetGaugePosition(set, windowId, RightNavballBlock(rightNavBlockIndex++));
            }
         }

         protected void AddToLeftNavballBlock(GaugeSet set, int windowId)
         {
            if (gauges.ContainsId(windowId))
            {
               SetGaugePosition(set, windowId, LeftNavballBlock(leftNavBlockIndex++));
            }
         }

         protected void AddToLeftBlock(GaugeSet set, int windowId)
         {
            if (gauges.ContainsId(windowId))
            {
               SetGaugePosition(set, windowId, LeftBlock(leftBlockIndex++));
            }
         }

         protected void AddToRightBlock(GaugeSet set, int windowId)
         {
            if (gauges.ContainsId(windowId))
            {
               SetGaugePosition(set, windowId, RightBlock(rightBlockIndex++));
            }
         }

         protected void AddToTopBlock(GaugeSet set, int windowId)
         {
            if (gauges.ContainsId(windowId))
            {
               SetGaugePosition(set, windowId, TopBlock(topBlockIndex++));
            }
         }

         protected void AddToSpare(GaugeSet set, int windowId)
         {
            if (gauges.ContainsId(windowId))
            {
               int MARGIN_X_SPARE = Screen.width / 2;
               int MARGIN_Y_SPARE = 100;
               int x = MARGIN_X_SPARE + spareIndex * (verticalGaugeWidth);
               int y = MARGIN_Y_SPARE + spareRow * (verticalGaugeHeight + Gauges.LAYOUT_GAP);
               // next line ?
               if (x + verticalGaugeWidth > Screen.width)
               {
                  spareIndex = 0;
                  x = MARGIN_X_SPARE;
                  spareRow++;
                  y = MARGIN_Y_SPARE + spareRow * (verticalGaugeHeight + Gauges.LAYOUT_GAP);
               }
               set.SetWindowPosition(windowId, x, y);
               spareIndex++;
            }
         }

         public void Layout(GaugeSet set)
         {
            Log.Info("layout " + GetType() + ", set " + set);
            //
            // move all gauges to 0/0 to find gauges that are unaffected by layout
            foreach(AbstractGauge g in gauges)
            {
               int id = g.GetWindowId();
               set.SetWindowPosition(id, Constants.ORIGIN);
            }
            //
            DoLayout(set);
            //
            EnableGauges(set);

            Reset();
            foreach (AbstractGauge g in gauges)
            {
               int id = g.GetWindowId();
               if(set.GetWindowPosition(id).Equals(Constants.ORIGIN))
               {
                  AddToSpare(set, id);
               }
            }

            Log.Detail("layout done for " + set);
         }



         public sealed override string ToString()
         {
            return GetType().Name;
         }

         protected void Reset()
         {
            topBlockIndex = 0;
            leftBlockIndex = 0;
            rightBlockIndex = 0;
            leftNavBlockIndex = 0;
            rightNavBlockIndex = 0;
            spareIndex = 0;
            spareRow = 0;
         }


         public abstract void DoLayout(GaugeSet set);

         public abstract void EnableGauges(GaugeSet set);
      }


   }
}
