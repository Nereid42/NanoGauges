using System;
using UnityEngine;

namespace Nereid
{
   namespace NanoGauges
   {
      public class Flag
      {
         private const int Y_UP = 0;

         private readonly AbstractGauge gauge;
         private readonly Texture2D texture;
         private bool visible = false;
         private float offset;
         private readonly double max_offset;
         private float d;
         private readonly float a;

         private STATE state;

         enum STATE { UP, DOWN };

         private static readonly Rect texCoords = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
         private Rect position = new Rect();

         private int width;
         private int height;

         public Flag(AbstractGauge gauge, Texture2D texture, double scale = Configuration.GAUGE_SCALE_100, float a = 0.240f)
         {
            this.gauge = gauge;
            this.texture = texture;
            this.max_offset = texture.height;
            this.a = a;
            this.d = 0;
            this.state = STATE.UP;
            this.offset = Y_UP;
            this.width = (int)(texture.width * scale);
            this.height = (int)(texture.height * scale);
         }

         public void Set(bool down)
         {
            this.state = down ? STATE.DOWN : STATE.UP;
         }

         public void Down()
         {
            this.state = STATE.DOWN;
         }

         public int GetWidth()
         {
            return width;
         }

         public int GetHeight()
         {
            return height;
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

         public void ScaleTo(double scale)
         {
            this.width = (int)(texture.width * scale);
            this.height = (int)(texture.height * scale);
         }

         protected void Next()
         {
            if(state==STATE.UP)
            {
               offset -= d;
               if (d < 2) d += a;
               if (offset < Y_UP) offset = Y_UP;
            }
            else
            {
               offset += d;
               if (d < 2) d += a;
               if (offset > height) offset = height;
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
            float sw = this.width;
            float sh = this.height;
            float w = sw / gw;
            float h = sh / gh;

            position.x = x;
            position.y = y - sh + offset;
            position.width = sw;
            position.height = sh;
            GUI.DrawTextureWithTexCoords(position, texture, texCoords, false);
            Next();
         }

      }

      public class PowerOffFlag : Flag
      {
         private static Texture2D TEXTURE = Utils.GetTexture("Nereid/NanoGauges/Resource/OFF-flag");

         public PowerOffFlag(AbstractGauge gauge)
            : base(gauge, TEXTURE)
         {

         }
      }

      public class LimiterFlag : Flag
      {
         private static Texture2D TEXTURE = Utils.GetTexture("Nereid/NanoGauges/Resource/LIMIT-flag");

         public LimiterFlag(AbstractGauge gauge)
            : base(gauge, TEXTURE)
         {

         }
      }

      public class RFlag : Flag
      {
         private static Texture2D TEXTURE = Utils.GetTexture("Nereid/NanoGauges/Resource/R-flag");

         public RFlag(AbstractGauge gauge)
            : base(gauge, TEXTURE)
         {

         }
      }
   }
}
