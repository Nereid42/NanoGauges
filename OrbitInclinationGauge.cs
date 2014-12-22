using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {

      public class OrbitInclinationGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/INCL-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/INCL-scale");
         private const double MAX_INCL = 90;

         public OrbitInclinationGauge()
            : base(Constants.WINDOW_ID_GAUGE_INCL, SKIN, SCALE)
         {
         }

         public override string GetName()
         {
            return "Orbital Inclination";
         }

         public override string GetDescription()
         {
            return "The inclination of an established orbit.";
         }

         protected override void AutomaticOnOff()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && FlightGlobals.ActiveVessel.parts.Count > 0)
            {
               if (vessel.orbit != null && vessel.orbit.PeA > 0)
               {
                  On();
               }
               else 
               {
                  Off();
               }
            }
            else
            {
               Off();
            }
         }

         protected override float GetScaleOffset()
         {
            float b = GetLowerOffset();
            float y = b;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null)
            {
               if (vessel.orbit != null && vessel.orbit.PeA>0)
               {
                  if (vessel.orbit.ApR > 0 && (vessel.orbit.ApR + vessel.orbit.PeR) != 0)
                  {
                     double incl = vessel.orbit.inclination;
                     if (incl > MAX_INCL) incl = MAX_INCL;
                     if (incl < 0) incl = 0;
                     y = b + 300.0f / 90.0f * (float)incl / 400.0f;
                  }
               }
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:INCL";
         }
      }
   }
}
