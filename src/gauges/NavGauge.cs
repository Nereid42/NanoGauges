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
         private static Texture2D FLAG_KL_TEXTURE = Utils.GetTexture("Nereid/NanoGauges/Resource/KL-flag");
         private static Texture2D FLAG_BK_TEXTURE = Utils.GetTexture("Nereid/NanoGauges/Resource/BK-flag");
         private static Texture2D FLAG_CR_TEXTURE = Utils.GetTexture("Nereid/NanoGauges/Resource/Cr-flag");
         Flag navScFlag;
         Flag navOaFlag;
         Flag navKlFlag;
         Flag navBkFlag;
         // ID flags
         private static Texture2D FLAG_0_TEXTURE = Utils.GetTexture("Nereid/NanoGauges/Resource/0-flag");
         private static Texture2D FLAG_1_TEXTURE = Utils.GetTexture("Nereid/NanoGauges/Resource/1-flag");
         private static Texture2D FLAG_2_TEXTURE = Utils.GetTexture("Nereid/NanoGauges/Resource/2-flag");
         private static Texture2D FLAG_3_TEXTURE = Utils.GetTexture("Nereid/NanoGauges/Resource/3-flag");
         private static Texture2D FLAG_4_TEXTURE = Utils.GetTexture("Nereid/NanoGauges/Resource/4-flag");
         private static Texture2D FLAG_5_TEXTURE = Utils.GetTexture("Nereid/NanoGauges/Resource/5-flag");
         private static Texture2D FLAG_6_TEXTURE = Utils.GetTexture("Nereid/NanoGauges/Resource/6-flag");
         private static Texture2D FLAG_7_TEXTURE = Utils.GetTexture("Nereid/NanoGauges/Resource/7-flag");
         private static Texture2D FLAG_8_TEXTURE = Utils.GetTexture("Nereid/NanoGauges/Resource/8-flag");
         private static Texture2D FLAG_9_TEXTURE = Utils.GetTexture("Nereid/NanoGauges/Resource/9-flag");
         Flag nav00Flag;
         Flag nav10Flag;
         Flag nav20Flag;
         Flag nav0Flag;
         Flag nav1Flag;
         Flag nav2Flag;
         Flag nav3Flag;
         Flag nav4Flag;
         Flag nav5Flag;
         Flag nav6Flag;
         Flag nav7Flag;
         Flag nav8Flag;
         Flag nav9Flag;
         // positions
         private const int FLAG_X10 = 14;
         private const int FLAG_X01 = 24;

         public NavGauge()
            : base(Constants.WINDOW_ID_GAUGE_NAV, SKIN)
         {
            float scale = (float)NanoGauges.configuration.gaugeScaling;
            this.buttonSwitch = new Button(this, SKIN_SWITCH_OFF, SKIN_SWITCH_ON, BUTTON_SWITCH_WIDTH * scale, BUTTON_SWITCH_HEIGHT * scale);

            this.navScFlag = new Flag(this, FLAG_SC_TEXTURE);
            this.navOaFlag = new Flag(this, FLAG_OA_TEXTURE);
            this.navKlFlag = new Flag(this, FLAG_KL_TEXTURE);
            this.navBkFlag = new Flag(this, FLAG_BK_TEXTURE);
            this.nav0Flag = new Flag(this, FLAG_0_TEXTURE);
            this.nav1Flag = new Flag(this, FLAG_1_TEXTURE);
            this.nav2Flag = new Flag(this, FLAG_2_TEXTURE);
            this.nav3Flag = new Flag(this, FLAG_3_TEXTURE);
            this.nav4Flag = new Flag(this, FLAG_4_TEXTURE);
            this.nav5Flag = new Flag(this, FLAG_5_TEXTURE);
            this.nav6Flag = new Flag(this, FLAG_6_TEXTURE);
            this.nav7Flag = new Flag(this, FLAG_7_TEXTURE);
            this.nav8Flag = new Flag(this, FLAG_8_TEXTURE);
            this.nav9Flag = new Flag(this, FLAG_9_TEXTURE);
            this.nav00Flag = new Flag(this, FLAG_0_TEXTURE);
            this.nav10Flag = new Flag(this, FLAG_1_TEXTURE);
            this.nav20Flag = new Flag(this, FLAG_2_TEXTURE);

            navScFlag.Down();
            navOaFlag.Down();
            navKlFlag.Down();
            navBkFlag.Down();

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

            if(IsOn())
            {
               // gauge is on
               if (NavGlobals.landingRunway != null)
               {
                  // we have a runway for landing
                  SetBlueNeedleTo(NavGlobals.bearingToRunway);
                  if (IsInLimits())
                  {
                     SetYellowNeedleTo(NavGlobals.horizontalGlideslopeDeviation * 10);
                  }
                  else
                  {
                     SetYellowNeedleTo(-999);
                  }
               }
               else
               {
                  // we do not have a runway for landing
                  SetYellowNeedleTo(-999);
                  //
                  // bearing to airport
                  if (NavGlobals.destinationAirfield != null)
                  {
                     SetBlueNeedleTo(NavGlobals.bearingToAirfield);
                  }
                  else
                  {
                     // no airport, no bearing
                     SetBlueNeedleTo(999);
                  }
               }
            }
            else
            {
               // gauge is off
               SetYellowNeedleTo(-999);
               SetBlueNeedleTo(999);
            }
         }

         protected override void DrawFlags()
         {
            // choose flag for navigation
            navScFlag.Set(NavGlobals.destinationAirfield == NavGlobals.AIRFIELD_SPACECENTER);
            navOaFlag.Set(NavGlobals.destinationAirfield == NavGlobals.AIRFIELD_OLDAIRFIELD);
            navKlFlag.Set(NavGlobals.destinationAirfield == NavGlobals.AIRFIELD_KERMAN_LAKE);
            navBkFlag.Set(NavGlobals.destinationAirfield == NavGlobals.AIRFIELD_BLACK_KRAGS);

            bool hide_id_flags = NavGlobals.destinationAirfield==null || navScFlag.IsDown() || navOaFlag.IsDown() || navKlFlag.IsDown() || navBkFlag.IsDown();

            // set ID flags
            int id = NavGlobals.destinationAirfield != null ? NavGlobals.destinationAirfield.id : 0;
            nav00Flag.Set( !hide_id_flags && id < 10 );
            nav10Flag.Set( !hide_id_flags && id > 9 && id < 20 );
            nav20Flag.Set( !hide_id_flags && id > 19 && id < 30 );
            nav0Flag.Set(  !hide_id_flags && id % 10 == 0 );
            nav1Flag.Set(  !hide_id_flags && id % 10 == 1 );
            nav2Flag.Set(  !hide_id_flags && id % 10 == 2 );
            nav3Flag.Set(  !hide_id_flags && id % 10 == 3 );
            nav4Flag.Set(  !hide_id_flags && id % 10 == 4 );
            nav5Flag.Set(  !hide_id_flags && id % 10 == 5 );
            nav6Flag.Set(  !hide_id_flags && id % 10 == 6 );
            nav7Flag.Set(  !hide_id_flags && id % 10 == 7 );
            nav8Flag.Set(  !hide_id_flags && id % 10 == 8 );
            nav9Flag.Set(  !hide_id_flags && id % 10 == 9 );
            //
            // draw current state of flags (on/off and limiter)
            // increment animation step on each draw (flags will not show up immediately)
            float scaling = (float)NanoGauges.configuration.gaugeScaling;
            navScFlag.Draw(14*scaling, 0);
            navOaFlag.Draw(31*scaling, 0);
            navKlFlag.Draw(68*scaling, 0);
            navBkFlag.Draw(84*scaling, 0);
            //
            // ID flags
            nav00Flag.Draw(FLAG_X10 * scaling, 0);
            nav10Flag.Draw(FLAG_X10 * scaling, 0);
            nav20Flag.Draw(FLAG_X10 * scaling, 0);
            nav0Flag.Draw(FLAG_X01 * scaling, 0);
            nav1Flag.Draw(FLAG_X01 * scaling, 0);
            nav2Flag.Draw(FLAG_X01 * scaling, 0);
            nav3Flag.Draw(FLAG_X01 * scaling, 0);
            nav4Flag.Draw(FLAG_X01 * scaling, 0);
            nav5Flag.Draw(FLAG_X01 * scaling, 0);
            nav6Flag.Draw(FLAG_X01 * scaling, 0);
            nav7Flag.Draw(FLAG_X01 * scaling, 0);
            nav8Flag.Draw(FLAG_X01 * scaling, 0);
            nav9Flag.Draw(FLAG_X01 * scaling, 0);

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

            if (vessel == null || vessel.isEVA || vessel.mainBody == null || !vessel.mainBody.isHomeWorld || vessel.altitude > vessel.mainBody.Radius / 2)
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
