using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nereid
{
   namespace NanoGauges
   {
      class MovingAverage
      {
         private readonly double[] values;
         private int cnt;
         private int p;

         private double value;

         public MovingAverage(int capcity=10)
         {
            this.values = new double[capcity];
            this.cnt = 0;
            this.p = 0;
            this.value = 0.0;
         }

         public void AddValue(double value)
         {
            values[p] = value;
            if (cnt < values.Length) cnt++;
            p = (p + 1) % values.Length;
            CalcValue();
         }

         public void Clear()
         {
            this.cnt = 0;
            this.p = 0;
            this.value = 0.0;
         }

         private void CalcValue()
         {
            this.value = 0.0;
            if (cnt == 0) return;
            for (int i = 0; i < cnt; i++)
            {
               this.value += values[i];
            }
            this.value = this.value / cnt;
         }

         public double GetValue()
         {
            return value;
         }
      }
   }
}
