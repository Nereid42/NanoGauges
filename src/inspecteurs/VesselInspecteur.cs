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
         private static readonly double MIN_INTERVAL = 0.20;

         private double totalMass = 0;
         private double dragCoefficent = 0;
         private double heatshieldTemp = 0;
         private bool heatshieldInstalled = false;


         private readonly List<Part> heatshieldParts = new List<Part>();


         public VesselInspecteur()
            : base(10, MIN_INTERVAL)
         {
            Reset();
         }


         public override void Reset()
         {
            this.heatshieldParts.Clear();
            this.totalMass = 0.0;
            this.dragCoefficent = 0.0;
            this.heatshieldTemp = 0.0;
            this.heatshieldInstalled = false;
         }

         public double GetDragCoefficent()
         {
            return this.dragCoefficent;
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

               this.heatshieldParts.Clear();
               this.heatshieldInstalled = false;
               if (vessel == null || vessel.Parts == null) return;
               foreach (Part part in vessel.Parts)
               {
                  if (part.packed) part.Unpack();
                  foreach (PartResource r in part.Resources)
                  {
                     if (r.info!=null && Resources.ABLATIVE_SHIELDING!=null && r.info.id==Resources.ABLATIVE_SHIELDING.id)
                     {
                        heatshieldParts.Add(part);
                        this.heatshieldInstalled = true;
                        break;
                     }
                  }
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

         protected override void Inspect(Vessel vessel)
         {
            if(vessel==null)
            {
               totalMass = 0.0;
               heatshieldTemp = 0.0;
            }
            else
            {
               totalMass = vessel.GetTotalMass();
               heatshieldTemp = Constants.MIN_TEMP;
               foreach(Part p in heatshieldParts)
               {
                  if(p.temperature > heatshieldTemp)
                  {
                     heatshieldTemp = p.temperature;
                  }
               }
            }
         }
      }
   }
}
