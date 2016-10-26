using System;
using UnityEngine;

namespace Nereid
{
   namespace NanoGauges
   {
      // WIP
      public class Sprite
      {

         protected readonly AbstractGauge gauge;
         private readonly Texture2D skin;

         private Rect position = new Rect();
         private Rect texCoords = new Rect(0.0f, 0.0f, 1.0f, 1.0f);

         public Sprite(AbstractGauge gauge, Texture2D skin, int x=0, int y=0)
         {
            this.gauge = gauge;
            this.skin = skin;
            this.position.x = x;
            this.position.y = y;
         }


         public float GetWidth()
         {
            return skin.width;
         }

         public float GetHeight()
         {
            return skin.height;
         }


         public virtual void Draw(float x = 0, float y = 0)
         {
            float gw = (float)gauge.GetWidth();
            float gh = (float)gauge.GetHeight();
            float sw = (float)skin.width;
            float sh = (float)skin.height;
            float w = sw / gw;
            float h = sh / gh;

            position.x = x;
            position.y = y;
            position.width = sw;
            position.height = sh;
            GUI.DrawTextureWithTexCoords(position, skin, texCoords, true);
         }

      }

   }
}
