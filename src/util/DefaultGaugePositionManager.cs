using System;
using UnityEngine;
using System.Collections.Generic;

namespace Nereid
{
   namespace NanoGauges
   {
      public class DefaultGaugePositionManager
      {

         private enum ORIGIN { RIGHTEDGE, LEFTEDGE, TOPEDGE, BOTTOMEDGE, BOTTOMRIGHT, NAVBALL };

         private readonly Dictionary<GaugeSet.ID, Dictionary<int,bool>> defaultEnabledMap = new Dictionary<GaugeSet.ID, Dictionary<int,bool>>();
         private readonly Dictionary<GaugeSet.ID, Dictionary<int, int>> defaultPositionXMap = new Dictionary<GaugeSet.ID, Dictionary<int, int>>();
         private readonly Dictionary<GaugeSet.ID, Dictionary<int, int>> defaultPositionYMap = new Dictionary<GaugeSet.ID, Dictionary<int, int>>();

         public DefaultGaugePositionManager()
         {
            var gaugesets = Enum.GetValues(typeof(GaugeSet.ID));

            foreach (GaugeSet.ID id in gaugesets)
            {
               defaultEnabledMap[id] = new Dictionary<int, bool>();
               defaultPositionXMap[id] = new Dictionary<int, int>();
               defaultPositionYMap[id] = new Dictionary<int, int>();
            }
         }

         private void DefineDefaultPosition(GaugeSet.ID set, int windowId, bool enabled, ORIGIN origin, int dx, int dy)
         {
            Dictionary<int, bool> enabledMap = defaultEnabledMap[set];
            Dictionary<int, int> positionXMap = defaultPositionXMap[set];
            Dictionary<int, int> positionYMap = defaultPositionYMap[set];

            int LAYOUT_CELL_X = NanoGauges.configuration.verticalGaugeWidth + Gauges.LAYOUT_GAP;
            int LAYOUT_CELL_Y = NanoGauges.configuration.verticalGaugeHeight + Gauges.LAYOUT_GAP;
            int LAYOUT_RANGE_X = 3 * LAYOUT_CELL_X / 2;
            int LAYOUT_RANGE_Y = 3 * LAYOUT_CELL_Y / 2;

            enabledMap.Add(windowId, enabled);
            switch(origin)
            {
               case ORIGIN.RIGHTEDGE:
                  {
                     int x0 = Screen.width - LAYOUT_CELL_X;
                     int y0 = Screen.height / 2;
                     positionXMap.Add(windowId, x0 + dx);
                     positionYMap.Add(windowId, y0 + dy);
                     break;
                  }
               case ORIGIN.BOTTOMRIGHT:
                  {
                     int x0 = Screen.width - LAYOUT_CELL_X;
                     int y0 = Screen.height - LAYOUT_CELL_Y;
                     positionXMap.Add(windowId, x0 + dx);
                     positionYMap.Add(windowId, y0 + dy);
                     break;
                  }
               case ORIGIN.LEFTEDGE:
                  {
                     int y0 = Screen.height / 2;
                     positionXMap.Add(windowId, dx);
                     positionYMap.Add(windowId, y0 + dy);
                     break;
                  }
               case ORIGIN.TOPEDGE:
                  {
                     int x0 = Screen.width/2;
                     positionXMap.Add(windowId, x0 + dx);
                     positionYMap.Add(windowId, dy);
                     break;
                  }
               case ORIGIN.BOTTOMEDGE:
                  {
                     int x0 = Screen.width / 2;
                     int y0 = Screen.height - LAYOUT_CELL_Y;
                     positionXMap.Add(windowId, x0 + dx);
                     positionYMap.Add(windowId, y0 + dy);
                     break;
                  }
               case ORIGIN.NAVBALL:
                  {
                     int x0 = Screen.width / 2;
                     int y0 = Screen.height - 200;
                     positionXMap.Add(windowId, x0 + dx);
                     positionYMap.Add(windowId, y0 + dy);
                     break;
                  }
               default:
                  {
                     Log.Warning("invalid origin in gauge set " + set + ", window id " + windowId);
                     positionXMap.Add(windowId, dx);
                     positionYMap.Add(windowId, dy);
                     break;
                  }
            }

         }

         public int GetDefaultX(GaugeSet.ID set, int windowId)
         {
            return 0;
         }

         public int GetDefaultY(GaugeSet.ID set, int windowId)
         {
            return 0;
         }

         public bool GetDefaultEnabled(GaugeSet.ID set, int windowId)
         {
            return true;
         }

      }
   }
}
