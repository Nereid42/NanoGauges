using System;
using UnityEngine;

namespace Nereid
{
   namespace NanoGauges
   {
      // WIP
      public class DigitalDisplay : Sprite
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/DIGIT2-skin");

         private const int BASE = 10;

         private readonly Sprite[] digits;
         private readonly Sprite seg7I;
         private readonly Sprite seg7H;

         private const int BORDER = 3;
         private const int numberOfDigits = 2;

         private int value;
         private readonly int limit;
         private float widthOfDigit;

         private float scale = 1.0f;

         public DigitalDisplay(AbstractGauge gauge)
            : base(gauge,SKIN)
         {
            digits = new Sprite[10];
            for(int i=0; i<10; i++)
            {
               digits[i] = new Sprite(gauge,Utils.GetTexture("Nereid/NanoGauges/Resource/"+i+"-digit"));
            }
            seg7I = new Sprite(gauge, Utils.GetTexture("Nereid/NanoGauges/Resource/I-digit"));
            seg7H = new Sprite(gauge, Utils.GetTexture("Nereid/NanoGauges/Resource/H-digit"));
            limit = (int)Math.Pow(10, numberOfDigits);
            widthOfDigit = seg7I.GetWidth();
         }

         public override void Draw(float x = 0, float y = 0)
         {
            base.Draw(x, y);

            float xdigits = x + BORDER;
            float ydigits = y + BORDER;

            if (value < limit)
            {
               int divisor = this.limit / BASE;
               int remainder = this.value;
               for(int i=0; i<numberOfDigits; i++)
               {
                  int digit = (remainder / divisor) % BASE;
                  remainder = remainder - digit*divisor;
                  divisor = divisor / BASE;
                  //
                  // draw digit
                  digits[digit].Draw(xdigits + i * widthOfDigit, ydigits);
               }
            }
            else
            {
               seg7H.Draw(xdigits, ydigits);
               seg7I.Draw(xdigits + widthOfDigit, ydigits);
            }
         }

         public void SetValue(int value)
         {
            this.value = value;
         }

         public void SetScale(float scale)
         {
            this.scale = scale;
         }
      }

   }
}
