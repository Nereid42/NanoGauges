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

         private static readonly Texture2D SKIN_SWITCH_OFF = Utils.GetTexture("Nereid/NanoGauges/Resource/SWITCH-left");
         private static readonly Texture2D SKIN_SWITCH_ON = Utils.GetTexture("Nereid/NanoGauges/Resource/SWITCH-right");

         // Buttons
         private const int BUTTON_SWITCH_WIDTH = 17;
         private const int BUTTON_SWITCH_HEIGHT = 8;
         private readonly Button buttonSwitch;


         public NavGauge()
            : base(Constants.WINDOW_ID_GAUGE_NAV, SKIN)
         {
            this.buttonSwitch = new Button(this, SKIN_SWITCH_OFF, SKIN_SWITCH_ON, BUTTON_SWITCH_WIDTH, BUTTON_SWITCH_HEIGHT);

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

         protected override void DrawOverlay()
         {
            int w = GetWidth();
            int h = GetHeight();
            int margin = w / 20;
            float x0 = w - buttonSwitch.GetWidth() - margin;
            float y = 4;

            buttonSwitch.Draw(x0, y);
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
