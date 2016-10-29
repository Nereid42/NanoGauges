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

         protected void SetDme(double distance)
         {
            int dme = (int)(distance / 1000.0);
            if (dme <= 99)
            {
               DmeDisplay.SetValue(dme);
            }
            else
            {
               DmeDisplay.SetValue(99);
            }
         }

         protected override void AjustValues()
         {
            base.AjustValues();
            Vessel vessel = FlightGlobals.ActiveVessel;

            if (NavGlobals.InBeam || IsOff() || NavGlobals.destinationAirfield == null)
            {
               InLimits();
            }
            else
            {
               NotInLimits();
            }

            if(IsOn() && vessel != null)
            {
               if (NavGlobals.landingRunway != null) 
               {
                  // we have a runway 
                  //
                  if(IsInLimits())
                  {
                     // on glide slope
                     yellowNeedle.degrees = (float)(-NavGlobals.verticalGlideslopeDeviation * 50.0);
                  }
                  else
                  {
                     // not on glide slope
                     yellowNeedle.degrees = -200.0f;
                  }
                  //
                  // DME
                  double d = NavUtils.DistanceToRunway(vessel, NavGlobals.landingRunway);
                  SetDme(d);
               }
               else
               {
                  // no runway
                  //
                  DmeDisplay.SetValue(99);
                  yellowNeedle.degrees = -200.0f;
               }
            }
            else
            {
               // gaue is off or no vessel
               DmeDisplay.SetValue(99);
               yellowNeedle.degrees = -200.0f;
            }

         }

         protected override void DrawInternalScale()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            yellowNeedle.Draw();
         }

         protected override void DrawOverlay()
         {
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

            if(vessel==null || vessel.isEVA || vessel.mainBody==null || vessel.altitude > vessel.mainBody.Radius/2)
            {
               Off();
               return y;
            }
            else
            { 
               On(); 
            }

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


         public override void OnGaugeScalingChanged()
         {
            base.OnGaugeScalingChanged();
            //
            float scaling = (float)NanoGauges.configuration.gaugeScaling;
            this.yellowNeedle.Resize((float)NEEDLE_YELLOW.width * scaling, (float)NEEDLE_YELLOW.height * scaling);

         }

         private class Needle : Sprite
         {
            public float degrees = 0.0f;
            public bool enabled = false;

            private readonly Damper traverseDamper;
            private bool traversing = false;
            // offset to draw at center off needle
            private float offset;

            public Needle(AbstractGauge gauge, Texture2D texture)
               : base(gauge, texture)
            {
               traverseDamper = new Damper(1.0f, int.MinValue, int.MaxValue);
               this.offset = (float)texture.height / 2.0f;
            }

            public override void Resize(float width, float height)
            {
               base.Resize(width, height);
               this.offset = height / 2.0f;
            }

            public void Draw()
            {
               if (enabled)
               {
                  float y = gauge.GetHeight() / 2 - degrees;
                  float topmargin = gauge.GetHeight() / 5.5f;
                  float bottommargin = gauge.GetHeight() / 12.0f;
                  float ymin = topmargin;
                  float ymax = gauge.GetHeight() - bottommargin - GetHeight();
                  if (y <= ymin || y > ymax)
                  {
                     float limit = (y <= ymin) ? ymin : ymax;
                     if (!traversing)
                     {
                        traverseDamper.SetValue(limit);
                        traversing = true;
                     }
                     traverseDamper.Set(limit);
                     Draw(0, traverseDamper.Get() - this.offset);
                  }
                  else
                  {
                     traversing = false;
                     Draw(0, y - this.offset);
                  }
               }
            }
         }
      }
   }
}
