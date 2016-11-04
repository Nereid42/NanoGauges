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

         private float width;
         private float height;

         public Sprite(AbstractGauge gauge, Texture2D skin, int x=0, int y=0)
         {
            this.gauge = gauge;
            this.skin = skin;
            this.position.x = x;
            this.position.y = y;
            this.width = skin.width;
            this.height = skin.height;
         }


         public float GetWidth()
         {
            return this.width;
         }

         public float GetHeight()
         {
            return this.height;
         }

         public virtual void Resize(float width, float height)
         {
            this.width = width;
            this.height = height;
         }

         public virtual void Draw(float x = 0, float y = 0)
         {
            position.x = x;
            position.y = y;
            position.width = width;
            position.height = height;
            GUI.DrawTextureWithTexCoords(position, skin, texCoords, true);
         }

      }

   }
}
