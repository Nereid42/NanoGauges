﻿using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {
      public abstract class VerticalGauge : AbstractClosableGauge
      {
         public const int SCALE_HEIGHT = 400;
         public const int SCALE_WIDTH = 42;
         //

         private readonly Texture2D skin;
         private readonly Texture2D scale;
         protected Rect gaugeBounds;

         // position of scale
         private Rect position = new Rect();
         
         private Damper damper;
         private bool autoLimiterEnabled = false;

         private readonly Flag offFlag;
         private readonly Flag limitFlag;

         private readonly VerticalGaugeZoom zoom;

         public VerticalGauge(int id, Texture2D skin, Texture2D scale, bool damped = true, float dampfactor=0.002f)
            : base(id)
         {
            this.damper = new Damper(dampfactor); 
            this.damper.SetEnabled(damped);
            this.scale = scale;
            this.skin = skin;
            this.gaugeBounds = new Rect(0, 0, NanoGauges.configuration.verticalGaugeWidth, NanoGauges.configuration.verticalGaugeHeight);
            //
            this.zoom = new VerticalGaugeZoom(this,skin,scale);
            //
            this.position.x = 0;
            this.position.width = 1.0f;


            if (scale == null) Log.Error("no scale for gauge " + id + " defined");
            if (skin == null) Log.Error("no skin for gauge " + id + " defined");

            offFlag = new PowerOffFlag(this);
            limitFlag = new LimiterFlag(this);
         }

         /**
          * Get the fractional offset of the scale. This method will move the scale regarding to readout value. It has to be implemented by a gauge. 
          */
         protected abstract float GetScaleOffset();

         public override sealed int GetWidth()
         {
            return NanoGauges.configuration.verticalGaugeWidth;
         }

         public override sealed int GetHeight()
         {
            return NanoGauges.configuration.verticalGaugeHeight;
         }

         protected int GetScaleHeight()
         {
            return scale.height;
         }

         protected float GetCenterOffset()
         {
            return (scale.height - Configuration.UNSCALED_VERTICAL_GAUGE_HEIGHT) / (2.0f * scale.height);
         }

         /**
          * Get the fractional offset of the scale for minimal value
          */
         protected float GetLowerOffset()
         {
            return 0.0f;
         }

         /**
          * Get the fractional offset of the scale for a specific offset in pixel
          */
         protected float GetOffset(int y)
         {
            int d = (scale.height-y) - Configuration.UNSCALED_VERTICAL_GAUGE_HEIGHT / 2;
            return ((float)d) / (float)scale.height;
         }

         /**
          * Get the fractional offset of the scale for maximal value
          */
         protected float GetUpperOffset()
         {
            return ((float)scale.height - (float)Configuration.UNSCALED_VERTICAL_GAUGE_HEIGHT) / (float)scale.height;
         }


         protected virtual void DrawInternalScale()
         {
            // to be overwritten and implemented by subclasses
         }

         protected virtual void DrawOverlay()
         {
            // to be overwritten and implemented by subclasses
         }

         private double GetInternalScaleOffset()
         {
            try 
            {
               return GetScaleOffset();
            }
            catch(Exception e)
            {
               Log.Error("Exception (scale offset) in gauge " + this.GetType() + " detected: " + e.GetType());
               return GetLowerOffset();
            }
         }

         private float GetDamperValue()
         {
            try
            {
               return damper.Get();
            }
            catch (Exception e)
            {
               Log.Error("Exception (damper value) in gauge " + this.GetType() + " detected: " + e.GetType()+" - "+damper);
               return 0;
            }
         }

         protected virtual void DrawFlags()
         {
            // to be implemented by subclasses
         }

         private void InternalDrawFlags()
         {
            // draw current state of flags (on/off and limiter)
            // increment animation step on each draw (flags will not show up immediately)
            offFlag.Draw(GetWidth() / 2 - 4*(float)NanoGauges.configuration.gaugeScaling, 0);
            limitFlag.Draw(0, 0);
            // draw flags from subclasses
            DrawFlags();
         }

         protected override void OnWindow(int id)
         {
            // check for on/off
            AutomaticOnOff();
            // check for limits (wont work very well, so disabled at the moment)
            //AutomaticLimiter();

            // damper for smoother changes in the gauges
            damper.Set(GetScaleOffset());

            float verticalScaleratio = (float)Configuration.UNSCALED_VERTICAL_GAUGE_HEIGHT / (float)SCALE_HEIGHT;

            // scale
            float scaleoffset = GetDamperValue();
            this.position.y = scaleoffset;
            this.position.height = verticalScaleratio;
            // draw scale
            GUI.DrawTextureWithTexCoords(gaugeBounds, scale, position, false);
            //
            // internal scale
            DrawInternalScale();
            //
            // zoom
            zoom.value = scaleoffset;
            //
            // flags
            if(NanoGauges.configuration.gaugeMarkerEnabled)
            {
               InternalDrawFlags();
            }
            //
            // skin
            GUI.DrawTexture(gaugeBounds, skin);
            //
            // Overlay
            DrawOverlay();
         }

         protected override void OnTooltip()
         {
            if(NanoGauges.configuration.exactReadoutEnabled)
            {
               // FIXME
               //zoom.Draw();
            }
         }

         public override void On()
         {
            offFlag.Up();
         }

         public override void Off()
         {
            offFlag.Down();
         }

         public override bool IsOn()
         {
            return offFlag.IsUp();
         }

         public override void InLimits()
         {
            limitFlag.Up();
         }

         public override void NotInLimits()
         {
            limitFlag.Down();
         }

         public override bool IsInLimits()
         {
            return limitFlag.IsUp();
         }

         public override void Reset()
         {
            damper.Reset();
            limitFlag.Up();
         }

         protected virtual void AutomaticOnOff()
         {
            if (FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.parts.Count>0)
            {
               On();
            }
            else
            {
               Off();
            }
         }

         // not working right now
         protected virtual void AutomaticLimiter()
         {
            if(autoLimiterEnabled)
            {
               if(damper.IsInLimits())
               {
                  InLimits();
               }
               else
               {
                  NotInLimits();
               }
            }
         }

         protected void SetAutoLimiterEnabled(bool enabled)
         {
            this.autoLimiterEnabled = enabled;
         }

         public override void OnGaugeScalingChanged()
         {
            // change dimensions of window
            bounds.width = NanoGauges.configuration.verticalGaugeWidth;
            bounds.height = NanoGauges.configuration.verticalGaugeHeight;
            //
            //change dimensions of skin and scale
            gaugeBounds.width = NanoGauges.configuration.verticalGaugeWidth;
            gaugeBounds.height = NanoGauges.configuration.verticalGaugeHeight;
            //
            // change flags
            limitFlag.ScaleTo(NanoGauges.configuration.gaugeScaling);
            offFlag.ScaleTo(NanoGauges.configuration.gaugeScaling);
         }

      }
   }
}
