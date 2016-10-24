using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {
      
      public class GlideGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/GLIDE-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/GLIDE-scale");

         private readonly DigitalDisplay DmeDisplay;

         private Runway runway = NavGlobals.RUNWAY_090_SPACECENTER;

         private readonly Texture2D NEEDLE_YELLOW = Utils.GetTexture("Nereid/NanoGauges/Resource/YELLOW-vertical-needle");
         private Needle yellowNeedle;



         public GlideGauge()
            : base(Constants.WINDOW_ID_GAUGE_GLIDE, SKIN, SCALE)
         {
            this.DmeDisplay = new DigitalDisplay(this);
            this.yellowNeedle = new Needle(this, NEEDLE_YELLOW);
            this.yellowNeedle.enabled = true;
         }

         public override string GetName()
         {
            return "ILS/DME";
         }

         public override string GetDescription()
         {
            return "Landing Glide path and distance to runway";
         }

         protected override void DrawInternalScale()
         {
            // TODO: to NavGlobals
            Vessel vessel = FlightGlobals.ActiveVessel;
            float d = NavUtils.DistanceToRunway(vessel, NavGlobals.RUNWAY_090_SPACECENTER);
            float slopeAltitude = runway.slopeTangens * d + runway.elevation;
            float deviation = (slopeAltitude - (float)vessel.altitude);
            yellowNeedle.Draw(deviation / 4.0f);
         }

         protected override void DrawOverlay()
         {
            // TODO: to NavGlobals
            float d = NavUtils.DistanceToRunway(FlightGlobals.ActiveVessel, NavGlobals.RUNWAY_090_SPACECENTER);
            Log.Test("Distance to rwy: "+d.ToString("0.0"));
            int dme = (int)(d/1000.0f);
            DmeDisplay.SetValue(dme);

            float h = GetHeight();
            float w = GetWidth();
            float x = w - DmeDisplay.GetWidth();
            float y = h - DmeDisplay.GetHeight();
            DmeDisplay.Draw(x, y);
         }

         protected override float GetScaleOffset()
         {
            float b = GetOffset(200);
            float y = b;

            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && IsOn())
            {
               Vector3 forward = vessel.GetTransform().up;
               double glide = 90.0 - Vector3.Angle(forward, vessel.upAxis);


               y = b + 90.0f*((float)glide/90.0f) / (float)SCALE_HEIGHT;
            }
            return y;
         }

         public override string ToString()
         {
            return "Gauge:GLIDE";
         }


         private class Needle : Sprite
         {
            public float degrees = 0.0f;
            public bool enabled = false;

            private readonly Damper traverseDamper;

            private bool traversing = false;

            public Needle(AbstractGauge gauge, Texture2D texture)
               : base(gauge, texture)
            {
               traverseDamper = new Damper(1.0f, int.MinValue, int.MaxValue);
            }

            public void Draw(float degrees)
            {
               if (enabled)
               {
                  float y = gauge.GetHeight() / 2 - degrees;
                  Log.Test("y=" + y);
                  float margin = gauge.GetHeight() / 6.0f;
                  float ymin = margin;
                  float ymax = gauge.GetHeight() - margin - GetHeight();
                  if (y <= ymin || y > ymax)
                  {
                     float limit = (y <= ymin) ? ymin : ymax;
                     if (!traversing)
                     {
                        traverseDamper.SetValue(limit);
                        traversing = true;
                     }
                     traverseDamper.Set(limit);
                     Draw(0, traverseDamper.Get());
                  }
                  else
                  {
                     traversing = false;
                     Draw(0, y);
                  }
               }
            }
         }
      }
   }
}
