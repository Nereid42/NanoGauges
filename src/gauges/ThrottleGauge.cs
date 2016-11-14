using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {
      
      public class ThrottleGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/THROTTLE-skin");
         private static readonly Texture2D INNERSCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/THROTTLE-innerscale");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/THROTTLE-scale");


         private readonly DigitalDisplay throttleDisplay;

         private EngineInspecteur inspector;

         // position of scale
         private Rect innerposition = new Rect();

         public ThrottleGauge(EngineInspecteur inspector)
            : base(Constants.WINDOW_ID_GAUGE_THROTTLE, SKIN, SCALE, true, 0.001f)
         {
            this.inspector = inspector;
            this.throttleDisplay = new DigitalDisplay(this,3,false);
            this.innerposition.x = 0;
            this.innerposition.width = 1.0f;
         }

         public override string GetName()
         {
            return "Throttle";
         }

         public override string GetDescription()
         {
            return "Throttle setting";
         }


         protected override void AjustValues()
         {
            base.AjustValues();

            if(FlightInputHandler.state!=null)
            {
               int throttle = (int)(FlightInputHandler.state.mainThrottle*100+0.5f);
               if (throttle <= 100 && throttle >= 0)
               {
                  throttleDisplay.SetValue(throttle);
               }
               else
               {
                  throttleDisplay.SetValue(999);
               }
            }
            else
            {
               throttleDisplay.SetValue(0);
            }            
         }

         protected override void DrawInternalScale()
         {
            // analog throttle display
            float b = GetLowerOffset();
            float verticalScaleratio = (float)Configuration.UNSCALED_VERTICAL_GAUGE_HEIGHT / (float)SCALE_HEIGHT;

            if (FlightInputHandler.state != null)
            {
               float throttle = FlightInputHandler.state.mainThrottle;
               if (throttle < 0) throttle = 0;
               if (throttle > 1) throttle = 1;

               float scaleoffset = b + 300.0f * throttle / (float)SCALE_HEIGHT;

               // scale
               this.innerposition.y = scaleoffset;
               this.innerposition.height = verticalScaleratio;
            }
            else
            {
               // scale
               this.innerposition.y = b;
               this.innerposition.height = verticalScaleratio;
            }
            // draw scale
            GUI.DrawTextureWithTexCoords(gaugeBounds, INNERSCALE, innerposition, true);

            // digital throttle display
            float scaling = (float)NanoGauges.configuration.gaugeScaling;
            throttleDisplay.Draw(0.5f * scaling, GetHeight() - throttleDisplay.GetHeight() - 3.0f * scaling);
         }

         protected override void DrawOverlay()
         {
         }

         protected override float GetScaleOffset()
         {
            float b = GetLowerOffset();
            float y = b;

            Vessel vessel = FlightGlobals.ActiveVessel;

            if(vessel==null || vessel.isEVA || FlightInputHandler.state == null)
            {
               Off();
               return y;
            }
            else
            { 
               On(); 
            }

            if (vessel != null && IsOn())
            {

               double thrust = inspector.engineRelativeThrust;

               Log.Test("thrust=" + thrust.ToString("0.00"));

               if (thrust >= 0.0 && thrust <= 1.0)
               {
                  y = b + 300.0f * (float)thrust / (float)SCALE_HEIGHT;
                  InLimits();
               }
               else
               {
                  NotInLimits();
               }
            }
            return y;
         }

         public override string ToString()
         {
            return "Gauge:THROTTLE";
         }


         public override void OnGaugeScalingChanged()
         {
            base.OnGaugeScalingChanged();
            throttleDisplay.SetScaling((float)NanoGauges.configuration.gaugeScaling);
         }

      }
   }
}
