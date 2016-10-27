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
         private const float BUTTON_SWITCH_WIDTH = 17;
         private const float BUTTON_SWITCH_HEIGHT = 8;
         private readonly Button buttonSwitch;

         // Nav flags
         private static Texture2D FLAG_SC_TEXTURE = Utils.GetTexture("Nereid/NanoGauges/Resource/SC-flag");
         private static Texture2D FLAG_OA_TEXTURE = Utils.GetTexture("Nereid/NanoGauges/Resource/OA-flag");
         Flag navScFlag;
         Flag navOaFlag;

         public NavGauge()
            : base(Constants.WINDOW_ID_GAUGE_NAV, SKIN)
         {
            float scale = (float)NanoGauges.configuration.gaugeScaling;
            this.buttonSwitch = new Button(this, SKIN_SWITCH_OFF, SKIN_SWITCH_ON, BUTTON_SWITCH_WIDTH * scale, BUTTON_SWITCH_HEIGHT * scale);

            this.navScFlag = new Flag(this, FLAG_SC_TEXTURE);
            this.navOaFlag = new Flag(this, FLAG_OA_TEXTURE);

            navScFlag.Down();
            navOaFlag.Down();

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

            if (NavGlobals.InBeam || IsOff() || NavGlobals.destinationAirfield == null)
            {
               InLimits();
            }
            else
            {
               NotInLimits();
            }

            if (NavGlobals.landingRunway != null && IsOn() && IsInLimits())
            {
               SetBlueNeedleTo(NavGlobals.bearingToRunway);
               SetYellowNeedleTo(NavGlobals.horizontalGlideslopeDeviation * 10);
            }
            else
            {
               SetBlueNeedleTo(180);
               SetYellowNeedleTo(-180);
            }
         }

         protected override void DrawFlags()
         {
            // choose flag for navigation
            navScFlag.Set(NavGlobals.destinationAirfield == NavGlobals.AIRFIELD_SPACECENTER);
            navOaFlag.Set(NavGlobals.destinationAirfield == NavGlobals.AIRFIELD_OLDAIRFIELD);
            //
            // draw current state of flags (on/off and limiter)
            // increment animation step on each draw (flags will not show up immediately)
            float scaling = (float)NanoGauges.configuration.gaugeScaling;
            navScFlag.Draw(14*scaling, 0);
            navOaFlag.Draw(31*scaling, 0);
         }

         protected override void DrawOverlay()
         {
            int w = GetWidth();
            int h = GetHeight();
            int margin = w / 20;
            float x0 = w - buttonSwitch.GetWidth() - margin;
            float y = 4;

            if(buttonSwitch.Draw(x0, y))
            {
               NavGlobals.SelectNextAirfield();
            }
         }

         protected override float GetDegrees()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;

            if (vessel.mainBody == null || vessel == null || !vessel.mainBody.isHomeWorld || vessel.altitude > vessel.mainBody.Radius / 2)
            {
               Off();
               return 0;
            }

            On();

            float hdg = FlightGlobals.ship_heading;

            return hdg; 
         }

         public override void OnGaugeScalingChanged()
         {
            base.OnGaugeScalingChanged();
            float scaling = (float)NanoGauges.configuration.gaugeScaling;
            buttonSwitch.SetWidth(BUTTON_SWITCH_WIDTH * scaling);
            buttonSwitch.SetHeight(BUTTON_SWITCH_HEIGHT * scaling);
            navScFlag.ScaleTo(scaling);
            navOaFlag.ScaleTo(scaling);
         }
      }
   }
}
