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
         public double engineTotalThrust { get; private set; }
         public double engineIspPerRunningEngine  { get; private set; }
         public float propReqPerRunningEngine { get; private set; }
         public int enginesRunningCount { get; private set; }
         public int enginesTotalCount { get; private set; }
         private MovingAverage deltaIspPerSecond = new MovingAverage(5);

         public bool afterburnerInstalled { get; private set; }
         public bool afterburnerEnabled { get; private set; }
         private readonly List<ModuleEngines> engines = new List<ModuleEngines>();
         private readonly List<MultiModeEngine> afterburner = new List<MultiModeEngine>();         

         public EngineInspecteur()
            : base(2)
         {
            Reset();
         }

         public override void Reset()
         {
            this.engineTotalThrust = 0.0;
            this.engineIspPerRunningEngine = 0.0;
            this.propReqPerRunningEngine = 0.0f;
            this.enginesRunningCount = 0;
            deltaIspPerSecond.Clear();
            engines.Clear();
            this.afterburnerEnabled = false;
            this.afterburnerInstalled = false;
         }

         public double GetDeltaIspperSecond()
         {
            return deltaIspPerSecond.GetValue();
         }


         private void ScanPart(Part part)
         {
            int moduleCount = 0;
            foreach (ModuleEngines engine in part.Modules.OfType<ModuleEngines>())
            {
               if(!this.engines.Contains(engine))
               {
                  this.engines.Add(engine);
                  moduleCount++;
               }
            }

            foreach (MultiModeEngine afterburner in part.Modules.OfType<MultiModeEngine>())
            {
               if (!this.afterburner.Contains(afterburner))
               {
                  this.afterburner.Add(afterburner);
                  this.afterburnerInstalled = true;
               }
            }
            if (moduleCount > 0)
            {
               enginesTotalCount++;
            }
         }

         protected override void PartUnpacked(Part part)
         {
            ScanPart(part);
         }

         protected override void ScanVessel(Vessel vessel)
         {
            engines.Clear();
            enginesTotalCount = 0;
            this.afterburnerInstalled = false;
            if (vessel == null || vessel.Parts == null) return;
            foreach (Part part in vessel.Parts)
            {
               if (!part.packed)  ScanPart(part);
            }
         }

         protected override void Inspect(Vessel vessel)
         {
            double previousIspPerRunningEngine = engineIspPerRunningEngine;
            engineTotalThrust = 0.0;
            engineIspPerRunningEngine = 0.0;
            propReqPerRunningEngine = 0.0f;
            enginesRunningCount = 0;
            enginesTotalCount = 0;
            deltaIspPerSecond.Clear();;
            foreach (ModuleEngines engine in engines)
            {
               enginesTotalCount++;
               double thrust = engine.finalThrust;
               engineTotalThrust += thrust;
               if (engine.isEnabled && thrust > 0.0)
               {
                  engineIspPerRunningEngine += engine.realIsp;
                  propReqPerRunningEngine += engine.propellantReqMet;
                  enginesRunningCount++;
               }
            }
            this.afterburnerEnabled = false;
            if (enginesRunningCount > 0)
            {
               // ISP per engine
               engineIspPerRunningEngine = engineIspPerRunningEngine / enginesRunningCount;
               // propellant requirements per running engine
               propReqPerRunningEngine = propReqPerRunningEngine / enginesRunningCount;
               // Delta ISP
               double interval = Planetarium.GetUniversalTime() - GetLastInspectTime();
               if (interval > 0.0)
               {
                  deltaIspPerSecond.AddValue((engineIspPerRunningEngine - previousIspPerRunningEngine) * (1 / interval));
               }
               // afterbruner
               // doesnt work
               /*foreach (MultiModeEngine afterburner in this.afterburner)
               {
                  if (afterburner.isEnabled && !afterburner.runningPrimary)
                  {
                     this.afterburnerEnabled = true;
                     break;
                  }
               }*/
            }
            else
            {
               engineIspPerRunningEngine = 0;
               propReqPerRunningEngine = 0;
            }
         }
      }
   }
}
