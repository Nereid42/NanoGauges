using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {
      // NOT IMPLEMENTED 
      public abstract class TrippleVerticalGauge : VerticalGauge
      {
         public TrippleVerticalGauge(int id, Texture2D skin, Texture2D scale, bool damped = true, float dampfactor = 0.002f)
            : base(id, skin, scale, damped, dampfactor)
         {
         }

      }
   }
}
