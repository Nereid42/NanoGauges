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
            p++;
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
            if (cnt == 0)
            {
               this.value = 0.0;
               return;
            }
            double result = 0.0;
            for (int i = 0; i < cnt; i++)
            {
               result += values[i];
            }
            this.value = result / cnt;
         }

         public double GetValue()
         {
            return value;
         }
      }
   }
}
