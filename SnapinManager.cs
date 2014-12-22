using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nereid
{
   namespace NanoGauges
   {
      public class SnapinManager
      {
         private readonly Gauges gauges;

         public SnapinManager(Gauges gauges)
         {
            this.gauges = gauges;
         }

         public void snap(AbstractGauge gauge)
         {
            checkSnap(gauge, gauge.GetX(), gauge.GetY(), gauge.GetWidth(), gauge.GetHeight());
         }

         private void checkSnap(AbstractGauge gauge, int x, int y, int w, int h)
         {
            int r = NanoGauges.configuration.GetSnapinRange();
            foreach (AbstractGauge g in gauges)
            {
               int x0 = g.GetX();
               int y0 = g.GetY();
               int w0 = g.GetWidth();
               int h0 = g.GetHeight();
               int x1 = x0 + w0;
               int y1 = y0 + h0;

               // snap left side
               if ( Math.Abs(x - x1) <= r && Math.Abs(y - y0) <= r)
               {
                  snapGaugeAt(gauge, x1 + Gauges.LAYOUT_GAP, y0);
               }
               // snap right side
               else if (Math.Abs(x0 - (x+w)) <= r && Math.Abs(y - y0) <= r)
               {
                  snapGaugeAt(gauge, x0 - w - Gauges.LAYOUT_GAP, y0);
               }
               // snap bottom side
               else if (Math.Abs(y0 - (y + h)) <= r && Math.Abs(x - x0) <= r)
               {
                  snapGaugeAt(gauge, x0, y0 - h - Gauges.LAYOUT_GAP);
               }
               // snap top side
               else if (Math.Abs(y1 - y) <= r && Math.Abs(x - x0) <= r)
               {
                  snapGaugeAt(gauge, x0, y1 + Gauges.LAYOUT_GAP);
               }
            }
         }

         private void snapGaugeAt(AbstractGauge gauge, int x, int y)
         {
            gauge.SetPosition(x, y);
            NanoGauges.configuration.SetWindowPosition(gauge);
            if(Log.IsLogable(Log.LEVEL.DETAIL))
            {
               Log.Detail("snapped window "+gauge.GetName()+" at "+x+"/"+y);
            }
         }

      }
   }
}
