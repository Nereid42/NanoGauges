using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {
      public abstract class HorizontalDigitalGauge : AbstractClosableGauge
      {
         private const int MARGIN_VERTICAL = 15;
         private const int MARGIN_HORIZONTAL = 5;
         private const int FONT_SIZE = 14;
         private const int MAX_DIGITS = 8;
         private const int MAX_VALUE = 99999999;
         private const int MIN_VALUE = -9999999;
         private readonly Texture2D back;
         private readonly Texture2D skin;
         private Rect skinBounds;
         private Rect textBounds;

         private DigitalDisplay display;

         public HorizontalDigitalGauge(int id, Texture2D skin, Texture2D back = null)
            : base(id)
         {
            this.skin = skin;
            this.back = back==null?Utils.GetTexture("Nereid/NanoGauges/Resource/DIGIT-background"):back;
            this.skinBounds = new Rect(0, 0, GetWidth(), GetHeight());
            this.display = new DigitalDisplay(this, MAX_DIGITS, true);
            double scale = NanoGauges.configuration.gaugeScaling;
            float margin_vert = (float)(MARGIN_VERTICAL * scale);
            float margin_hori = (float)(MARGIN_HORIZONTAL * scale);
            this.textBounds = new Rect(margin_hori, margin_vert, GetWidth() - 2 * margin_hori, GetHeight() - margin_vert);
            //
            if (back == null) Log.Error("no background for gauge " + id + " defined");
            if (skin == null) Log.Error("no skin for gauge " + id + " defined");
         }



         protected override void OnWindow(int id)
         {
            // back
            GUI.DrawTexture(skinBounds, back);
            // text
            double value = GetValue();
            if(value==float.NaN)
            {
               value = 0;
            }
            double scale = NanoGauges.configuration.gaugeScaling;
            if(value > MAX_VALUE) value = MAX_VALUE;
            if(value < MIN_VALUE) value = MIN_VALUE;
            display.SetValue((int)value);
            display.Draw((float)(10.0*scale), (float)(17.0*scale));
            // skin
            GUI.DrawTexture(skinBounds, skin);
         }

         public override sealed int GetWidth()
         {
            return NanoGauges.configuration.horizontalGaugeWidth;
         }

         public override sealed int GetHeight()
         {
            return NanoGauges.configuration.horizontalGaugeHeight;
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

         public override void NotInLimits()
         {
         }

         public override bool IsInLimits()
         {
            return true;
         }

         public override void Reset()
         {
         }

         protected abstract double GetValue();

         public override void OnGaugeScalingChanged()
         {
            // change dimensions of window
            bounds.width = NanoGauges.configuration.horizontalGaugeWidth;
            bounds.height = NanoGauges.configuration.horizontalGaugeHeight;
            //
            //change dimensions of skin
            skinBounds.width = NanoGauges.configuration.horizontalGaugeWidth;
            skinBounds.height = NanoGauges.configuration.horizontalGaugeHeight;
            //
            // change text bounds and font
            double scale = NanoGauges.configuration.gaugeScaling;
            float margin_vert = (float)(MARGIN_VERTICAL * scale);
            float margin_hori = (float)(MARGIN_HORIZONTAL * scale);
            textBounds.x = margin_hori;
            textBounds.y = margin_vert;
            textBounds.width = GetWidth() - 2 * margin_hori;
            textBounds.height = GetHeight() - margin_vert;
         }
      }
   }
}
