using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public abstract class AbstractCompassGauge : HorizontalGauge
      {
         private static readonly Texture2D SCALE00 = Utils.GetTexture("Nereid/NanoGauges/Resource/COMPASS00-scale");
         private static readonly Texture2D SCALE0B = Utils.GetTexture("Nereid/NanoGauges/Resource/COMPASS0B-scale");

         private const int X_FLAG_RELATIVE = 14;
         private readonly Flag relativeFlag;

         private float degrees = 0;
         private float scaleOffset = 0;


         // needles
         private readonly Texture2D NEEDLE_BLUE = Utils.GetTexture("Nereid/NanoGauges/Resource/BLUE-horizontal-needle");
         private readonly Texture2D NEEDLE_RED = Utils.GetTexture("Nereid/NanoGauges/Resource/RED-horizontal-needle");
         private readonly Texture2D NEEDLE_YELLOW = Utils.GetTexture("Nereid/NanoGauges/Resource/YELLOW-horizontal-needle");
         private Needle blueNeedle;
         private Needle redNeedle;
         private Needle yellowNeedle;

         public AbstractCompassGauge(int id, Texture2D skin)
            : base(id, skin, SCALE00,SCALE0B)
         {
            relativeFlag = new RFlag(this);

            this.blueNeedle = new Needle(this, NEEDLE_BLUE);
            this.redNeedle = new Needle(this, NEEDLE_RED);
            this.yellowNeedle = new Needle(this, NEEDLE_YELLOW);

         }

         public void SetBlueNeedleTo(float degrees)
         {
            this.blueNeedle.degrees = degrees;
         }

         public void EnableBlueNeedle()
         {
            this.blueNeedle.enabled = true;
         }

         public void DisableBlueNeedle()
         {
            this.blueNeedle.enabled = false;
         }

         public void SetRedNeedleTo(float degrees)
         {
            this.redNeedle.degrees = degrees;
         }

         public void EnableRedNeedle()
         {
            this.redNeedle.enabled = true;
         }

         public void DisableRedNeedle()
         {
            this.redNeedle.enabled = false;
         }

         public void SetYellowNeedleTo(float degrees)
         {
            this.yellowNeedle.degrees = degrees;
         }

         public void EnableYellowNeedle()
         {
            this.yellowNeedle.enabled = true;
         }

         public void DisableYellowNeedle()
         {
            this.yellowNeedle.enabled = false;
         }


         protected override void DrawInternalScale()
         {
            blueNeedle.Draw(this.degrees);
            redNeedle.Draw(this.degrees);
            yellowNeedle.Draw(this.degrees);
         }

         protected abstract float GetDegrees();

         protected override void DrawFlags()
         {
            base.DrawFlags();
            relativeFlag.Draw(X_FLAG_RELATIVE, 0);
         }

         protected void Relative()
         {
            SecondaryScale();
            relativeFlag.Down();
         }

         protected void Absolut()
         {
            PrimaryScale();
            relativeFlag.Up();
         }

         protected override float GetScaleOffset()
         {
            float degrees = GetDegrees() % 360;
            float offset = degrees / (float)SCALE_WIDTH;
            if (!IsOn() || !IsInLimits()) return this.scaleOffset;
            this.scaleOffset = offset;
            this.degrees = degrees;
            return offset;
         }

         private class Needle : Sprite
         {
            public float degrees = 0.0f;
            public bool enabled = false;

            private readonly Damper traverseDamper;

            private bool traversing = false;

            public Needle(AbstractGauge gauge, Texture2D texture)
               : base(gauge,texture)
            {
               traverseDamper = new Damper(1.0f,int.MinValue,int.MaxValue);
            }

            public void Draw(float relativeTo)
            {
               if(enabled)
               {
                  float angle = degrees - relativeTo;
                  if (angle > 180.0f) angle = angle - 360.0f;
                  if (angle < -180.0f) angle = angle + 360.0f;
                  float x = angle + gauge.GetWidth() / 2;
                  float margin = gauge.GetWidth() / 24.0f;
                  float xmin = margin;
                  float xmax = gauge.GetWidth() - margin - GetWidth();
                  if (x <= xmin)
                  {
                     if(!traversing)
                     {
                        traverseDamper.SetValue(xmin);
                        traversing = true;
                     }
                     traverseDamper.Set(xmin);
                     x = traverseDamper.Get();
                     Draw(x, 0);
                     return;
                  }
                  if (x >= xmax)
                  {
                     if (!traversing)
                     {
                        traverseDamper.SetValue(xmax);
                        traversing = true;
                     }
                     traverseDamper.Set(xmax);
                     x = traverseDamper.Get();
                     Draw(x, 0);
                     return;
                  }
                  traversing = false;
                  Draw(x, 0);
               }
            }
         }
      }
   }
}
