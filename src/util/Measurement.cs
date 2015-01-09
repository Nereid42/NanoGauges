using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nereid
{
   namespace NanoGauges
   {
      class Measurement
      {
         private const double MIN_INTERVAL = 0.05;
         //
         private readonly double[] samples;
         private int ptr = 0;
         //
         private double _time;
         private double _value;
         private double _delta;
         private double _per_second;


         public double value
         {
            get { return _value; }
            set 
            {
               try
               {
                  double now = Planetarium.GetUniversalTime();
                  double interval = now - _time;
                  if (interval < MIN_INTERVAL) return;
                  samples[ptr] = value;
                  ptr = (ptr + 1) % samples.Length;

                  if (_time > 0)
                  {
                     double avg = Average();
                     _delta = avg - _value;
                     _value = avg;
                     _per_second = _delta / interval;
                  }
                  _time = now;
               }
               catch
               {
                  Log.Detail("measurement failed");
                  Reset();
               }
            }
         }

         public double delta
         {
            get { return _delta; }
         }

         public double ChangePerSecond
         {
            get { return _per_second; }
         }

         private double Average()
         {
            double sum = 0.0;
            for (int i = 0; i < samples.Length; i++)
            {
               sum += samples[i];
            }
            return sum / samples.Length;
         }

         public Measurement(int samples=3)
         {
            this.samples = new double[samples];
            Reset();
         }

         public void Reset()
         {
            _value = 0.0;
            _delta = 0.0;
            _time = 0.0;
            _per_second = 0.0;
            for(int i=0; i<samples.Length; i++)
            {
               samples[i] = 0.0;
            }
         }

         public override string ToString()
         {
            String s = "";
            for (int i = 0; i < samples.Length; i++)
            {
               if (i > 0) s = s + "/";
               s = s + samples[i].ToString("0.00");
            }
            return _value.ToString(".00") + " [d=" + _delta.ToString("0.00") + " " + _per_second.ToString("0.00") + "/s, samples="+s+"]";
         }
      }
   }
}
