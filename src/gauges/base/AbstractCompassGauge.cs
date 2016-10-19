using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public abstract class AbstractCompassGauge : HorizontalGauge
      {
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/COMPASS00-scale");

         public AbstractCompassGauge(int id, Texture2D skin)
            : base(id, skin, SCALE)
         {
         }

         protected abstract float GetDegrees();

         protected override float GetScaleOffset()
         {
            if(!IsOn()) return 0;
            float v = GetDegrees() % 360;
            return v / (float)SCALE_WIDTH;
         }
      }
   }
}
