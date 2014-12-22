using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public abstract class AbstractResourceGauge : VerticalGauge
      {

         private readonly ResourceInspecteur inspecteur;
         private readonly PartResourceDefinition resource;

         public AbstractResourceGauge(int id, ResourceInspecteur inspecteur, PartResourceDefinition resource, Texture2D skin, Texture2D scale)
            : base(id, skin, scale)
         {
            this.inspecteur = inspecteur;
            this.resource = resource;
         }


         protected override void AutomaticOnOff()
         {
            if (resource!=null && FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.parts.Count > 0)
            {
               double capacity = inspecteur.GetCapacity(resource);
               if (capacity == 0.0)
               {
                  Off();
               }
               else
               {
                  On();
               }
            }
            else
            {
               Off();
            }
         }

         public PartResourceDefinition GetResource()
         {
            return resource;
         }

         protected override float GetScaleOffset()
         {
            float b = GetLowerOffset();
            float y = b;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null)
            {
               double remaining = inspecteur.GetAmount(resource);
               double capacity = inspecteur.GetCapacity(resource);
               
               float pct = capacity > 0 ? (float)(remaining / capacity) : 0.0f;
               if (pct >= 0)
               {
                  y = b + 300.0f * pct / (float)SCALE_HEIGHT;
               }
            }
            return y;
         }
      }
   }
}
