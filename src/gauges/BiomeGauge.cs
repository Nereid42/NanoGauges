using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {
      class BiomeGauge : HorizontalTextGauge        
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/BIOME-skin");
         private static readonly Texture2D BACK = Utils.GetTexture("Nereid/NanoGauges/Resource/BIOME-back");

         private readonly BiomeInspecteur inspecteur;

         public BiomeGauge(BiomeInspecteur inspecteur)
            : base(Constants.WINDOW_ID_GAUGE_BIOME,SKIN,BACK)
         {
            this.inspecteur = inspecteur;
         }

         public override string GetName()
         {
            return "Biome";
         }

         public override string GetDescription()
         {
            return "Biome the vessel flyes over.";
         }

         protected override String GetText()
         {
            return inspecteur.GetBiomeName();
         }
      }
   }
}
