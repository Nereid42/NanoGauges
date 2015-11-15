using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace Nereid
{
   namespace NanoGauges
   {
      public class VesselInspecteur : Inspecteur
      {
         private static readonly double MIN_INTERVAL = 0.15;

         public enum GEARSTATES {      
            RETRACTED = 0,
            DEPLOYED = 1,
            PARTIAL_DEPLOYED = 2,
            DEPLOYING = 3,
            RETRACTING = 4,
            NOT_INSTALLED = 5
         }

         public enum BRAKESTATES {      
            NOT_ENGAGED = 0,
            ENGAGED = 1,
            PARTIAL_ENGAGED = 2,
            NOT_INSTALLED = 3
         }

         public enum FPLAPSTATES
         {
            NOT_ENGAGED = 0,
            ENGAGED = 1,
            PARTIAL_ENGAGED = 2,
            NOT_INSTALLED = 3
         }

         private double totalMass = 0;
         private double dragCoefficent = 0;
         private double heatshieldTemp = 0;
         private bool heatshieldInstalled = false;
         private double drillTemp = 0;
         private bool drillInstalled = false;
         public GEARSTATES landingGearState { get; private set; }
         public BRAKESTATES brakeState { get; private set; }
         public BRAKESTATES airBrakeState { get; private set; }
         public FPLAPSTATES flapState { get; private set; }

         private readonly List<Part> heatshieldParts = new List<Part>();
         private readonly List<Part> drillParts = new List<Part>();
         private readonly List<ModuleLandingGear> landingGears = new List<ModuleLandingGear>();
         private readonly List<ModuleLandingGearFixed> landingGearsFixed = new List<ModuleLandingGearFixed>();
         private readonly List<ModuleControlSurface> airBrakes = new List<ModuleControlSurface>();
         private readonly List<ModuleControlSurface> flaps = new List<ModuleControlSurface>();



         public VesselInspecteur()
            : base(10, MIN_INTERVAL)
         {
            Reset();
         }


         public override void Reset()
         {
            this.drillParts.Clear();
            this.drillInstalled = false;
            this.heatshieldParts.Clear();
            this.landingGears.Clear();
            this.airBrakes.Clear();
            this.flaps.Clear();
            this.totalMass = 0.0;
            this.dragCoefficent = 0.0;
            this.heatshieldTemp = 0.0;
            this.heatshieldInstalled = false;
            this.landingGearState = GEARSTATES.NOT_INSTALLED;
            this.brakeState = BRAKESTATES.NOT_INSTALLED;
            this.airBrakeState = BRAKESTATES.NOT_INSTALLED;
            this.flapState = FPLAPSTATES.NOT_INSTALLED;
         }

         public double GetDragCoefficent()
         {
            return this.dragCoefficent;
         }

         protected void ScanVesselParts(Vessel vessel)
         {
            Reset();
            foreach (Part part in vessel.Parts)
            {
               if (part.packed) part.Unpack();

               // Debug
               if(Log.IsLogable(Log.LEVEL.DETAIL))
               {
                  Log.Detail("scanning vessel part "+part.name);
                  foreach(PartModule module in part.Modules)
                  {
                     Log.Detail(" module " + module.GetType());
                  }
               }

               foreach (PartResource r in part.Resources)
               {
                  if ((r.info != null && Resources.ABLATOR != null && r.info.id == Resources.ABLATOR.id)                          // Stock
                  || (r.info != null && Resources.ABLATIVE_SHIELDING != null && r.info.id == Resources.ABLATIVE_SHIELDING.id))  // Deadly Reentry
                  {
                     heatshieldParts.Add(part);
                     this.heatshieldInstalled = true;
                     break;
                  }
               }

               if (part.IsDrill())
               {
                  drillParts.Add(part);
                  this.drillInstalled = true;
               }

               // landing gears and brakes
               foreach(ModuleLandingGear g in part.Modules.OfType<ModuleLandingGear>())
               {
                  landingGears.Add(g);
               }
               foreach (ModuleLandingGearFixed g in part.Modules.OfType<ModuleLandingGearFixed>())
               {
                  landingGearsFixed.Add(g);
               }

               foreach (ModuleControlSurface s in part.Modules.OfType<ModuleControlSurface>())
               {
                  bool isAirBrake = false;
                  // 
                  if (s.ignoreRoll && s.ignoreYaw && s.ignorePitch)
                  {
                     foreach (BaseAction a in s.Actions)
                     {
                        if (a.name.Equals("ActionToggleBrakes"))
                        {
                           airBrakes.Add(s);
                           isAirBrake = true;
                           break;
                        }
                     }
                     // no airbrake => flap
                     if (!isAirBrake)
                     {
                        flaps.Add(s);
                     }
                  }
               }
            }
            this.landingGearState = InspectLandingGear();
            this.brakeState = InspectBrakes();
            this.airBrakeState = InspectAirBrakes();
            this.flapState = InspectFlaps();
         }

         protected override void ScanVessel(Vessel vessel)
         {

            if (vessel == null)
            {
               this.totalMass = 0.0;
               this.dragCoefficent = 0.0;
               this.heatshieldTemp = 0.0;
               this.heatshieldInstalled = false;

            }
            else
            {
               totalMass = vessel.GetTotalMass();

               // heat shields
               this.heatshieldParts.Clear();
               this.heatshieldInstalled = false;
               // drills
               this.drillParts.Clear();
               this.drillInstalled = false;

               // inspect parts if present
               if (vessel != null && vessel.Parts != null)
               {
                  ScanVesselParts(vessel);
               }
            }
         }


         public double GetTotalMass()
         {
            return totalMass;
         }

         public double GetHeatshieldTemp()
         {
            return heatshieldTemp;
         }

         public bool IsHeatshieldInstalled()
         {
            return heatshieldInstalled;
         }

         public bool IsDrillInstalled()
         {
            return drillInstalled;
         }

         public double GetDrillTemperature()
         {
            return drillTemp;
         }

         private BRAKESTATES InspectAirBrakes()
         {
            if (airBrakes.Count == 0) return BRAKESTATES.NOT_INSTALLED;
            int deployed = 0;
            foreach (ModuleControlSurface s in airBrakes)
            {
               if (s.deploy)
               {
                  deployed++;
               }
            }
            if (deployed == 0)
            {
               return BRAKESTATES.NOT_ENGAGED;
            }
            if (deployed < airBrakes.Count)
            {
               return BRAKESTATES.PARTIAL_ENGAGED;
            }
            return BRAKESTATES.ENGAGED;
         }

         private FPLAPSTATES InspectFlaps()
         {
            if (flaps.Count == 0) return FPLAPSTATES.NOT_INSTALLED;
            int deployed = 0;
            foreach (ModuleControlSurface s in flaps)
            {
               if (s.deploy)
               {
                  deployed++;
               }
            }
            if (deployed == 0)
            {
               return FPLAPSTATES.NOT_ENGAGED;
            }
            if (deployed < airBrakes.Count)
            {
               return FPLAPSTATES.PARTIAL_ENGAGED;
            }
            return FPLAPSTATES.ENGAGED;
         }


         private GEARSTATES InspectLandingGear()
         {
            if (landingGears.Count == 0) return GEARSTATES.NOT_INSTALLED;
            int deployed = 0;
            foreach (ModuleLandingGear g in landingGears)
            {
               switch (g.gearState)
               {
                  case ModuleLandingGear.GearStates.DEPLOYED: 
                     deployed++;
                     break;
                  case ModuleLandingGear.GearStates.RETRACTING:
                     return GEARSTATES.RETRACTING;
                  case ModuleLandingGear.GearStates.DEPLOYING:
                     return GEARSTATES.DEPLOYING;
               }
            }
            if(deployed == 0)
            {
               return GEARSTATES.RETRACTED;
            }
            if(deployed < landingGears.Count)
            {
               return GEARSTATES.PARTIAL_DEPLOYED;
            }
            return GEARSTATES.DEPLOYED;
         }

         private BRAKESTATES InspectBrakes()
         {
            if (landingGears.Count == 0 && landingGearsFixed.Count == 0) return BRAKESTATES.NOT_INSTALLED;
            int engaged = 0;
            foreach (ModuleLandingGear g in landingGears)
            {
               if (g.brakesEngaged && g.BrakeTorque>0)
               {
                  engaged++;
               }
            }
            foreach (ModuleLandingGearFixed g in landingGearsFixed)
            {
               if (g.brakesEngaged && g.BrakeTorque > 0)
               {
                  engaged++;
               }
            }
            if (engaged == 0)
            {
               return BRAKESTATES.NOT_ENGAGED;
            }
            if (engaged < (landingGears.Count+landingGearsFixed.Count))
            {
               return BRAKESTATES.PARTIAL_ENGAGED;
            }
            return BRAKESTATES.ENGAGED;
         }

         protected override void Inspect(Vessel vessel)
         {
            if(vessel==null)
            {
               totalMass = 0.0;
               heatshieldTemp = 0.0;
            }
            else
            {
               // mass
               totalMass = vessel.GetTotalMass();
               // heat shields
               heatshieldTemp = Constants.MIN_TEMP;
               foreach(Part p in heatshieldParts)
               {
                  if(p.temperature > heatshieldTemp)
                  {
                     heatshieldTemp = p.temperature;
                  }
               }
               // drills
               drillTemp = Constants.MIN_TEMP;
               foreach (Part p in drillParts)
               {
                  if (p.temperature > drillTemp)
                  {
                     drillTemp = p.temperature;
                  }
               }
               // landing gears
               this.landingGearState = InspectLandingGear();
               // brakes
               this.brakeState = InspectBrakes();
               // air brakes
               this.airBrakeState = InspectAirBrakes();
               // flaps
               this.flapState = InspectFlaps();
            }
         }
      }
   }
}
