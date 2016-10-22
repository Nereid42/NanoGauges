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

         private float scaleOffset = 0;

         // needles
         private readonly Texture2D NEEDLE_BLUE = Utils.GetTexture("Nereid/NanoGauges/Resource/BLUE-horizontal-needle");
         private bool blueNeedleEnabled;
         private float blueNeedleDegrees;
         //private bool redNeedleEnabled;


         public AbstractCompassGauge(int id, Texture2D skin)
            : base(id, skin, SCALE00,SCALE0B)
         {
            relativeFlag = new RFlag(this);

            this.blueNeedleEnabled = false;
            this.blueNeedleDegrees = 0.0f;
            //this.redNeedleEnabled = false;
         }

         public void SetBlueNeedleTo(float degrees)
         {
            this.blueNeedleDegrees = degrees;
         }

         public void EnableBlueNeedle()
         {
            blueNeedleEnabled = true;
         }

         /*public void EnableRedNeedle()
         {
            redNeedleEnabled = true;
         }

         public void DisableRedNeedle()
         {
            redNeedleEnabled = false;
         }*/

         public void DisableBlueNeedle()
         {
            blueNeedleEnabled = false;
         }


         protected override void DrawInternalScale()
         {
            if(blueNeedleEnabled)
            {
               //float delta = 
               float gw = (float)GetWidth();
               float gh = (float)GetHeight();
               float sw = (float)NEEDLE_BLUE.width;
               float sh = (float)NEEDLE_BLUE.height;
               float w = sw / gw;
               float h = sh / gh;
               Rect off = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
               float offset =  0.5f; //GetDegrees()
               Rect skinBounds = new Rect(x, y - sh + offset, sw, sh);
               GUI.DrawTextureWithTexCoords(skinBounds, NEEDLE_BLUE, off, false);
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
            float offset = ( GetDegrees() % 360 )  / (float)SCALE_WIDTH;
            if (!IsOn() || !IsInLimits()) return this.scaleOffset;
            this.scaleOffset = offset;
            return offset;
         }
      }
   }
}
