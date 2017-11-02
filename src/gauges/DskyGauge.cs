using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {
      public class DskyGauge : AbstractClosableGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/DSKY-skin");
         private static readonly Texture2D BACKGROUND = Utils.GetTexture("Nereid/NanoGauges/Resource/DIGIT-background");

         private Rect skinBounds = new Rect(0, 0, NanoGauges.configuration.verticalGaugeWidth, NanoGauges.configuration.verticalGaugeHeight);

         private readonly Color LIGHT_ON_YELLOW = new Color(0.98f, 1.0f, 0.0f);
         private readonly Color LIGHT_OFF_YELLOW = new Color(0.24f, 0.25f, 0.0f);
         private readonly Color LIGHT_ON_GREEN = new Color(0.24f, 1.0f, 0.0f);
         private readonly Color LIGHT_OFF_GREEN = new Color(0.0f, 0.24f, 0.0f);
         private readonly Color LIGHT_ON_RED = new Color(1.0f, 0.24f, 0.0f);
         private readonly Color LIGHT_OFF_RED = new Color(0.42f, 0.21f, 0.10f);

         // bound for mode buttons
         private static Rect BOUNDS_MODE_ORBIT;
         private static Rect BOUNDS_MODE_VELOCITY;
         private static Rect BOUNDS_MODE_RUNWAY;
         private static Rect BOUNDS_MODE_GLIDE;
         private static Rect BOUNDS_MODE_TRIM;

         enum DISPLAY_MODE { ORBIT, VELOCITY, RUNWAY, GLIDE, TRIM }
         private DISPLAY_MODE mode = DISPLAY_MODE.ORBIT;

         const int DIGITS = 5;
         private DigitalDisplay digital1;
         private DigitalDisplay digital2;
         private DigitalDisplay digital3;

         private static Rect ButtonRect(int left)
         {
            float gaugeScale = (float)NanoGauges.configuration.gaugeScaling;
            return new Rect(left * gaugeScale, 76 * gaugeScale, 14 * gaugeScale, 13 * gaugeScale);
         }

         public override sealed int GetWidth()
         {
            return NanoGauges.configuration.verticalGaugeHeight;
         }

         public override sealed int GetHeight()
         {
            return NanoGauges.configuration.verticalGaugeHeight;
         }

         public override string GetName()
         {
            return "DSKY";
         }

         public override string GetDescription()
         {
            return "DSKY gauge. Shows orbital, velocity, runway, ils and trim information.";
         }

         public override void Reset()
         {
            // nothing todo
         }

         public DskyGauge()
            : base(Constants.WINDOW_ID_GAUGE_DSKY)
         {
            this.digital1 = new DigitalDisplay(this, DIGITS);
            this.digital2 = new DigitalDisplay(this, DIGITS);
            this.digital3 = new DigitalDisplay(this, DIGITS);
            CreateButtons();
         }

         private void DrawIndicator(float y, bool state, Color on, Color off)
         {
            float scale = (float)NanoGauges.configuration.gaugeScaling;
            DrawRectagle(7.0f * scale, y * scale, 18 * scale, 14 * scale, state?on:off);
         }

         private void DrawIndicators()
         {
            DrawIndicator( 9, true, LIGHT_ON_YELLOW, LIGHT_OFF_YELLOW);
            DrawIndicator(24, true, LIGHT_ON_GREEN, LIGHT_OFF_GREEN);
            DrawIndicator(39, true, LIGHT_ON_YELLOW, LIGHT_OFF_YELLOW);
            DrawIndicator(54, true, LIGHT_ON_RED, LIGHT_OFF_RED);
         }

         private void DrawMode(Rect r, DISPLAY_MODE mode)
         {
            DrawRectagle(r.x, r.y, r.width, r.height, this.mode == mode ? LIGHT_ON_GREEN : LIGHT_OFF_GREEN);
         }

         private void DrawDisplayMode()
         {
            DrawMode(BOUNDS_MODE_ORBIT,  DISPLAY_MODE.ORBIT);
            DrawMode(BOUNDS_MODE_VELOCITY, DISPLAY_MODE.VELOCITY);
            DrawMode(BOUNDS_MODE_RUNWAY, DISPLAY_MODE.RUNWAY);
            DrawMode(BOUNDS_MODE_GLIDE, DISPLAY_MODE.GLIDE);
            DrawMode(BOUNDS_MODE_TRIM, DISPLAY_MODE.TRIM);
         }

         // draw digital value with n digits (rightbound)
         private void DrawDigitals()
         {
            float scale = (float)NanoGauges.configuration.gaugeScaling;

            digital1.Draw(31 * scale, 7 * scale);
            digital2.Draw(31 * scale, 27 * scale);
            digital3.Draw(31 * scale, 47 * scale);
         }

         private int Bounds(double value, int min, int max)
         {
            if (value < min) value = min;
            if (value > max) value = max;
            return (int)value;
         }

         private void SetDigitals()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            switch (mode)
            {
               case DISPLAY_MODE.ORBIT:
                  int apa = 0;
                  int pea = 0;
                  int inc = 0;
                  if(vessel!=null)
                  {
                     Orbit orbit = vessel.orbit;
                     apa = Bounds( (orbit.ApA+500) / 1000,  0, 99999  );
                     pea = Bounds( (orbit.PeA+500) / 1000,  0, 99999  );
                     inc = Bounds( (orbit.inclination+0.5), 0, 99999 );
                  }
                  digital1.SetValue(apa);
                  digital2.SetValue(pea);
                  digital3.SetValue(inc);
                  break;
               case DISPLAY_MODE.VELOCITY:
                  digital1.SetValue(-900);
                  digital2.SetValue(-90);
                  digital3.SetValue(-5000);
                  break;
               default:
                  digital1.SetValue(88888);
                  digital2.SetValue(88888);
                  digital3.SetValue(88888);
                  break;
            }
         }

         private void SetIndicators()
         {

         }

         protected override void OnWindow(int id)
         {
            // set display
            SetDigitals();
            SetIndicators();

            // background
            GUI.DrawTexture(skinBounds, BACKGROUND);
            // indicator lamps
            DrawIndicators();
            // moder
            DrawDisplayMode();
            // digitals
            DrawDigitals();
            // skin
            GUI.DrawTexture(skinBounds, SKIN);


            // Mouseclicks
            Event e = Event.current;
            if (e.type == EventType.MouseDown && e.button == 0)
            {
               float x = e.mousePosition.x;
               float y = e.mousePosition.y;
               CheckModeClick(x, y, BOUNDS_MODE_ORBIT, DISPLAY_MODE.ORBIT);
               CheckModeClick(x, y, BOUNDS_MODE_VELOCITY, DISPLAY_MODE.VELOCITY);
               CheckModeClick(x, y, BOUNDS_MODE_RUNWAY, DISPLAY_MODE.RUNWAY);
               CheckModeClick(x, y, BOUNDS_MODE_GLIDE, DISPLAY_MODE.GLIDE);
               CheckModeClick(x, y, BOUNDS_MODE_TRIM, DISPLAY_MODE.TRIM);
            }
         }


         private void CheckModeClick(float x, float y, Rect bounds, DISPLAY_MODE mode)
         {
            if(x>=bounds.x && x<bounds.x+bounds.width && y>=bounds.y && y<bounds.y+bounds.height)
            {
               this.mode = mode;
            }
         }

         public override void On()
         {
         }

         public override void Off()
         {
         }

         public override bool IsOn()
         {
            return true;
         }

         public override void InLimits()
         {
         }

         public override void NotInLimits()
         {
         }

         public override bool IsInLimits()
         {
            return true;
         }

         private void CreateButtons()
         {
            BOUNDS_MODE_ORBIT = ButtonRect(7);
            BOUNDS_MODE_VELOCITY = ButtonRect(25);
            BOUNDS_MODE_RUNWAY = ButtonRect(43);
            BOUNDS_MODE_GLIDE = ButtonRect(61);
            BOUNDS_MODE_TRIM = ButtonRect(79);
         }

         public override void OnGaugeScalingChanged()
         {
            // change dimensions of window
            //
            // rectangular, so width = height
            bounds.width = NanoGauges.configuration.verticalGaugeHeight;
            bounds.height = NanoGauges.configuration.verticalGaugeHeight;
            //
            //change dimensions of skin
            skinBounds.width = NanoGauges.configuration.verticalGaugeHeight;
            skinBounds.height = NanoGauges.configuration.verticalGaugeHeight;
            //
            CreateButtons();
         }
      }
   }
}
