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
         private Boolean enabled = true;

         private readonly float lower;
         private readonly float upper;

         public Damper(float damp, float lower = 0.0f, float upper = 1.0f)
         {
            this.damp = damp;
            this.lower = lower;
            this.upper = upper;
         }

         public bool IsInLimits()
         {
            return value >= lower && value <= upper;
         }

         public void SetEnabled(bool enabled)
         {
            this.enabled = enabled;
         }

         public void Reset()
         {
            this.value = 0.0f;
            this.targetValue = 0.0f;
         }

         public float GetValue()
         {
            if(enabled)
            {
               float d = Math.Sign(value - targetValue) * damp;
               if (value < targetValue)
               {
                  if (value + damp < targetValue)
                  {
                     value = value + damp;
                  }
                  else
                  {
                     value = targetValue;
                  }
               }
               else if (value > targetValue)
               {
                  if (value - damp > targetValue)
                  {
                     value = value - damp;
                  }
                  else
                  {
                     value = targetValue;
                  }
               }
               return value;
            }

            return targetValue;
         }

         public void SetValue(float value)
         {
            // just to be safe
            if (value == float.NaN)
            {
               if (Log.IsLogable(Log.LEVEL.DETAIL)) Log.Detail("WARNING: invalid damper value");
               targetValue = lower;
            }
            else
            {
               targetValue = value;
            }
         }

         public override string ToString()
         {
            return "damper: value="+value+", target value="+targetValue+", damp="+damp+", enabled="+enabled+", lower="+lower+", upper="+upper;
         }
      }
   }
}