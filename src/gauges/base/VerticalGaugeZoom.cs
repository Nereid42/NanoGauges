using System;
using System.Collections.Generic;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {
      class VerticalGaugeZoom 
      {
         private const float ZOOM = 2.0f;
         private const int ID = 8777; // the window id doesn't matter

         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/ZOOM-skin");

         private readonly VerticalGauge gauge;

         private Rect skinBounds = new Rect(0, 0, 0, 0);
         //
         private readonly Texture2D skin;
         private Rect gaugeSkinBounds = new Rect(0, 0, 0, 0);
         private readonly Texture2D scale;
         private Rect gaugeScaleBounds = new Rect(0, 0, VerticalGauge.SCALE_WIDTH*ZOOM, VerticalGauge.SCALE_HEIGHT*ZOOM);

         public float value { get; set; }

         protected Rect bounds = new Rect(0, 0, 0, 0);

         public VerticalGaugeZoom(VerticalGauge gauge, Texture2D skin, Texture2D scale)
         {
            this.value = 0.0f;
            this.gauge = gauge;
            this.bounds.width = (float)Math.Truncate(gauge.GetWidth() * ZOOM);
            this.bounds.height = (float)Math.Truncate(gauge.GetHeight() / 3.0f);
            this.scale = scale;
            this.skin = skin;
            this.gaugeSkinBounds.width = (float)Math.Truncate(gauge.GetWidth() * ZOOM);
            this.gaugeSkinBounds.height = (float)Math.Truncate(gauge.GetHeight() * ZOOM);
            //
            this.skinBounds.width = this.bounds.width;
            this.skinBounds.height = this.bounds.height;
         }

         public void Draw()
         {
            bounds.x = gauge.GetX() - bounds.width/2 + gauge.GetWidth() / 2;
            bounds.y = gauge.GetY() + (float)gauge.GetHeight() / 2 - bounds.height / 2 ;
            bounds = GUI.Window(ID, bounds, OnWindowInternal, "", GUI.skin.window);
         }

         private void OnWindowInternal(int id)
         {
            float pixelOffsetInGauge = VerticalGauge.SCALE_HEIGHT * value;
            float pixelOffsetInZoom = (pixelOffsetInGauge + gauge.GetHeight()/2 + bounds.height/2/ZOOM);
            float offset = pixelOffsetInZoom / (float)VerticalGauge.SCALE_HEIGHT;
            Rect scaleoff = new Rect(0, offset, 1.0f, 1.0f); // bounds.height / (float)VerticalGauge.SCALE_HEIGHT);
            // scale
            GUI.DrawTextureWithTexCoords(gaugeScaleBounds, scale, scaleoff, false);
            // skin
            Rect skinoff = new Rect(0, 0.5f + 1f / 6f / ZOOM, 1.0f, 1.0f); // ZOOM, bounds.height / gauge.GetHeight());
            GUI.DrawTextureWithTexCoords(gaugeSkinBounds, skin, skinoff, true);
            //
            // zoom skin
            GUI.DrawTexture(skinBounds, SKIN);
         }

      }
   }
}
