using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nereid
{
   namespace NanoGauges
   {

      public class Damper
      {
         private float targetValue;
         private float value;
         private float damp;
         private bool enabled = true;

         private readonly float lower;
         private readonly float upper;

         private bool damping;

         public Damper(float damp, float lower = 0.0f, float upper = 1.0f)
         {
            this.damp = damp;
            this.lower = lower;
            this.upper = upper;
            this.damping = false;
            this.targetValue = lower;
            this.value = lower;
         }

         public bool IsInLimits()
         {
            return value >= lower && value <= upper;
         }

         public void SetEnabled(bool enabled)
         {
            this.enabled = enabled;
         }

         public void SetValue(float value)
         {
            this.value = value;
            this.targetValue = value;
            this.damping = false;
         }

         public void Reset()
         {
            this.value = 0.0f;
            this.targetValue = 0.0f;
         }

         public bool IsDamping()
         {
            return damping;
         }

         public float Get()
         {
            if(enabled)
            {
               float d = Math.Sign(value - targetValue) * damp;
               if (value < targetValue)
               {
                  if (value + damp < targetValue)
                  {
                     value = value + damp;
                     this.damping = true;
                  }
                  else
                  {
                     value = targetValue;
                     this.damping = false;
                  }
               }
               else if (value > targetValue)
               {
                  if (value - damp > targetValue)
                  {
                     value = value - damp;
                     this.damping = true;
                  }
                  else
                  {
                     value = targetValue;
                     this.damping = false;
                  }
               }
               return value;
            }

            return targetValue;
         }

         public void Set(float value)
         {
            // just to be safe
            if (value == float.NaN)
            {
               if (Log.IsLogable(Log.LEVEL.DETAIL)) Log.Detail("WARNING: invalid damper value");
               this.targetValue = lower;
            }
            else
            {
               this.targetValue = value;
               if (this.targetValue == this.value)
               {
                  this.damping = false;
               }
               else
               {
                  this.damping = true;
               }
            }
         }

         public override string ToString()
         {
            return "damper: value="+value+", target value="+targetValue+", damp="+damp+", enabled="+enabled+", lower="+lower+", upper="+upper;
         }
      }
   }
}