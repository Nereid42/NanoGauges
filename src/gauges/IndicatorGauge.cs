using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {
      public class IndicatorGauge : AbstractClosableGauge
      {
         private readonly VesselInspecteur vesselInspecteur;
         private readonly EngineInspecteur engineInspecteur;

         private const int INDICATOR_GEAR = 0;
         private const int INDICATOR_BRAKE = 1;
         private const int INDICATOR_AIRBRAKES = 2;
         private const int INDICATOR_FLAPS = 3;
         private const int INDICATOR_AFTERBURNER = 4;
         private const int INDICATOR_WHEELS = 5;

         private const int INDICATOR_TOP_OFFSET = 7;
         private const int INDICATOR_HEIGHT = 14;
         private const float BLINKING_INTERVAL = 0.6f;

         private readonly Texture2D skin;
         private Rect skinBounds = new Rect(0, 0, NanoGauges.configuration.verticalGaugeWidth, NanoGauges.configuration.verticalGaugeHeight);
         private Rect colorBounds = new Rect(0, 0, NanoGauges.configuration.verticalGaugeWidth,INDICATOR_HEIGHT);

         private readonly Texture2D noLight = Utils.CreateColorTexture(Color.gray);
         private readonly Texture2D redLight = Utils.CreateColorTexture(Color.red);
         private readonly Texture2D yellowLight = Utils.CreateColorTexture(Color.yellow);
         private readonly Texture2D greenLight = Utils.CreateColorTexture(Color.green);
         private readonly Texture2D orangeLight = Utils.CreateColorTexture(new Color(255,120,0));
         private readonly Texture2D blueLight = Utils.CreateColorTexture(Color.blue);

         // blinking lights
         private float blinkTime;
         private int blinkingPhase = 0;

         public IndicatorGauge(VesselInspecteur vesselInspecteur, EngineInspecteur engineInspecteur)
            : base(Constants.WINDOW_ID_GAUGE_INDICATOR)
         {
            this.skin = Utils.GetTexture("Nereid/NanoGauges/Resource/INDICATOR-skin");
            this.vesselInspecteur = vesselInspecteur;
            this.engineInspecteur = engineInspecteur;
            blinkTime = Time.time;
         }

         protected void drawLight(int pos, Texture2D color)
         {
            float y0 = (INDICATOR_TOP_OFFSET + pos * INDICATOR_HEIGHT) * (float)NanoGauges.configuration.gaugeScaling;
            colorBounds.y = y0;
            GUI.DrawTexture(colorBounds, color);
         }

         protected void drawBlinkingLight(int pos, Texture2D color1, Texture2D color2)
         {
            float y0 = (INDICATOR_TOP_OFFSET + pos * INDICATOR_HEIGHT) * (float)NanoGauges.configuration.gaugeScaling;
            colorBounds.y = y0;

            if (blinkingPhase==0)
            {
               GUI.DrawTexture(colorBounds, color1);
            }
            else
            {
               GUI.DrawTexture(colorBounds, color2);
            }
         }

         private void drawLandingGearState()
         {
            switch(vesselInspecteur.landingGearState)
            {
               case VesselInspecteur.GEARSTATES.NOT_INSTALLED:
                  drawLight(INDICATOR_GEAR, noLight);
                  break;
               case VesselInspecteur.GEARSTATES.DEPLOYED:
                  drawLight(INDICATOR_GEAR, greenLight);
                  break;
               case VesselInspecteur.GEARSTATES.RETRACTED:
                  drawLight(INDICATOR_GEAR, redLight);
                  break;
               case VesselInspecteur.GEARSTATES.RETRACTING:
               case VesselInspecteur.GEARSTATES.DEPLOYING:
                  drawBlinkingLight(INDICATOR_GEAR, noLight, redLight);
                  break;
               case VesselInspecteur.GEARSTATES.PARTIAL_DEPLOYED:
                  drawLight(INDICATOR_GEAR, yellowLight);
                  break;
               default:
                  drawLight(INDICATOR_GEAR, noLight);
                  break;
            }
         }

         private void drawBrakeState()
         {
            switch (vesselInspecteur.brakeState)
            {
               case VesselInspecteur.BRAKESTATES.NOT_INSTALLED:
                  drawLight(INDICATOR_BRAKE, noLight);
                  break;
               case VesselInspecteur.BRAKESTATES.NOT_ENGAGED:
                  drawLight(INDICATOR_BRAKE, greenLight);
                  break;
               case VesselInspecteur.BRAKESTATES.ENGAGED:
                  drawLight(INDICATOR_BRAKE, redLight);
                  break;
               case VesselInspecteur.BRAKESTATES.PARTIAL_ENGAGED:
                  drawLight(INDICATOR_BRAKE, yellowLight);
                  break;
               default:
                  drawLight(INDICATOR_BRAKE, noLight);
                  break;
            }
         }

         private void drawAirBrakeState()
         {
            switch (vesselInspecteur.airBrakeState)
            {
               case VesselInspecteur.BRAKESTATES.NOT_INSTALLED:
                  drawLight(INDICATOR_AIRBRAKES, noLight);
                  break;
               case VesselInspecteur.BRAKESTATES.NOT_ENGAGED:
                  drawLight(INDICATOR_AIRBRAKES, greenLight);
                  break;
               case VesselInspecteur.BRAKESTATES.ENGAGED:
                  drawLight(INDICATOR_AIRBRAKES, redLight);
                  break;
               case VesselInspecteur.BRAKESTATES.PARTIAL_ENGAGED:
                  drawLight(INDICATOR_AIRBRAKES, yellowLight);
                  break;
               default:
                  drawLight(INDICATOR_AIRBRAKES, noLight);
                  break;
            }
         }

         private void drawFlapState()
         {
            switch (vesselInspecteur.flapState)
            {
               case VesselInspecteur.FPLAPSTATES.NOT_INSTALLED:
                  drawLight(INDICATOR_FLAPS, noLight);
                  break;
               case VesselInspecteur.FPLAPSTATES.NOT_ENGAGED:
                  drawLight(INDICATOR_FLAPS, greenLight);
                  break;
               case VesselInspecteur.FPLAPSTATES.ENGAGED:
                  drawLight(INDICATOR_FLAPS, redLight);
                  break;
               case VesselInspecteur.FPLAPSTATES.PARTIAL_ENGAGED:
                  drawLight(INDICATOR_FLAPS, yellowLight);
                  break;
               default:
                  drawLight(INDICATOR_FLAPS, noLight);
                  break;
            }
         }


         private void drawAfterburnerState()
         {
            if (engineInspecteur.afterburnerInstalled)
            {
               if (engineInspecteur.afterburnerRunning)
               {
                  drawLight(INDICATOR_AFTERBURNER, redLight);
               }
               else
               {
                  if(engineInspecteur.afterburnerOperational)
                  {
                     drawLight(INDICATOR_AFTERBURNER, greenLight);
                  }
                  else
                  {
                     drawLight(INDICATOR_AFTERBURNER, yellowLight);
                  }
               }
            }
            else
            {
               drawLight(INDICATOR_AFTERBURNER, noLight);
            }
         }

         private void drawWheelState()
         {
            switch (vesselInspecteur.wheelState)
            {

               case VesselInspecteur.WHEELDAMAGE.OPERATIONAL:
                  drawLight(INDICATOR_WHEELS, greenLight);
                  break;
               case VesselInspecteur.WHEELDAMAGE.PARTIAL_DAMAGED:
                  drawLight(INDICATOR_WHEELS, yellowLight);
                  break;
               case VesselInspecteur.WHEELDAMAGE.DAMAGED:
                  drawLight(INDICATOR_WHEELS, redLight);
                  break;
               case VesselInspecteur.WHEELDAMAGE.NOT_INSTALLED:
               default:
                  drawLight(INDICATOR_WHEELS, noLight);
                  break;
            }
         }

         protected override void OnWindow(int id)
         {
            if (Time.time-blinkTime > BLINKING_INTERVAL)
            {
               blinkingPhase = (blinkingPhase + 1) % 2;
               blinkTime = Time.time;
            }
            //
            //
            // status indication
            drawLandingGearState();
            drawBrakeState();
            drawAirBrakeState();
            drawFlapState();
            drawAfterburnerState();
            drawWheelState();
            //drawLight(5, noLight);
            // skin
            GUI.DrawTexture(skinBounds, skin);
         }

         public override sealed int GetWidth()
         {
            return NanoGauges.configuration.verticalGaugeWidth;
         }

         public override sealed int GetHeight()
         {
            return NanoGauges.configuration.verticalGaugeHeight;
         }

         public override string GetName()
         {
            return "Status";
         }

         public override string GetDescription()
         {
            return "Display status of Gear, Brakes, Air Brakes, Flaps, Afterburner and Antennas.";
         }

         public override void Reset()
         {
            // nothing todo
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

         public override void OnGaugeScalingChanged()
         {
            // change dimensions of window
            bounds.width = NanoGauges.configuration.verticalGaugeWidth;
            bounds.height = NanoGauges.configuration.verticalGaugeHeight;
            //
            //change dimensions of skin
            skinBounds.width = NanoGauges.configuration.verticalGaugeWidth;
            skinBounds.height = NanoGauges.configuration.verticalGaugeHeight;
            // 
            // colors
            colorBounds.width = NanoGauges.configuration.verticalGaugeWidth;
            colorBounds.height = INDICATOR_HEIGHT * (float)NanoGauges.configuration.gaugeScaling;
         }
      }
   }
}
