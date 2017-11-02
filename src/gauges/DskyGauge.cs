using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {
      public class DskyGauge : AbstractClosableGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/DSKY-skin");
         private static readonly Texture2D BACKGROUND = Utils.GetTexture("Nereid/NanoGauges/Resource/DSKY-back");

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
            CreateButtons();
         }

         private void DrawIndicator(int y, bool state, Color on, Color off)
         {
            DrawRectagle(7, y, 18, 14, state?on:off);
         }

         private void DrawIndicators()
         {
            DrawIndicator(9, true, LIGHT_ON_YELLOW, LIGHT_OFF_YELLOW);
            DrawIndicator(24, true, LIGHT_ON_GREEN, LIGHT_OFF_GREEN);
            DrawIndicator(39, true, LIGHT_ON_YELLOW, LIGHT_OFF_YELLOW);
            DrawIndicator(54, true, LIGHT_ON_RED, LIGHT_OFF_RED);
         }

         private void DrawMode(float x, DISPLAY_MODE mode)
         {
            DrawRectagle(x, 76, 14, 13, this.mode == mode ? LIGHT_ON_GREEN : LIGHT_OFF_GREEN);
         }

         private void DrawDisplayMode()
         {
            DrawMode(BOUNDS_MODE_ORBIT.x,  DISPLAY_MODE.ORBIT);
            DrawMode(BOUNDS_MODE_VELOCITY.x, DISPLAY_MODE.VELOCITY);
            DrawMode(43, DISPLAY_MODE.RUNWAY);
            DrawMode(61, DISPLAY_MODE.GLIDE);
            DrawMode(79, DISPLAY_MODE.TRIM);
         }


         protected override void OnWindow(int id)
         {
            // background
            GUI.DrawTexture(skinBounds, BACKGROUND);
            // indicator lamps
            DrawIndicators();
            // moder
            DrawDisplayMode();
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
