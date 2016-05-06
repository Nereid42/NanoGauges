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

         CBAttributeMapSO.MapAttribute mapAttribute;

         public BiomeInspecteur()
            : base(6)
         {
            Reset();
         }

         public override void Reset()
         {
         }

         protected override void ScanVessel(Vessel vessel)
         {
            // nothing to scan
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
