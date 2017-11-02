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

         private readonly Color LIGHT_ON_YELLOW = new Color(250, 255, 0);
         private readonly Color LIGHT_OFF_YELLOW = new Color(62, 63, 0);

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
         }

         private void DrawIndicators()
         {
            DrawRectagle(7, 9, 18, 14, LIGHT_ON_YELLOW);
            //DrawRectagle(7, 24, 18, 14, LIGHT_OFF_YELLOW);
            //DrawRectagle(7, 39, 18, 14, LIGHT_ON_YELLOW);
            DrawRectagle(7, 54, 18, 14, LIGHT_OFF_YELLOW);
         }




         protected override void OnWindow(int id)
         {
            // background
            GUI.DrawTexture(skinBounds, BACKGROUND);
            // indicator lamps
            DrawIndicators();
            // skin
            GUI.DrawTexture(skinBounds, SKIN);


            // Mouseclicks
            Event e = Event.current;
            if (e.type == EventType.MouseDown && e.button == 0)
            {
               float x = e.mousePosition.x;
               float y = e.mousePosition.y;
               /*CheckIndicatorClick(x, y, BOUNDS_INDICATOR_STD, GaugeSet.ID.STANDARD);
               CheckIndicatorClick(x, y, BOUNDS_INDICATOR_LAU, GaugeSet.ID.LAUNCH);
               CheckIndicatorClick(x, y, BOUNDS_INDICATOR_LAN, GaugeSet.ID.LAND);
               CheckIndicatorClick(x, y, BOUNDS_INDICATOR_DCK, GaugeSet.ID.DOCK);
               CheckIndicatorClick(x, y, BOUNDS_INDICATOR_ORB, GaugeSet.ID.ORBIT);
               CheckIndicatorClick(x, y, BOUNDS_INDICATOR_FLT, GaugeSet.ID.FLIGHT);
               CheckIndicatorClick(x, y, BOUNDS_INDICATOR_S_1, GaugeSet.ID.SET1);
               CheckIndicatorClick(x, y, BOUNDS_INDICATOR_S_2, GaugeSet.ID.SET2);
               CheckIndicatorClick(x, y, BOUNDS_INDICATOR_S_3, GaugeSet.ID.SET3);*/
            }
         }


         private void CheckIndicatorClick(float x, float y, Rect bounds, GaugeSet.ID id)
         {
            if(x>=bounds.x && x<bounds.x+bounds.width && y>=bounds.y && y<bounds.y+bounds.height)
            {
               NanoGauges.configuration.SetGaugeSet(id);
               NanoGauges.gauges.ReflectGaugeSetChange();
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
         }
      }
   }
}
