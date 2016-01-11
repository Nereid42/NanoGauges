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

       public class TimeToTransistionGauge : AbstractTimeGauge
        {
            private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/TTRANS-skin");
            private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/TTRANS-scale");

            public TimeToTransistionGauge()
               : base(Constants.WINDOW_ID_GAUGE_TIMETOTRANS, SKIN, SCALE)
            {
            }

            protected override void AutomaticOnOff()
            {
               Vessel vessel = FlightGlobals.ActiveVessel;
               if (vessel != null && vessel.parts.Count > 0 && vessel.orbit != null
               && (vessel.situation == Vessel.Situations.FLYING || vessel.situation == Vessel.Situations.ORBITING || vessel.situation == Vessel.Situations.SUB_ORBITAL))
               {
                  On();
               }
               else
               {
                  Off();
               }
            }


            public override string GetName()
            {
                return "Time to\nTransition";
            }

            public override string GetDescription()
            {
                return "\n\n Remaining time to next transition.";
            }

            protected override double GetTime()
            {
               Vessel vessel = FlightGlobals.ActiveVessel;
               if(vessel == null) return double.NaN;
               if (vessel.situation != Vessel.Situations.ORBITING) return double.NaN;
               if (vessel.orbit == null) return double.NaN;
               //Log.Test("epoch "+vessel.orbit.epoch);
               //Log.Test("period " + vessel.orbit.period);
               double t1 = vessel.orbit.timeToTransition1;
               double t2 = vessel.orbit.timeToTransition2;
               if(t1<t2 && t1>0)
               {
                  return t1;
               }
               return t2;
            }

            public override string ToString()
            {
                return "Gauge:TRANSITION";
            }
        }
    }
}
