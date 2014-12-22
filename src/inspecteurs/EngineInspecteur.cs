using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Nereid
{
   namespace NanoGauges
   {
      public class EngineInspecteur : Inspecteur 
      {
         private static readonly double MIN_INTERVAL = 0.2;

         private double engineTotalThrust = 0.0;
         private double engineIspPerRunningEngine = 0.0;
         private int enginesRunning = 0;
         private int enginesTotal = 0;
         private MovingAverage deltaIspPerSecond = new MovingAverage(5);

         private readonly List<ModuleEnginesFX> enginesFX = new List<ModuleEnginesFX>();
         private readonly List<ModuleEngines> engines = new List<ModuleEngines>();

         public EngineInspecteur()
            : base(10, MIN_INTERVAL)
         {
            Reset();
         }

         public override void Reset()
         {
            this.engineTotalThrust = 0.0;
            this.engineIspPerRunningEngine = 0.0;
            this.enginesRunning = 0;
            deltaIspPerSecond.Clear();
            enginesFX.Clear();
            engines.Clear();
         }

         public double GetTotalThrust()
         {
            return engineTotalThrust;
         }


         public int GetRunningEnginesCount()
         {
            return enginesRunning;
         }

         public int GetTotalEnginesCount()
         {
            return enginesTotal;
         }

         public double GetIspPerRunningEngine()
         {
            return engineIspPerRunningEngine;
         }

         public double GetDeltaIspperSecond()
         {
            return deltaIspPerSecond.GetValue();
         }

         protected override void ScanVessel(Vessel vessel)
         {
            enginesFX.Clear();
            engines.Clear();
            if (vessel == null || vessel.Parts == null) return;
            foreach (Part part in vessel.Parts)
            {
               if(part.packed) part.Unpack();
               foreach (ModuleEnginesFX engine in part.Modules.OfType<ModuleEnginesFX>())
               {
                  enginesFX.Add(engine);
               }
               foreach (ModuleEngines engine in part.Modules.OfType<ModuleEngines>())
               {
                  engines.Add(engine);
               }
            }
         }

         protected override void Inspect(Vessel vessel)
         {
            double previousIspPerRunningEngine = engineIspPerRunningEngine;
            engineTotalThrust = 0.0;
            engineIspPerRunningEngine = 0.0;
            enginesRunning = 0;
            enginesTotal = 0;
            deltaIspPerSecond.Clear();;
            foreach (ModuleEnginesFX engine in enginesFX)
            {
               enginesTotal++;
               double thrust = engine.finalThrust;
               engineTotalThrust += thrust;
               if (thrust > 0.0)
               {
                  engineIspPerRunningEngine += engine.realIsp;
                  enginesRunning++;
               }
            }
            foreach (ModuleEngines engine in engines)
            {
               enginesTotal++;
               double thrust = engine.finalThrust;
               engineTotalThrust += thrust;
               if (thrust > 0.0)
               {
                  engineIspPerRunningEngine += engine.realIsp;
                  enginesRunning++;
               }
            }
            if (enginesRunning > 0)
            {
               // ISP per engine
               engineIspPerRunningEngine = engineIspPerRunningEngine / enginesRunning;
               // Delta ISP
               double interval = Planetarium.GetUniversalTime()-GetLastInspectTime();
               if(interval>0.0)
               {
                  deltaIspPerSecond.AddValue( (engineIspPerRunningEngine - previousIspPerRunningEngine) * (1 / interval) );
               }
              
            }
         }
      }
   }
}
