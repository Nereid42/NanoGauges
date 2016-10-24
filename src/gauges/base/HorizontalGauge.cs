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

         // wich scale toi display
         private enum SCALE { PRIMARY, SECONDARY } ;
         private SCALE mode = SCALE.PRIMARY;

         private readonly Texture2D skin;
         private readonly Texture2D primaryScale;
         private readonly Texture2D secondaryScale;
         private Rect scaleBounds;
         private Rect skinBounds;
         // position of scale
         private Rect off = new Rect();
         

         private readonly PowerOffFlag offFlag;
         private readonly LimiterFlag limitFlag;


         public HorizontalGauge(int id, Texture2D skin, Texture2D primaryScale, Texture2D secondaryScale = null)
            : base(id)
         {
            this.primaryScale = primaryScale;
            this.secondaryScale = secondaryScale;
            this.skin = skin;
            this.skinBounds = new Rect(0, 0, NanoGauges.configuration.verticalGaugeWidth, NanoGauges.configuration.verticalGaugeHeight);
            this.scaleBounds = new Rect(0, 0, SCALE_WIDTH, SCALE_HEIGHT);
            //
            this.off.y = 0;
            this.off.height = 1.0f;


            if (primaryScale == null) Log.Error("no scale for gauge " + id + " defined");
            if (skin == null) Log.Error("no skin for gauge " + id + " defined");

            offFlag = new PowerOffFlag(this);
            limitFlag = new LimiterFlag(this);
         }

         protected void PrimaryScale()
         {
            mode = SCALE.PRIMARY;
         }

         protected void SecondaryScale()
         {
            mode = SCALE.SECONDARY;
         }



         /**
          * Get the fractional offset of the scale. This method will move the scale regarding to readout value. It has to be implemented by a gauge. 
          */
         protected abstract float GetScaleOffset();

         public override sealed int GetWidth()
         {
            return NanoGauges.configuration.horizontalGaugeWidth;
         }

         public override sealed int GetHeight()
         {
            return NanoGauges.configuration.horizontalGaugeHeight;
         }

         protected int GetScaleHeight()
         {
            return primaryScale.height;
         }

         protected float GetCenterOffset()
         {
            return (primaryScale.height - Configuration.UNSCALED_VERTICAL_GAUGE_HEIGHT) / (2.0f * primaryScale.height);
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
            int d = (primaryScale.height-y) - Configuration.UNSCALED_VERTICAL_GAUGE_HEIGHT / 2;
            return ((float)d) / (float)primaryScale.height;
         }

         /**
          * Get the fractional offset of the scale for maximal value
          */
         protected float GetUpperOffset()
         {
            return ((float)primaryScale.height - (float)Configuration.UNSCALED_VERTICAL_GAUGE_HEIGHT) / (float)primaryScale.height;
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

         protected virtual void DrawOverlay()
         {
            // to be overwritten and implemented by subclasses
         }

         protected virtual void DrawFlags()
         {
            // draw current state of flags (on/off and limiter)
            // increment animation step on each draw (flags will not show up immediately)
            offFlag.Draw(GetWidth() - offFlag.GetWidth(), 0);
            limitFlag.Draw(0, 0);
         }

         protected override void OnWindow(int id)
         {
            // check for on/off
            AutomaticOnOff();

            float horizonalScaleratio = (float)Configuration.UNSCALED_HORIZONTAL_GAUGE_WIDTH / (float)SCALE_WIDTH;

            // draw scale
            float scaleOffset = GetScaleOffset();
            this.off.x = scaleOffset;
            this.off.width = horizonalScaleratio;
            if(mode==SCALE.PRIMARY && secondaryScale!=null)
            {
               // primary scale
               GUI.DrawTextureWithTexCoords(skinBounds, primaryScale, off, false);
            }
            else
            {
               // secondary scale
               GUI.DrawTextureWithTexCoords(skinBounds, secondaryScale, off, false);
            }
            //
            // internal scales or pointer (gauge specific)
            DrawInternalScale();
            //
            // flags
            if(NanoGauges.configuration.gaugeMarkerEnabled)
            {
               DrawFlags();
            }
            //
            // skin
            GUI.DrawTexture(skinBounds, skin);
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
