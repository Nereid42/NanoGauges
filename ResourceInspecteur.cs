using System;
using UnityEngine;
using System.Collections.Generic;

namespace Nereid
{
   namespace NanoGauges
   {
      public class ResourceInspecteur : Inspecteur
      {
         private static readonly double MIN_INTERVAL = 0.2;

         private Dictionary<int, double> previous = new Dictionary<int, double>();
         private Dictionary<int, double> amount = new Dictionary<int, double>();
         private Dictionary<int, double> capacity = new Dictionary<int, double>();
         private Dictionary<int, double> rate = new Dictionary<int, double>();

         private readonly List<PartResource> resourceParts = new List<PartResource>();

         public ResourceInspecteur()
            : base(10, MIN_INTERVAL)
         {
            Reset();
         }

         public override void Reset()
         {
            resourceParts.Clear();
            previous.Clear();
            amount.Clear();
            rate.Clear();
            capacity.Clear();
         }

         protected override void ScanVessel(Vessel vessel)
         {
            resourceParts.Clear();
            if (vessel == null || vessel.Parts == null) return;
            foreach (Part part in vessel.Parts)
            {
               if (part.packed) part.Unpack();
               foreach (PartResource r in part.Resources)
               {
                  resourceParts.Add(r);
               }
            }
         }

         public double GetCapacity(PartResourceDefinition resource)
         {
            if(!capacity.ContainsKey(resource.id))
            {
               this.capacity.Add(resource.id, 0.0);
            }
            return capacity[resource.id];
         }

         public double GetAmount(PartResourceDefinition resource)
         {
            if (!amount.ContainsKey(resource.id))
            {
               this.amount.Add(resource.id, 0.0);
            }
            return amount[resource.id];
         }

         public double GetRate(PartResourceDefinition resource)
         {
            if (!rate.ContainsKey(resource.id))
            {
               this.rate.Add(resource.id, 0.0);
            }
            return rate[resource.id];
         }



         protected override void Inspect(Vessel vessel)
         {
            double now = Planetarium.GetUniversalTime();

            foreach (PartResourceDefinition item in PartResourceLibrary.Instance.resourceDefinitions)
            {
               previous[item.id] = GetAmount(item);
            }

            this.amount.Clear();
            this.capacity.Clear();
            foreach (PartResource r in resourceParts)
            {
               int id = r.info.id;
               double amount = GetAmount(r.info);
               double capacity = GetCapacity(r.info);
               amount += r.amount;
               capacity += r.maxAmount;
               this.amount[id] = amount;
               this.capacity[id] = capacity;
            }
            //
            if (GetLastInspectTime() > 0)
            {
               double time = now - GetLastInspectTime();
               foreach (PartResourceDefinition item in PartResourceLibrary.Instance.resourceDefinitions)
               {
                  int id = item.id;
                  if(amount.ContainsKey(id) && previous.ContainsKey(id))
                  {
                     rate[id] = (1 / time) * (this.amount[id] - this.previous[id]);
                  }
                  else
                  {
                     rate[id] = 0.0;
                  }
               }

            }
         }

      }
   }
}
