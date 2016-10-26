using System;
using UnityEngine;

namespace Nereid
{
   namespace NanoGauges
   {
      // WIP
      public class Button
      {

         protected readonly AbstractGauge gauge;
         private readonly Texture2D skinOff;
         private readonly Texture2D skinOn;

         private Rect position = new Rect();
         private Rect texCoords = new Rect(0.0f, 0.0f, 1.0f, 1.0f);

         public bool pressed { get; private set; }
         private bool clicked;

         public Button(AbstractGauge gauge, Texture2D off, Texture2D on, float width, float height)
         {
            this.gauge = gauge;
            this.skinOff = off;
            this.skinOn = on;
            this.pressed = false;
            this.clicked = false;

            this.position.width = width; 
            this.position.height = height; 
         }


         public float GetWidth()
         {
            return this.position.width;
         }

         public float GetHeight()
         {
            return this.position.height;
         }


         public virtual bool Draw(float x = 0, float y = 0)
         {

            position.x = x;
            position.y = y;

            // Mouseclicks
            //bool clicked = false;
            Event e = Event.current;
            if (e.type == EventType.MouseDown && e.button == 0)
            {
               float xClick = e.mousePosition.x;
               float yClick = e.mousePosition.y;
               if (xClick >= x && xClick < x + position.width && yClick >= y && yClick < y + position.height)
               {
                  clicked = true;
                  e.Use();
               }
            }
            else if (clicked && e.type == EventType.MouseUp && e.button == 0)
            {
               clicked = false;
               e.Use();
            }
            else if (clicked)
            {
               float xMove = e.mousePosition.x;
               float yMove = e.mousePosition.y;
               if (xMove < x || xMove >= x + position.width || yMove < y || yMove >= y + position.height)
               {
                  clicked = false;
               }
            }

            if(pressed || clicked)
            {
               GUI.DrawTextureWithTexCoords(position, skinOn, texCoords, true);
               if(!pressed)
               {
                  pressed = true;
                  return true;
               }
               pressed = clicked;
            }
            else
            {
               GUI.DrawTextureWithTexCoords(position, skinOff, texCoords, true);
            }

            return false;
         }

      }

   }
}
