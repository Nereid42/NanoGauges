﻿/************************************************************************************
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

       public class TimeToApoapsisGauge : AbstractTimeGauge
        {
            private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/TAPA-skin");
            private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/TAPA-scale");

            public TimeToApoapsisGauge()
               : base(Constants.WINDOW_ID_GAUGE_TIMETOAPA, SKIN, SCALE)
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
                return "Time to\nApoapsis";
            }

            public override string GetDescription()
            {
                return "\n\n Remaining time to apoapsis.";
            }

            protected override double GetTime()
            {
               Vessel vessel = FlightGlobals.ActiveVessel;
               if(vessel == null) return double.NaN;
               if (vessel.orbit == null) return double.NaN;
               return vessel.orbit.timeToAp;
            }

            public override string ToString()
            {
                return "Gauge:APATIME";
            }
        }
    }
}
