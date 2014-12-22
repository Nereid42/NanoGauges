using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {

      public class OrbitGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/ORBIT-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/ORBIT-scale");

         public OrbitGauge()
            : base(Constants.WINDOW_ID_GAUGE_ORBIT, SKIN, SCALE)
         {
         }

         public override string GetName()
         {
            return "Orbital Excentricy";
         }

         public override string GetDescription()
         {
            return "Excentricy of the established orbit. A value of 0 is a perfect circle. You will get higher values for a more eliptic orbit.";
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
            float y = GetUpperOffset();
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null)
            {
               if (vessel.orbit != null && vessel.orbit.PeA > 0)
               {
                  if (vessel.orbit.ApR > 0 && (vessel.orbit.ApR + vessel.orbit.PeR) != 0)
                  {
                     double ex = (vessel.orbit.ApR - vessel.orbit.PeR) / (vessel.orbit.ApR + vessel.orbit.PeR);
                     if (ex >= 0)
                     {
                        y = b + 149.5f * (float)Math.Log10(1 + 100 * ex) / 400.0f;
                     }
                  }
               }
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:ORBIT";
         }
      }
   }
}
