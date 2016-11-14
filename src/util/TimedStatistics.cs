using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;


namespace Nereid
{
   namespace NanoGauges
   {
      class TimedStatistics
      {
         public static readonly TimedStatistics instance = new TimedStatistics();

         private Stopwatch inflightTimer = new Stopwatch();

         private readonly Dictionary<String, Timer> timer;

         private TimedStatistics()
         {
            timer = new Dictionary<string, Timer>();
            GameEvents.OnFlightGlobalsReady.Add(OnFlightGlobalsReady);
         }

         private void OnFlightGlobalsReady(bool ready)
         {
            Reset();
         }

         public Timer GetTimer(String name)
         {
            if(!timer.ContainsKey(name))
            {
               timer[name] = new Timer(name);
            }
            return timer[name];
         }

         public override String ToString()
         {

            long inflight = inflightTimer.ElapsedMilliseconds;

            StringBuilder sb = new StringBuilder("time in flight: " + inflight + " ms\n");
            long total = 0;
            foreach(String name in timer.Keys)
            {
               TimedStatistics.Timer t = timer[name];
               long elapsed = t.ElapsedMilliseconds;
               total += elapsed;
               double pct = (double)elapsed / (double)inflight * 100;
               double cntPerSecond = (double)t.Count * 1000.0 / (double)inflight;
               sb.Append(name + ": " + elapsed + " ms (" + pct.ToString("0.000") + "%, " + cntPerSecond.ToString("0.0") + "x per sec, "+t.Count+"x)\n");
            }

            double totalPct = (double)total / (double)inflight * 100;
            sb.Append("Total: " + total + " ms (" + totalPct.ToString("0.000") + "%)\n");

            return sb.ToString();
         }

         public void Reset()
         {
            if (Log.IsLogable(Log.LEVEL.DETAIL)) Log.Detail("reset of timed statistics");
            //
            inflightTimer.Reset();
            inflightTimer.Start();
            foreach (String name in timer.Keys)
            {
               timer[name].Reset();
            }
         }

         public class Timer
         {
            private readonly String name;
            private readonly Stopwatch stopwatch;

            public long ElapsedMilliseconds { get { return stopwatch.ElapsedMilliseconds; } }

            public long Count { get; private set; }

            public Timer(String name)
            {
               this.name = name;
               stopwatch = new Stopwatch();
            }

            public void Start()
            {
               stopwatch.Start();
            }

            public void Stop()
            {
               if(stopwatch.IsRunning)
               {
                  stopwatch.Stop();
                  Count++;
                  //Log.Test("STOP of "+name+": "+Count);
               }
               else if(Log.IsLogable(Log.LEVEL.WARNING))
               {
                  Log.Warning("timer "+name+" not running");
               }
            }

            public void Reset()
            {
               stopwatch.Reset();
               Count = 0;
            }
         }
      }
   }
}
