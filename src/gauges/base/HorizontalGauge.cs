using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {
      public abstract class HorizontalGauge : AbstractClosableGauge
      {
         public const int SCALE_HEIGHT = 42;
         public const int SCALE_WIDTH = 800;
         //

         private readonly Texture2D skin;
         private readonly Texture2D scale;
         private Rect scaleBounds;
         private Rect skinBounds;
         

         private readonly PowerOffFlag offFlag;
         private readonly LimiterFlag limitFlag;

         public HorizontalGauge(int id, Texture2D skin, Texture2D scale)
            : base(id)
         {
            this.scale = scale;
            this.skin = skin;
            this.skinBounds = new Rect(0, 0, NanoGauges.configuration.verticalGaugeWidth, NanoGauges.configuration.verticalGaugeHeight);
            this.scaleBounds = new Rect(0, 0, SCALE_WIDTH, SCALE_HEIGHT);

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


         protected virtual void DrawInternalScale()
         {
            // to be overwritten and implemented by subclasses
         }

         protected override void OnWindow(int id)
         {
            // check for on/off
            AutomaticOnOff();
            // check for limits (wont work very well, so disabled at the moment)
            //AutomaticLimiter();


            float horizonalScaleratio = (float)Configuration.UNSCALED_HORIZONTAL_GAUGE_WIDTH / (float)SCALE_WIDTH;

            Rect off = new Rect(GetScaleOffset(), 0, horizonalScaleratio, 1.0f);
            GUI.DrawTextureWithTexCoords(skinBounds, scale, off, false);
            //
            // internal scales or pointer (gauge specific)
            DrawInternalScale();
            //
            // flags
            if(NanoGauges.configuration.gaugeMarkerEnabled)
            {
               // draw current state of flags (on/off and limiter)
               // increment animation step on each draw (flags will not show up immediately)
               offFlag.Draw(GetWidth() / 2 - 4, 0);
               limitFlag.Draw(0, 0);
            }
            //
            // skin
            GUI.DrawTexture(skinBounds, skin);
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

         public override void OutOfLimits()
         {
            limitFlag.Down();
         }

         public override bool IsInLimits()
         {
            return limitFlag.IsUp();
         }

         public override void Reset()
         {
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

         public override void OnGaugeScalingChanged()
         {
            // change dimensions of window
            bounds.width = NanoGauges.configuration.horizontalGaugeWidth;
            bounds.height = NanoGauges.configuration.horizontalGaugeHeight;
            //
            //change dimensions of skin and scale
            skinBounds.width = NanoGauges.configuration.horizontalGaugeWidth;
            skinBounds.height = NanoGauges.configuration.horizontalGaugeHeight;
         }

      }
   }
}
