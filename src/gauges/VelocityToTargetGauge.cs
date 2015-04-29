using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {

      public class VelocityToTargetGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/VTGT-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/VTGT-scale");
         private static double MAX_SPEED = 10000;
         private static double MIN_SPEED = -10000;

         public VelocityToTargetGauge()
            : base(Constants.WINDOW_ID_GAUGE_VTGT, SKIN, SCALE)
         {
         }

         public override string GetName()
         {
            return "Velocity\n to Target";
         }

         public override string GetDescription()
         {
            return "\n\nRelative velocity to a selected target.";
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
            float m = GetCenterOffset();
            float y = m;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null)
            {
               ITargetable targetable = vessel.targetObject;
               if(targetable!=null)
               {
                  Vessel target = targetable.GetVessel();
                  if(target!=null)
                  {
                     Vector3d velocity = target.obt_velocity - vessel.obt_velocity;
                     Vector3d dir = vessel.CoM - target.CoM;
                     double angle = Vector3d.Angle(velocity, dir);
                     //Log.Test("ANGLE: " + angle);
                     double v = velocity.magnitude * (angle<90.0?1:-1);
                     if (v > MAX_SPEED)
                     {
                        v = MAX_SPEED;
                        OutOfLimits();
                     }
                     else if (v < MIN_SPEED)
                     {
                        v = MIN_SPEED;
                        OutOfLimits();
                     }
                     else
                     {
                        InLimits();
                     }

                     if (v >= 0)
                     {
                        y = m + 37.5f * (float)Math.Log10(1 + v) / (float)SCALE_HEIGHT;
                     }
                     else
                     {
                        y = m - 37.5f * (float)Math.Log10(1 - v) / (float)SCALE_HEIGHT;
                     }
                  }
               }
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:VTGT";
         }
      }
   }
}
