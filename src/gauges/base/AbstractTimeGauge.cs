/************************************************************************************
 * 
 * Nanogauges - by Nereid
 * 
 * 
 * 
 * - this gauge is based on an idea by daid
 * - special thanks to cybutek who has granted the permission to use part of his
 *   code from the "Engineer Redux" plugin
 * 
 * 
 ************************************************************************************/

using System;
using UnityEngine;


namespace Nereid
{
    namespace NanoGauges
    {

        public abstract class AbstractTimeGauge : VerticalGauge
        {

            private const float NO_TIME = -1;
            private const float MAX_TIME = 2 * Constants.SECONDS_PER_HOUR; //2 hours is the maximum shown for the scale

            public AbstractTimeGauge(int id, Texture2D skin, Texture2D scale)
               : base(id, skin, scale, true, 0.0005f)
            {
            }

            protected abstract double GetTime();

            protected override float GetScaleOffset()
            {
                float lower = GetLowerOffset();
                float upper = GetUpperOffset();

               Vessel vessel = FlightGlobals.ActiveVessel;

               if(vessel == null) return upper;

               if(vessel.orbit == null) return upper;

               double time = GetTime();


               if (double.IsNaN(time) || time == NO_TIME || time >= MAX_TIME)
                {
                    OutOfLimits();
                    return upper;
                }
                InLimits();
                return lower + 75.0f * (float)Math.Log10(1.0 + time) / 400.0f;
            }

        }
    }
}
