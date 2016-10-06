using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Nereid
{
   namespace NanoGauges
   {
      public class VesselInspecteur : Inspecteur
      {
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
         private double dryMass = 0;
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
         private readonly List<ModuleWheels.ModuleWheelBrakes> wheelBrakes = new List<ModuleWheels.ModuleWheelBrakes>();
         private readonly List<ModuleWheels.ModuleWheelDeployment> wheelDeployment = new List<ModuleWheels.ModuleWheelDeployment>();
         private readonly List<ModuleControlSurface> airBrakes = new List<ModuleControlSurface>();
         private readonly List<ModuleControlSurface> flaps = new List<ModuleControlSurface>();



         public VesselInspecteur()
            : base(3)
         {
            Reset();
         }


         public override void Reset()
         {
            this.drillParts.Clear();
            this.drillInstalled = false;
            this.heatshieldParts.Clear();
            this.wheelBrakes.Clear();
            this.airBrakes.Clear();
            this.flaps.Clear();
            this.totalMass = 0.0;
            this.dryMass = 0.0;
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

         private void ScanPart(Part part)
         {
            foreach (PartResource r in part.Resources)
            {
               if ((r.info != null && Resources.ABLATOR != null && r.info.id == Resources.ABLATOR.id)                        // Stock
               || (r.info != null && Resources.ABLATIVE_SHIELDING != null && r.info.id == Resources.ABLATIVE_SHIELDING.id))  // Deadly Reentry
               {
                  if(!heatshieldParts.Contains(part))
                  {
                     heatshieldParts.Add(part);
                     this.heatshieldInstalled = true;
                  }
                  break;
               }
            }

            if (part.IsDrill())
            {
               if(!drillParts.Contains(part))
               {
                  drillParts.Add(part);
                  this.drillInstalled = true;
               }
            }

            // landing gears and brakes
            // not working at the moment
            Log.Test("PART "+part.name+" type "+part.GetType().Name);
            foreach (ModuleWheels.ModuleWheelBrakes brake in part.Modules.GetModules<ModuleWheels.ModuleWheelBrakes>())
            {
               wheelBrakes.Add(brake);
            }
            foreach (ModuleWheels.ModuleWheelDeployment deployment in part.Modules.GetModules<ModuleWheels.ModuleWheelDeployment>())
            {
               wheelDeployment.Add(deployment);
            }

            // not working at the moment
            foreach (ModuleControlSurface s in part.Modules.OfType<ModuleControlSurface>())
            {
               bool isAirBrake = false;
               // 
               if (s.ignoreRoll && s.ignoreYaw && s.ignorePitch)
               {
                  foreach (BaseAction a in s.Actions)
                  {
                     if (a.defaultActionGroup == KSPActionGroup.Brakes)
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


         protected override void PartUnpacked(Part part)
         {
            ScanPart(part);
         }

         protected void ScanVesselParts(Vessel vessel)
         {
            Reset();
            foreach (Part part in vessel.Parts)
            {
               if (!part.packed) ScanPart(part);
            }
            // not working at the moment
            //this.landingGearState = InspectLandingGear();
            //this.brakeState = InspectBrakes();
            //this.airBrakeState = InspectAirBrakes();
            //this.flapState = InspectFlaps();

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

         public double GetDryMass()
         {
            return dryMass;
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
            if (wheelDeployment.Count == 0) return GEARSTATES.NOT_INSTALLED;
            int deployedCnt = 0;
            foreach (ModuleWheels.ModuleWheelDeployment deployment in wheelDeployment)
            {
               double position = deployment.position;
               double rectracted = deployment.retractedPosition;
               double deployed = deployment.deployedPosition;
               if(position != rectracted && position != deployed) return GEARSTATES.DEPLOYING;
               if (position == deployed)
               {
                  deployedCnt++;
               }
            }
            if(deployedCnt == 0)
            {
               return GEARSTATES.RETRACTED;
            }
            if(deployedCnt < wheelBrakes.Count)
            {
               return GEARSTATES.PARTIAL_DEPLOYED;
            }
            return GEARSTATES.DEPLOYED;
         }

         private BRAKESTATES InspectBrakes()
         {
            if (wheelBrakes.Count == 0) return BRAKESTATES.NOT_INSTALLED;
            int engaged = 0;
            foreach (ModuleWheels.ModuleWheelBrakes brake in wheelBrakes)
            {
               if (brake.brakeInput > 0 && brake.brakeResponse>0)
               {
                  engaged++;
               }
            }

            if (engaged == 0)
            {
               return BRAKESTATES.NOT_ENGAGED;
            }
            if (engaged < (wheelBrakes.Count))
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
               dryMass = 0.0;
               heatshieldTemp = 0.0;
            }
            else
            {
               // mass
               totalMass = vessel.GetTotalMass();
               dryMass = vessel.GetDeltaV();
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
