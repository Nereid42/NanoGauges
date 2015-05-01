using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {
      public class SelectorGauge : AbstractClosableGauge
      {
         private static readonly Texture2D INDICATOR__ON_SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/INDICATOR-on");

         private static readonly Rect BOUNDS_INDICATOR_STD = ButtonRect( 6 ); // new Rect(5, 6, 9, 9);
         private static readonly Rect BOUNDS_INDICATOR_LAU = ButtonRect(16); //  new Rect(5, 16, 9, 9);
         private static readonly Rect BOUNDS_INDICATOR_LAN = ButtonRect(26); //  new Rect(5, 26, 9, 9);
         private static readonly Rect BOUNDS_INDICATOR_DCK = ButtonRect(36); //  new Rect(5, 36, 9, 9);
         private static readonly Rect BOUNDS_INDICATOR_ORB = ButtonRect(46); //  new Rect(5, 46, 9, 9);
         private static readonly Rect BOUNDS_INDICATOR_FLT = ButtonRect(56); //  new Rect(5, 56, 9, 9);
         private static readonly Rect BOUNDS_INDICATOR_S_1 = ButtonRect(66); //  new Rect(5, 66, 9, 9);
         private static readonly Rect BOUNDS_INDICATOR_S_2 = ButtonRect(76); //  new Rect(5, 76, 9, 9);
         private static readonly Rect BOUNDS_INDICATOR_S_3 = ButtonRect(86); //  new Rect(5, 86, 9, 9);

         private static Rect ButtonRect(int top)
         {
            float gaugeScale = (float)NanoGauges.configuration.gaugeScaling;
            return new Rect(5 * gaugeScale, top * gaugeScale, 9 * gaugeScale, 9 * gaugeScale);
         }

         public override string GetName()
         {
            return "Selector";
         }

         public override string GetDescription()
         {
            return "Selects a set of gauges for display.";
         }

         public override void Reset()
         {
            // nothing todo
         }

         private readonly Gauges gauges;

         private readonly Texture2D skin;
         private Rect skinBounds = new Rect(0, 0, NanoGauges.configuration.gaugeWidth, NanoGauges.configuration.gaugeHeight);

         public SelectorGauge(Gauges gauges)
            : base(Constants.WINDOW_ID_GAUGE_SETS)
         {
            this.gauges = gauges;
            this.skin = Utils.GetTexture("Nereid/NanoGauges/Resource/SETS-skin");
         }


         protected override void OnWindow(int id)
         {
            // skin
            GUI.DrawTexture(skinBounds, skin);

            // indicator
            GaugeSet.ID currentGaugeSet = NanoGauges.configuration.GetGaugeSetId();
            switch(currentGaugeSet)
            {
               case GaugeSet.ID.STANDARD:
                  GUI.DrawTexture(BOUNDS_INDICATOR_STD, INDICATOR__ON_SKIN);
                  break;
               case GaugeSet.ID.LAUNCH:
                  GUI.DrawTexture(BOUNDS_INDICATOR_LAU, INDICATOR__ON_SKIN);
                  break;
               case GaugeSet.ID.LAND:
                  GUI.DrawTexture(BOUNDS_INDICATOR_LAN, INDICATOR__ON_SKIN);
                  break;
               case GaugeSet.ID.DOCK:
                  GUI.DrawTexture(BOUNDS_INDICATOR_DCK, INDICATOR__ON_SKIN);
                  break;
               case GaugeSet.ID.ORBIT:
                  GUI.DrawTexture(BOUNDS_INDICATOR_ORB, INDICATOR__ON_SKIN);
                  break;
               case GaugeSet.ID.FLIGHT:
                  GUI.DrawTexture(BOUNDS_INDICATOR_FLT, INDICATOR__ON_SKIN);
                  break;
               case GaugeSet.ID.SET1:
                  GUI.DrawTexture(BOUNDS_INDICATOR_S_1, INDICATOR__ON_SKIN);
                  break;
               case GaugeSet.ID.SET2:
                  GUI.DrawTexture(BOUNDS_INDICATOR_S_2, INDICATOR__ON_SKIN);
                  break;
               case GaugeSet.ID.SET3:
                  GUI.DrawTexture(BOUNDS_INDICATOR_S_3, INDICATOR__ON_SKIN);
                  break;
            }

            // Mouseclicks
            Event e = Event.current;
            if (e.type == EventType.MouseDown && e.button == 0)
            {
               float x = e.mousePosition.x;
               float y = e.mousePosition.y;
               CheckIndicatorClick(x, y, BOUNDS_INDICATOR_STD, GaugeSet.ID.STANDARD);
               CheckIndicatorClick(x, y, BOUNDS_INDICATOR_LAU, GaugeSet.ID.LAUNCH);
               CheckIndicatorClick(x, y, BOUNDS_INDICATOR_LAN, GaugeSet.ID.LAND);
               CheckIndicatorClick(x, y, BOUNDS_INDICATOR_DCK, GaugeSet.ID.DOCK);
               CheckIndicatorClick(x, y, BOUNDS_INDICATOR_ORB, GaugeSet.ID.ORBIT);
               CheckIndicatorClick(x, y, BOUNDS_INDICATOR_FLT, GaugeSet.ID.FLIGHT);
               CheckIndicatorClick(x, y, BOUNDS_INDICATOR_S_1, GaugeSet.ID.SET1);
               CheckIndicatorClick(x, y, BOUNDS_INDICATOR_S_2, GaugeSet.ID.SET2);
               CheckIndicatorClick(x, y, BOUNDS_INDICATOR_S_3, GaugeSet.ID.SET3);
            }
         }


         private void CheckIndicatorClick(float x, float y, Rect bounds, GaugeSet.ID id)
         {
            if(x>=bounds.x && x<bounds.x+bounds.width && y>=bounds.y && y<bounds.y+bounds.height)
            {
               NanoGauges.configuration.SetGaugeSet(id);
               this.gauges.ReflectGaugeSetChange();
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

         public override void OutOfLimits()
         {
         }

         public override bool IsInLimits()
         {
            return true;
         }
      }
   }
}
