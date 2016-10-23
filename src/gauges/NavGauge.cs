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

            float bearing = Utils.InitialBearingFromTo(vessel.longitude, vessel.latitude, Constants.COORDS_RUNWAY_SPACE_CENTER.longitude, Constants.COORDS_RUNWAY_SPACE_CENTER.latitude);

            if(bearing<0)
            {
               bearing = 360 - bearing;
            }

            SetBlueNeedleTo(bearing);

            return hdg; 
         }

      }
   }
}
