using System;
using UnityEngine;

namespace Nereid
{
   namespace NanoGauges
   {
      public abstract class Flag
      {
         private readonly AbstractGauge gauge;
         private readonly Texture2D skin;
         private bool visible = false;
         private float offset;
         private readonly double max_offset;
         private float d;
         private readonly float a;

         private STATE state;

         enum STATE { UP, DOWN };


         public Flag(AbstractGauge gauge, Texture2D skin, float a = 0.260f)
         {
            this.gauge = gauge;
            this.skin = skin;
            this.max_offset = skin.height;
            this.a = a;
            this.d = 0;
            this.state = STATE.UP;
         }

         public void Down()
         {
            this.state = STATE.DOWN;
         }

         public void Up()
         {
            this.state = STATE.UP;
         }

         public bool IsUp()
         {
            return state == STATE.UP;
         }

         public bool IsDown()
         {
            return state == STATE.DOWN;
         }


         protected void Next()
         {
            if(state==STATE.UP)
            {
               offset -= d;
               if (d < 2) d += a;
               if (offset < 0) offset = 0;
            }
            else
            {
               offset += d;
               if (d < 2) d += a;
               if (offset > skin.height) offset = skin.height;
            }
         }

         public void SetVisible(bool visible)
         {
            this.visible = visible;
         }

         public void Draw(float x = 0, float y = 0)
         {
            float gw = (float)gauge.GetWidth();
            float gh = (float)gauge.GetHeight();
            float sw = (float)skin.width;
            float sh = (float)skin.height;
            float w = sw / gw;
            float h = sh / gh;
            Rect off = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
            Rect skinBounds = new Rect(x, y - sh + offset, sw, sh);
            GUI.DrawTextureWithTexCoords(skinBounds, skin, off, false);
            Next();
         }

      }

      public class PowerOffFlag : Flag
      {
         private static Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/OFF-flag");

         public PowerOffFlag(AbstractGauge gauge)
            : base(gauge, SKIN)
         {

         }
      }

      public class LimiterFlag : Flag
      {
         private static Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/LIMIT-flag");

         public LimiterFlag(AbstractGauge gauge)
            : base(gauge, SKIN)
         {

         }
      }

   }
}
