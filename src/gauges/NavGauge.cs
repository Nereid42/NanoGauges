using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {
      class NavGauge : AbstractCompassGauge        
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/NAV-skin");

         public NavGauge()
            : base(Constants.WINDOW_ID_GAUGE_NAV, SKIN)
         {
            Absolut();
            EnableBlueNeedle();
            EnableYellowNeedle();
         }

         public override string GetName()
         {
            return "VOR/ILS";
         }

         public override string GetDescription()
         {
            return "Navigation and ILS.";
         }

         private bool IsOrbitCamera(FlightCamera camera)
         {
            if (camera.mode == FlightCamera.Modes.ORBITAL) return true;
            if(camera.mode == FlightCamera.Modes.AUTO)
            {
               if (camera.autoMode == FlightCamera.Modes.ORBITAL) return true;
            }
            return false;
         }

         private bool IsRelativeCamera(FlightCamera camera)
         {
            if (camera.mode == FlightCamera.Modes.CHASE || camera.mode == FlightCamera.Modes.LOCKED) return true;
            if (camera.mode == FlightCamera.Modes.AUTO)
            {
               if (camera.autoMode == FlightCamera.Modes.CHASE || camera.autoMode == FlightCamera.Modes.LOCKED) return true;
            }
            return false;
         }

         protected override void AjustValues()
         {
            base.AjustValues();
            SetBlueNeedleTo(NavGlobals.bearingToRunway);
            SetYellowNeedleTo(NavGlobals.horizontalGlideslopeDeviation/8.0);
         }

         protected override float GetDegrees()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;

            if (vessel.mainBody == null || vessel == null || !vessel.mainBody.isHomeWorld)
            {
               Off();
               return 0;
            }

            if(vessel.altitude > vessel.mainBody.Radius/2)
            {
               OutOfLimits();
            }
            else
            {
               InLimits();
            }

            
            On();

            float hdg = FlightGlobals.ship_heading;

            return hdg; 
         }

      }
   }
}
