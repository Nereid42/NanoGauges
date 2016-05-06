using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Nereid
{
   namespace NanoGauges
   {
      class WindowManager
      {
         public static readonly WindowManager instance = new WindowManager();

         private readonly List<AbstractWindow> windows = new List<AbstractWindow>();

         // debug
         private readonly TimedStatistics.Timer windowTimer = TimedStatistics.instance.GetTimer("Windows");

         public void AddWindow(AbstractWindow window)
         {
            windows.Add(window);
         }


         public void RemoveWindow(AbstractWindow window)
         {
            windows.Remove(window);
         }

         public void OnGUI()
         {
            windowTimer.Start();
            if (Event.current.type == EventType.Layout)
            {
               foreach(AbstractWindow window in windows)
               {
                  // draw a window only if its visible 
                  if(window.IsVisible())
                  {
                     window.OnGUI();
                  }
               }
            }
            windowTimer.Stop();
         }
      }
   }
}
