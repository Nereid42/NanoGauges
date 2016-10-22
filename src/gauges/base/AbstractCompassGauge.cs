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
         private readonly Sprite blueNeedle;
         private bool blueNeedleEnabled;
         private float blueNeedleDegrees;

         public AbstractCompassGauge(int id, Texture2D skin)
            : base(id, skin, SCALE00,SCALE0B)
         {
            relativeFlag = new RFlag(this);

            this.blueNeedle = new Sprite(this, NEEDLE_BLUE);
            this.blueNeedleEnabled = false;
            this.blueNeedleDegrees = 0.0f;
         }

         public void SetBlueNeedleTo(float degrees)
         {
            this.blueNeedleDegrees = degrees;
         }

         public void EnableBlueNeedle()
         {
            blueNeedleEnabled = true;
         }

         public void DisableBlueNeedle()
         {
            blueNeedleEnabled = false;
         }


         protected override void DrawInternalScale()
         {
            if(blueNeedleEnabled)
            {
               blueNeedleDegrees = 45f;

               float angle = (blueNeedleDegrees - ((IsOn() && IsInLimits()) ? GetDegrees() : this.degrees)) % 360;
               if (angle < 0) angle += 360;
               float x = angle + GetWidth() / 2;
               blueNeedle.Draw(x, 0);
            }
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
      }
   }
}
