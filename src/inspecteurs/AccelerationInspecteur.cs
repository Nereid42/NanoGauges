using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace Nereid
{
   namespace NanoGauges
   {
      public class AccelerationInspecteur : Inspecteur
      {
         private static readonly double MIN_INTERVAL = 0.10;

         // acceleration
         private readonly Measurement velocity = new Measurement(1);
         private readonly Measurement horizontalVelocity = new Measurement(1);
         private readonly Measurement verticalVelocity = new Measurement(1);

         public AccelerationInspecteur()
            : base(2, MIN_INTERVAL)
         {
            Reset();
         }


         public override void Reset()
         {
            this.velocity.Reset();
            this.horizontalVelocity.Reset();
            this.verticalVelocity.Reset();
         }


         protected override void ScanVessel(Vessel vessel)
         {
            // nothing to scan
         }

         protected override void Inspect(Vessel vessel)
         {
            if(vessel!=null)
            {
               velocity.value = vessel.srfSpeed;
               horizontalVelocity.value = vessel.horizontalSrfSpeed;
               verticalVelocity.value = vessel.verticalSpeed;
            }
         }

         public double HorizontalAcceleration()
         {
            return horizontalVelocity.ChangePerSecond;
         }

         public double VerticalAcceleration()
         {
            return verticalVelocity.ChangePerSecond;
         }


         public double Acceleration()
         {
            return velocity.ChangePerSecond;
         }
      }
   }
}
