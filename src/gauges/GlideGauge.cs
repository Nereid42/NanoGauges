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

         public GlideGauge()
            : base(Constants.WINDOW_ID_GAUGE_GLIDE, SKIN, SCALE)
         {
            this.DmeDisplay = new DigitalDisplay(this);
         }

         public override string GetName()
         {
            return "ILS/DME";
         }

         public override string GetDescription()
         {
            return "Landing Glide path and distance to runway";
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
            float m = GetOffset(250);
            float y = m;


            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null && IsOn())
            {
               Log.Test("D = " + NavUtils.DistanceToRunway(vessel, NavGlobals.RUNWAY_090_SPACECENTER));
            }
            return y;
         }


         public override string ToString()
         {
            return "Gauge:GLIDE";
         }
      }
   }
}
