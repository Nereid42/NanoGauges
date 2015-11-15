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
            int y0 = INDICATOR_TOP_OFFSET + pos * INDICATOR_HEIGHT;
            colorBounds.y = y0;
            GUI.DrawTexture(colorBounds, color);
         }

         protected void drawBlinkingLight(int pos, Texture2D color1, Texture2D color2)
         {
            int y0 = INDICATOR_TOP_OFFSET + pos * INDICATOR_HEIGHT;
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
                  drawLight(0, noLight);
                  break;
               case VesselInspecteur.GEARSTATES.DEPLOYED:
                  drawLight(0, greenLight);
                  break;
               case VesselInspecteur.GEARSTATES.RETRACTED:
                  drawLight(0, redLight);
                  break;
               case VesselInspecteur.GEARSTATES.RETRACTING:
               case VesselInspecteur.GEARSTATES.DEPLOYING:
                  drawBlinkingLight(0, noLight, redLight);
                  break;
               case VesselInspecteur.GEARSTATES.PARTIAL_DEPLOYED:
                  drawLight(0, yellowLight);
                  break;
               default:
                  drawLight(0, noLight);
                  break;
            }
         }

         private void drawBrakeState()
         {
            switch (vesselInspecteur.brakeState)
            {
               case VesselInspecteur.BRAKESTATES.NOT_INSTALLED:
                  drawLight(1, noLight);
                  break;
               case VesselInspecteur.BRAKESTATES.NOT_ENGAGED:
                  drawLight(1, greenLight);
                  break;
               case VesselInspecteur.BRAKESTATES.ENGAGED:
                  drawLight(1, redLight);
                  break;
               case VesselInspecteur.BRAKESTATES.PARTIAL_ENGAGED:
                  drawLight(1, yellowLight);
                  break;
               default:
                  drawLight(1, noLight);
                  break;
            }
         }

         private void drawAirBrakeState()
         {
            switch (vesselInspecteur.airBrakeState)
            {
               case VesselInspecteur.BRAKESTATES.NOT_INSTALLED:
                  drawLight(2, noLight);
                  break;
               case VesselInspecteur.BRAKESTATES.NOT_ENGAGED:
                  drawLight(2, greenLight);
                  break;
               case VesselInspecteur.BRAKESTATES.ENGAGED:
                  drawLight(2, redLight);
                  break;
               case VesselInspecteur.BRAKESTATES.PARTIAL_ENGAGED:
                  drawLight(2, yellowLight);
                  break;
               default:
                  drawLight(2, noLight);
                  break;
            }
         }

         private void drawFlapState()
         {
            switch (vesselInspecteur.flapState)
            {
               case VesselInspecteur.FPLAPSTATES.NOT_INSTALLED:
                  drawLight(3, noLight);
                  break;
               case VesselInspecteur.FPLAPSTATES.NOT_ENGAGED:
                  drawLight(3, greenLight);
                  break;
               case VesselInspecteur.FPLAPSTATES.ENGAGED:
                  drawLight(3, redLight);
                  break;
               case VesselInspecteur.FPLAPSTATES.PARTIAL_ENGAGED:
                  drawLight(3, yellowLight);
                  break;
               default:
                  drawLight(3, noLight);
                  break;
            }
         }

         private void drawAfterburnerState()
         {
            if (engineInspecteur.afterburnerInstalled)
            {
               if (engineInspecteur.afterburnerEnabled)
               {
                  drawLight(4, redLight);
               }
               else
               {
                  drawLight(4, greenLight);
               }
            }
            else
            {
               drawLight(4, noLight);
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
            drawLight(5, noLight);
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
      }
   }
}
