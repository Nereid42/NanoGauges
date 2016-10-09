using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {

      public class DistanceToTargetGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/DTGT-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/DTGT-scale");
         private const double MAX_DISTANCE = 2000000;

         public DistanceToTargetGauge()
            : base(Constants.WINDOW_ID_GAUGE_DTGT, SKIN, SCALE)
         {
         }

         public override string GetName()
         {
            return "Distance\nto Target";
         }

         public override string GetDescription()
         {
            return "\n\nDistance to a selected target";
         }

         protected override void AutomaticOnOff()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && FlightGlobals.ActiveVessel.parts.Count > 0)
            {
               if (vessel.targetObject != null)
               {
                  On();
                  return;
               }
            }
            Off();
         }


         protected override float GetScaleOffset()
         {
            float b = GetLowerOffset();
            float y = b;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null)
            {
               ITargetable targetable = vessel.targetObject;
               if(targetable!=null)
               {
                  Vessel target = targetable.GetVessel();
                  if(target!=null)
                  {
                     double d = Vector3.Distance(vessel.GetWorldPos3D(),target.GetWorldPos3D());
                     if (d > MAX_DISTANCE)
                     {
                        d = MAX_DISTANCE;
                        OutOfLimits();
                     }
                     else
                     {
                        InLimits();
                     }
                     if (d < 0) d = 0;
                     y = b + 69.75f * (float)Math.Log10(1 + d/100) / 400.0f;
                  }
               }
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:DTGT";
         }
      }
   }
}
