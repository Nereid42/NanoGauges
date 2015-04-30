using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {
      class Resources
      {
         private static readonly Dictionary<String, PartResourceDefinition> resources = new Dictionary<String, PartResourceDefinition>();

         public static readonly PartResourceDefinition LIQUID_FUEL;
         public static readonly PartResourceDefinition XENON_GAS;
         public static readonly PartResourceDefinition SOLID_FUEL;
         public static readonly PartResourceDefinition ELECTRIC_CHARGE;
         public static readonly PartResourceDefinition INTAKE_AIR;
         public static readonly PartResourceDefinition OXIDIZER;
         public static readonly PartResourceDefinition MONOPROPELLANT;
         public static readonly PartResourceDefinition EVA_PROPELLANT;
         public static readonly PartResourceDefinition ABLATOR;
         public static readonly PartResourceDefinition ORE;
         // TAC life support
         public static readonly PartResourceDefinition FOOD;
         public static readonly PartResourceDefinition WATER;
         public static readonly PartResourceDefinition OXYGEN;
         public static readonly PartResourceDefinition CARBONDIOXIDE;
         public static readonly PartResourceDefinition WASTE;
         public static readonly PartResourceDefinition WASTEWATER;
         // Kethane
         public static readonly PartResourceDefinition KETHANE;
         public static readonly PartResourceDefinition KINTAKE_AIR;
         // Deadly Reentry
         public static readonly PartResourceDefinition ABLATIVE_SHIELDING;


         static Resources()
         {
            Log.Info("defining resources");
            try
            {
               LoadResources();
               Log.Info("defining stock resources");
               LIQUID_FUEL = resources[Constants.RESOURCE_NAME_LIQUID_FUEL];
               XENON_GAS = resources[Constants.RESOURCE_NAME_XENON_GAS];
               SOLID_FUEL = resources[Constants.RESOURCE_NAME_SOLID_FUEL];
               ELECTRIC_CHARGE = resources[Constants.RESOURCE_NAME_ELECTRIC_CHARGE];
               INTAKE_AIR = resources[Constants.RESOURCE_NAME_INTAKE_AIR];
               OXIDIZER = resources[Constants.RESOURCE_NAME_OXIDIZER];
               MONOPROPELLANT = resources[Constants.RESOURCE_NAME_MONOPROPELLANT];
               EVA_PROPELLANT = resources[Constants.RESOURCE_NAME_EVA_PROPELLANT];
               ABLATOR = resources[Constants.RESOURCE_NAME_ABLATOR];
               ORE = resources[Constants.RESOURCE_NAME_ORE];
               Log.Info("defining TAC life support resources");
               FOOD = OptionalResource(Constants.RESOURCE_NAME_FOOD);
               WATER = OptionalResource(Constants.RESOURCE_NAME_WATER);
               CARBONDIOXIDE = OptionalResource(Constants.RESOURCE_NAME_CARBONDIOXIDE);
               WASTE = OptionalResource(Constants.RESOURCE_NAME_WASTE);
               WASTEWATER = OptionalResource(Constants.RESOURCE_NAME_WASTEWATER);
               OXYGEN = OptionalResource(Constants.RESOURCE_NAME_OXYGEN);
               Log.Info("defining Khetane resources");
               KETHANE = OptionalResource(Constants.RESOURCE_NAME_KETHANE);
               KINTAKE_AIR = OptionalResource(Constants.RESOURCE_NAME_KINTAKE_AIR);
               Log.Info("defining Deadly Reentry resources");
               ABLATIVE_SHIELDING = OptionalResource(Constants.RESOURCE_NAME_ABLATIVE_SHIELDING);
               Log.Info("resources completely defined");
            }
            catch
            {
               Log.Error("defining resources failed");
               throw;
            }
         }

         private static PartResourceDefinition OptionalResource(String name)
         {
            if(resources.ContainsKey(name))
            {
               Log.Info("optional resource '" + name + "' found");
               return resources[name];
            }
            else
            {
               Log.Info("optional resource '"+name+"' not found");
               return null;
            }
         }

         public static void LoadResources()
         {
            Log.Info("loading resources");
            foreach (PartResourceDefinition item in PartResourceLibrary.Instance.resourceDefinitions)
            {
               Log.Info("loading resource " + item.name+" of id "+item.id);
               if(!resources.ContainsKey(item.name))
               {
                  resources.Add(item.name,item);
               }
            }
            Log.Info("loading resources done");
         }

      }
   }
}
