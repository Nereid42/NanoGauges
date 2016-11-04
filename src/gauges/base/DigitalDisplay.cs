using System;
using UnityEngine;

namespace Nereid
{
   namespace NanoGauges
   {
      // WIP
      public class DigitalDisplay : Sprite
      {
         private static readonly Texture2D BACKGROUND = Utils.GetTexture("Nereid/NanoGauges/Resource/DIGIT-background");

         private const int BASE = 10;
         private const int WIDTH_PER_DIGIT = 13;
         private const int HEIGHT = 21;


         private readonly Sprite[] digits;
         private readonly Sprite seg7;
         private readonly Sprite seg7I;
         private readonly Sprite seg7H;

         private const int BORDER = 0;
         private readonly int numberOfDigits;


         private int value;
         private readonly int limit;
         private float widthOfDigit;
         private bool leadingZeros;

         private float scaling = 1.0f;

         public DigitalDisplay(AbstractGauge gauge, int numberOfDigits = 2, bool leadingZeros = true)
            : base(gauge,BACKGROUND)
         {
            this.numberOfDigits = numberOfDigits;
            this.leadingZeros = leadingZeros;
            digits = new Sprite[10];
            for(int i=0; i<10; i++)
            {
               digits[i] = new Sprite(gauge,Utils.GetTexture("Nereid/NanoGauges/Resource/"+i+"-digit"));
            }
            seg7 = new Sprite(gauge, Utils.GetTexture("Nereid/NanoGauges/Resource/digit"));
            seg7I = new Sprite(gauge, Utils.GetTexture("Nereid/NanoGauges/Resource/I-digit"));
            seg7H = new Sprite(gauge, Utils.GetTexture("Nereid/NanoGauges/Resource/H-digit"));
            limit = (int)Math.Pow(10, numberOfDigits);
            this.scaling = (float)NanoGauges.configuration.gaugeScaling;
            this.widthOfDigit = WIDTH_PER_DIGIT * this.scaling;
            SetScaling(this.scaling);
         }

         public override void Draw(float x = 0, float y = 0)
         {
            base.Draw(x, y);

            float xdigits = x + BORDER;
            float ydigits = y + BORDER;

            bool drawLeadingZeros = leadingZeros;

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
                  if (drawLeadingZeros || digit > 0 || i==numberOfDigits-1)
                  {
                     digits[digit].Draw(xdigits + i * widthOfDigit, ydigits);
                     drawLeadingZeros = true;
                  }
                  else
                  {
                     seg7.Draw(xdigits + i * widthOfDigit, ydigits);
                  }
               }
            }
            else
            {
               for (int i = 0; i < numberOfDigits-2; i++)
               {
                  seg7.Draw(xdigits + i * widthOfDigit, ydigits);
               }
               float x0 = xdigits + (numberOfDigits - 2) * widthOfDigit;

               seg7H.Draw(x0, ydigits);
               if(numberOfDigits>1)
               {
                  seg7I.Draw(x0 + widthOfDigit, ydigits);
               }
            }
         }

         public void SetValue(int value)
         {
            this.value = value;
         }

         public void SetScaling(float scaling)
         {
            this.scaling = scaling;
            this.widthOfDigit = WIDTH_PER_DIGIT * scaling;
            Resize(scaling * (WIDTH_PER_DIGIT * numberOfDigits), scaling * HEIGHT);
            for (int i = 0; i < 10; i++)
            {
               digits[i].Resize(WIDTH_PER_DIGIT*scaling,HEIGHT*scaling);
            }
            seg7.Resize(WIDTH_PER_DIGIT * scaling, HEIGHT * scaling);
            seg7H.Resize(WIDTH_PER_DIGIT * scaling, HEIGHT * scaling);
            seg7I.Resize(WIDTH_PER_DIGIT * scaling, HEIGHT * scaling);
         }
      }

   }
}
