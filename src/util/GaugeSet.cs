using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Nereid
{
   namespace NanoGauges
   {
      public class GaugeSet : IEnumerable<int>
      {
         private static readonly Pair<int, int> ORIGIN = new Pair<int, int>(0, 0);

         private readonly ID id;

         private readonly HashSet<int> gaugeIds = new HashSet<int>();
         private readonly Dictionary<int, Pair<int, int>> windowPositions = new Dictionary<int, Pair<int, int>>();
         private readonly Dictionary<int, bool> gaugesEnabled = new Dictionary<int, bool>();

         public enum ID { STANDARD=0, LAUNCH=1, LAND=2, DOCK=3, ORBIT=4, FLIGHT=5, SET1=101, SET2=102, SET3=103, CLIPBOARD=999  };

         public GaugeSet(ID id)
         {
            this.id = id;   
         }

         public ID GetId()
         {
            return id;
         }

         public Pair<int, int> GetWindowPosition(AbstractWindow window)
         {
            return GetWindowPosition(window.GetWindowId());
         }

         public Pair<int, int> GetWindowPosition(int windowId)
         {
            try
            {
               return windowPositions[windowId];
            }
            catch (KeyNotFoundException)
            {
               Log.Warning("no initial position found for window " + windowId+" in gauge set");
               return ORIGIN;
            }
         }

         public void SetWindowPosition(AbstractWindow window)
         {
            SetWindowPosition(window.GetWindowId(), window.GetX(), window.GetY());
         }

         public void SetWindowPosition(int windowId, Pair<int, int> position)
         {
            if(Log.IsLogable(Log.LEVEL.TRACE)) Log.Trace("set window position for window id " + windowId + ": " + position);
            if (windowPositions.ContainsKey(windowId))
            {
               windowPositions[windowId] = position;
            }
            else
            {
               windowPositions.Add(windowId, position);
               if (!gaugeIds.Contains(windowId))
               {
                  gaugeIds.Add(windowId);
               }
            }
         }

         public void SetWindowPosition(int windowId, int x, int y)
         {
            SetWindowPosition(windowId, new Pair<int, int>(x, y));
         }

         public bool IsOptionalGauge(int windowId)
         {
            switch(windowId)
            {
               // TAC life support
               case Constants.WINDOW_ID_GAUGE_O2:
                  return true;
            }
            return false;
         }

         public void copyPositionsFrom(GaugeSet source)
         {
            foreach (int key in source.windowPositions.Keys)
            {
               SetWindowPosition(key, source.GetWindowPosition(key));
            }
         }

         public void copyStatesFrom(GaugeSet source)
         {
            foreach (int key in source.gaugesEnabled.Keys)
            {
               SetGaugeEnabled(key, source.IsGaugeEnabled(key));
            }
            foreach (int key in gaugeIds)
            {
               gaugesEnabled[key] = source.IsGaugeEnabled(key);
            }
         }


         public void copyFrom(GaugeSet source)
         {
            copyPositionsFrom(source);
            copyStatesFrom(source);
         }

         // BROKEN
         public void copyArrangementFrom(GaugeSet source)
         {
            int[] sourceSortedByPosition = new int[source.gaugeIds.Count];
            int sourceCnt = 0;
            foreach (int id in source.gaugeIds)
            {
               if (gaugesEnabled[id])
               {
                  sourceSortedByPosition[sourceCnt] = id;
                  sourceCnt++;
               }
            }
            int[] thisSortedByPosition = new int[source.gaugeIds.Count];
            int thisCnt = 0;
            foreach (int id in gaugeIds)
            {
               if (gaugesEnabled[id])
               {
                  thisSortedByPosition[thisCnt] = id;
                  thisCnt++;
               }
            }

            Array.Sort(sourceSortedByPosition,
               delegate(int left, int right)
               {
                  if (right == 0) return -1;
                  if (left == 0) return  1;
                  Pair<int, int> posLeft = source.GetWindowPosition(left);
                  Pair<int, int> posRight = source.GetWindowPosition(right);
                  int xLeft = posLeft.first;
                  int yLeft = posLeft.second;
                  int xRight = posLeft.first;
                  int yRight = posLeft.second;
                  //

                  if (yLeft.In(yRight, yRight + NanoGauges.configuration.verticalGaugeHeight)
                     || yRight.In(yLeft, yLeft + NanoGauges.configuration.verticalGaugeHeight))
                  {
                     return (xLeft.CompareTo(xRight));
                  }
                  return (yLeft.CompareTo(yRight));
               }
            );

            Array.Sort(thisSortedByPosition,
               delegate(int left, int right)
               {
                  if (right == 0) return -1;
                  if (left == 0) return 1;
                  Pair<int, int> posLeft = GetWindowPosition(left);
                  Pair<int, int> posRight = GetWindowPosition(right);
                  int xLeft = posLeft.first;
                  int yLeft = posLeft.second;
                  int xRight = posLeft.first;
                  int yRight = posLeft.second;
                  //

                  if (yLeft.In(yRight, yRight + NanoGauges.configuration.verticalGaugeHeight)
                     || yRight.In(yLeft, yLeft + NanoGauges.configuration.verticalGaugeHeight))
                  {
                     return (xLeft.CompareTo(xRight));
                  }
                  return (yLeft.CompareTo(yRight));
               }
            );

            for (int i = 0; i < sourceCnt && i < thisCnt; i++)
            {
               SetWindowPosition(thisSortedByPosition[i], source.GetWindowPosition(sourceSortedByPosition[i]));
            }
         }


         public bool IsGaugeEnabled(int windowId)
         {
            if (gaugesEnabled.ContainsKey(windowId))
            {
               return gaugesEnabled[windowId];
            }
            gaugesEnabled.Add(windowId, !IsOptionalGauge(windowId));
            if (!gaugeIds.Contains(windowId))
            {
               gaugeIds.Add(windowId);
            }
            return true;
         }

         public void SetGaugeEnabled(int windowId, bool enabled)
         {
            if (gaugesEnabled.ContainsKey(windowId))
            {
               gaugesEnabled[windowId] = enabled;
            }
            else
            {
               gaugesEnabled.Add(windowId, enabled);
               if (!gaugeIds.Contains(windowId))
               {
                  gaugeIds.Add(windowId);
               }
            }
         }

         public override string ToString()
         {
            return id.ToString();
         }

         public int Count()
         {
            return Math.Max(windowPositions.Count, gaugesEnabled.Count);
         }
         
         public HashSet<int> Keys()
         {
            return gaugeIds;
         }

         public System.Collections.IEnumerator GetEnumerator()
         {
            return gaugeIds.GetEnumerator();
         }

         IEnumerator<int> IEnumerable<int>.GetEnumerator()
         {
            return gaugeIds.GetEnumerator();
         }

      }
   }
}
