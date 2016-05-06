using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Nereid
{
   namespace NanoGauges
   {
      public abstract class Inspecteur
      {
         private long updateCycle = 0;

         private volatile Vessel vessel = null;
         private volatile int partCount = 0;
         private volatile bool forceRescan = false;

         private double lastInspectTime;
         private readonly long cycleInterval;

         public abstract void Reset();
         protected abstract void Inspect(Vessel vessel);
         protected abstract void ScanVessel(Vessel vessel);
         
         protected virtual void PartUnpacked(Part part)
         {
            // do nothing by default
         }

         // for debugging
         private long cnt;
         Stopwatch sw = new Stopwatch();
         //
         private readonly TimedStatistics.Timer inspectTimer = TimedStatistics.instance.GetTimer("Inspect");
         private readonly TimedStatistics.Timer scanTimer = TimedStatistics.instance.GetTimer("Scan");
         private readonly TimedStatistics.Timer unpackTimer = TimedStatistics.instance.GetTimer("Unpack");

         protected Inspecteur(long cycleInterval)
         {
            this.cycleInterval = cycleInterval;
            GameEvents.onVesselChange.Add(this.OnVesselChange);
            GameEvents.onPartCouple.Add(this.OnPartCouple);
            GameEvents.onPartUndock.Add(this.OnPartUndock);
            GameEvents.onPartDestroyed.Add(this.OnPartDestroyed);
            GameEvents.onPartAttach.Add(this.OnPartAttach);
            GameEvents.onVesselWasModified.Add(this.OnVesselWasModified);
            GameEvents.onJointBreak.Add(this.OnJointBreak);
            GameEvents.onGameStateCreated.Add(OnGameStateCreated);
            GameEvents.onPartUnpack.Add(this.OnPartUnpack);
         }

         private void OnPartDestroyed(Part part)
         {
            if (part == null || part.vessel == null || part.vessel != this.vessel) return;
            VesselModified();
         }


         private void OnPartAttach(GameEvents.HostTargetAction<Part,Part> action)
         {
            VesselModified();
         }

         private void OnPartCouple(GameEvents.FromToAction<Part, Part> action)
         {
            VesselModified();
         }

         private void OnPartUndock(Part part)
         {
            if (part == null || part.vessel == null || part.vessel != FlightGlobals.ActiveVessel) return;
            VesselModified();
         }

         private void OnPartUnpack(Part part)
         {
            unpackTimer.Start();
            try
            {
               Vessel vessel = FlightGlobals.ActiveVessel;
               if (vessel == null) return;
               if (part != null && part.vessel == vessel)
               {
                  if (Log.IsLogable(Log.LEVEL.DETAIL))
                  {
                     Log.Detail("part unpacked " + part.name);
                  }
                  PartUnpacked(part);
               }
            }
            finally
            {
               unpackTimer.Stop();
            }
         }

         private void OnGameStateCreated(Game game)
         {
            VesselModified();
         }

         private void OnVesselWasModified(Vessel vessel)
         {
            VesselModified();
         }

         private void OnVesselChange(Vessel vessel)
         {
            if(Log.IsLogable(Log.LEVEL.INFO)) Log.Info("Vessel changed");
            if (vessel == null || vessel!=FlightGlobals.ActiveVessel) return;
            VesselModified();
            
         }

         private void OnJointBreak(EventReport report)
         {
            if (report.origin == null || report.origin == null || report.origin != FlightGlobals.ActiveVessel) return;
            VesselModified();
         }

         private void VesselModified()
         {
            // this will cause a rescan of the vessel next time
            vessel = null;
            partCount = 0;
            forceRescan = true;
         }

         private void ReScanVessel()
         {
            ReScanVessel(FlightGlobals.ActiveVessel);
         }

         private void ReScanVessel(Vessel vessel)
         {
            cnt++;
            forceRescan = false;
            this.vessel = vessel;
            if (Log.IsLogable(Log.LEVEL.DETAIL)) Log.Detail("vessel rescan initiated " + this.GetType()+", vessel "+vessel );
            if (vessel != null)
            {
               if (Log.IsLogable(Log.LEVEL.DETAIL)) sw.Start();
               ScanVessel(vessel);
               if (Log.IsLogable(Log.LEVEL.DETAIL)) sw.Stop();
               if (Log.IsLogable(Log.LEVEL.DETAIL)) Log.Detail("elapsed scan time in " + this.GetType() + ": " + sw.ElapsedMilliseconds + " ms (" + cnt + " times)");
            }
            if (Log.IsLogable(Log.LEVEL.INFO)) Log.Info("vessel rescan finished (scanned " + cnt + " times)");
         }

         protected double GetLastInspectTime()
         {
            return lastInspectTime;
         }

         private int CountParts(Vessel vessel)
         {
            if (vessel == null) return 0;
            if (vessel.Parts == null) return 0;
            return vessel.Parts.Count;
         }

         public void Update()
         {
            if (updateCycle % cycleInterval == 0)
            {
               Vessel active = FlightGlobals.ActiveVessel;
               if (active != null)
               {
                    if (forceRescan || vessel == null || active!=vessel || CountParts(vessel) != partCount)
                    {
                       // rescan when physics are in place only
                       if (!active.HoldPhysics)
                       {
                          partCount = CountParts(active);
                          vessel = active;
                          scanTimer.Start();
                          ReScanVessel();
                          scanTimer.Stop();
                       }
                    }

                    // don't update anything if the game is paused 
                    if (!Planetarium.Pause)
                    {
                       inspectTimer.Start();
                       Inspect(vessel);
                       inspectTimer.Stop();
                       //
                       lastInspectTime = Planetarium.GetUniversalTime();
                    }
               }
               else
               {
                  vessel = null;
                  partCount = 0;
                  lastInspectTime = 0;
                  Reset();
               }
            }
            updateCycle++;
         }

 
      }
   }
}
