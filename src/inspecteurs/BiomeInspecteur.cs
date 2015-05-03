using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

namespace Nereid
{
   namespace NanoGauges
   {
      class BiomeInspecteur : Inspecteur
      {
         private static readonly double MIN_INTERVAL = 0.25;

         CBAttributeMapSO.MapAttribute mapAttribute;

         public BiomeInspecteur()
            : base(8, MIN_INTERVAL)
         {
            Reset();
         }

         public override void Reset()
         {
         }

         protected override void ScanVessel(Vessel vessel)
         {
         }

         protected override void Inspect(Vessel vessel)
         {
            if (vessel != null)
            {
               double lat = vessel.latitude * Math.PI / 180d;
               double lon = vessel.longitude * Math.PI / 180d;
               CelestialBody body = vessel.mainBody;
               if (body != null)
               {
                  CBAttributeMapSO biomeMap = body.BiomeMap;
                  if(biomeMap!=null)
                  {
                     mapAttribute = biomeMap.GetAtt(lat, lon);
                  }
               }
            }
         }

         public String GetBiomeName()
         {
            if (mapAttribute != null)
            {
               return mapAttribute.name;
            }
            else
            {
               return "- no biome -";
            }
         }
      }
   }
}
