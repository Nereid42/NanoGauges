using System;
using UnityEngine;
using System.Collections.Generic;

namespace Nereid
{
   namespace NanoGauges
   {

      public class MaxGeeGauge : VerticalGauge
      {
         private static Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/MAXG-skin");
         private static Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/MAXG-scale");
         private static readonly Texture2D RESET_BUTTON_OFF_SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/BUTTON-off");
         private static readonly Texture2D RESET_BUTTON_ON_SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/BUTTON-on");

         const int RESET_BUTTON_X = 5;
         const int RESET_BUTTON_Y = 86;
         const int RESET_BUTTON_WIDTH = 9;
         const int RESET_BUTTON_HEIGHT = 9;
         //
         private static Rect BOUNDS_RESET_BUTTON;

         private const double MAX_G = 10;

         private double maxg = 0;

         private Dictionary<Guid, double> maxgForVessel = new Dictionary<Guid, double>();
         private Dictionary<Guid, bool> limitsForVessel = new Dictionary<Guid, bool>();
         private Guid currentVesselId = Guid.Empty;

         private bool resetButtonPressed = false;
         private int ticksResetButtonPressed = 0;

         public MaxGeeGauge()
            : base(Constants.WINDOW_ID_GAUGE_MAXG, SKIN, SCALE)
         {
            GameEvents.onVesselChange.Add(this.OnVesselChange);
            BOUNDS_RESET_BUTTON = CreateButton();
         }

         private Rect CreateButton()
         {
            double scaling = NanoGauges.configuration.gaugeScaling;
            return new Rect(
               (int)(RESET_BUTTON_X * scaling), 
               (int)(RESET_BUTTON_Y * scaling), 
               (int)(RESET_BUTTON_WIDTH * scaling), 
               (int)(RESET_BUTTON_HEIGHT * scaling) );
         }

         public override string GetName()
         {
            return "Maximum Gee";
         }

         public override string GetDescription()
         {
            return "Maximal acceleration.";
         }

         private void OnVesselChange(Vessel newVessel)
         {
            if (currentVesselId != Guid.Empty)
            {
               SetMaxGForVessel(currentVesselId, maxg);
               SetLimitForVessel(currentVesselId, IsInLimits());
            }

            maxg = 0.0;
            if (newVessel != null)
            {
               Guid id = newVessel.id;

               if (maxgForVessel.ContainsKey(id))
               {
                  maxg = maxgForVessel[id];
               }
               else
               {
                  maxgForVessel.Add(id, 0.0);
               }
               // still not working
               if (limitsForVessel.ContainsKey(id))
               {
                  if (limitsForVessel[id])
                  {
                     InLimits();
                  }
                  else
                  {
                     OutOfLimits();
                  }
               }
               else
               {
                  limitsForVessel.Add(id, IsInLimits());
               }
            }
            if (Log.IsLogable(Log.LEVEL.DETAIL)) Log.Detail("max gee gauge: vessel change done");
         }

         private void SetMaxGForVessel(Guid id, double g)
         {
            if (id != Guid.Empty)
            {
               if (maxgForVessel.ContainsKey(id))
               {
                  maxgForVessel[id] = g;
               }
               else
               {
                  maxgForVessel.Add(id, g);
               }
            }
         }

         private void SetLimitForVessel(Guid id, bool inlimit)
         {
            if (id != Guid.Empty)
            {
               if (limitsForVessel.ContainsKey(id))
               {
                  limitsForVessel[id] = inlimit;
               }
               else
               {
                  limitsForVessel.Add(id, inlimit);
               }
            }
         }

         protected override void AutomaticOnOff()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && FlightGlobals.ActiveVessel.parts.Count > 0 && !double.IsNaN(vessel.geeForce))
            {
               if(!vessel.isEVA)
               {
                  On();
                  return;
               }
            }
            Off();
         }


         protected override void OnWindow(int id)
         {
            base.OnWindow(id);

            // Mouseclicks
            Event e = Event.current;
            if (e.type == EventType.MouseDown && e.button == 0)
            {
               float x = e.mousePosition.x;
               float y = e.mousePosition.y;
               if (InBounds(BOUNDS_RESET_BUTTON, x, y))
               {
                  maxg = 0.0;
                  InLimits();
                  resetButtonPressed = true;
                  ticksResetButtonPressed = Environment.TickCount;
               }
            }

            int ticksNow = Environment.TickCount;
            if (ticksNow - ticksResetButtonPressed>1000)
            {
               resetButtonPressed = false;
               ticksResetButtonPressed = 0;
            }

            GUI.DrawTexture(BOUNDS_RESET_BUTTON, resetButtonPressed?RESET_BUTTON_ON_SKIN:RESET_BUTTON_OFF_SKIN);
         }

         protected override float GetScaleOffset()
         {
            float b = GetLowerOffset();
            float y = b;
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && !vessel.isEVA && IsOn())
            {
               double g = vessel.geeForce;
               if (!double.IsNaN(g))
               {
                  if (g > maxg)
                  {
                     maxg = g;
                     currentVesselId = vessel.id;
                  }
                  if (maxg > MAX_G)
                  {
                     maxg = MAX_G;
                     OutOfLimits();
                  }
                  else if (maxg < 0)
                  {
                     maxg = 0;
                     OutOfLimits();
                  }
                  y = b + 30.0f * (float)maxg / 400.0f;
               }
            }
            return y;
         }

         public override string ToString()
         {
            return "Gauge:MAXG";
         }

         public override void OnGaugeScalingChanged()
         {
            base.OnGaugeScalingChanged();
            BOUNDS_RESET_BUTTON = CreateButton();
         }

      }
   }
}
