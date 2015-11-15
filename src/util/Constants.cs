using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Nereid
{
   namespace NanoGauges
   {
      static class Constants
      {
         private const int WINDOW_ID_BASE = 19280;
         public const int WINDOW_ID_TOOLTIP = WINDOW_ID_BASE + 1;
         public const int WINDOW_ID_ABOUT = WINDOW_ID_BASE + 3;
         public const int WINDOW_ID_CONFIG = WINDOW_ID_BASE + 4;
         //
         public const int WINDOW_ID_GAUGE_VSI = WINDOW_ID_BASE + 8;
         public const int WINDOW_ID_GAUGE_RADAR_ALTIMETER = WINDOW_ID_BASE + 9;
         public const int WINDOW_ID_GAUGE_MASS = WINDOW_ID_BASE + 10;
         public const int WINDOW_ID_GAUGE_HSPD = WINDOW_ID_BASE + 11;
         public const int WINDOW_ID_GAUGE_FUEL = WINDOW_ID_BASE + 12;
         public const int WINDOW_ID_GAUGE_FLOW = WINDOW_ID_BASE + 13;
         public const int WINDOW_ID_GAUGE_CHARGE = WINDOW_ID_BASE + 14;
         public const int WINDOW_ID_GAUGE_AMP = WINDOW_ID_BASE + 15;
         public const int WINDOW_ID_GAUGE_G = WINDOW_ID_BASE + 16;
         public const int WINDOW_ID_GAUGE_ORBIT = WINDOW_ID_BASE + 17;
         public const int WINDOW_ID_GAUGE_MONO = WINDOW_ID_BASE + 18;
         public const int WINDOW_ID_GAUGE_MFLOW = WINDOW_ID_BASE + 19;
         public const int WINDOW_ID_GAUGE_OXID = WINDOW_ID_BASE + 20;
         public const int WINDOW_ID_GAUGE_ATM = WINDOW_ID_BASE + 21;
         public const int WINDOW_ID_GAUGE_APA = WINDOW_ID_BASE + 22;
         public const int WINDOW_ID_GAUGE_PEA = WINDOW_ID_BASE + 23;
         public const int WINDOW_ID_GAUGE_AIRIN = WINDOW_ID_BASE + 24;
         public const int WINDOW_ID_GAUGE_AIRPCT = WINDOW_ID_BASE + 25;
         public const int WINDOW_ID_GAUGE_XENON = WINDOW_ID_BASE + 26;
         public const int WINDOW_ID_GAUGE_THRUST = WINDOW_ID_BASE + 27;
         public const int WINDOW_ID_GAUGE_TWR = WINDOW_ID_BASE + 28;
         public const int WINDOW_ID_GAUGE_SPD = WINDOW_ID_BASE + 29;
         public const int WINDOW_ID_GAUGE_AOA = WINDOW_ID_BASE + 30;
         public const int WINDOW_ID_GAUGE_DTGT = WINDOW_ID_BASE + 31;
         public const int WINDOW_ID_GAUGE_INCL = WINDOW_ID_BASE + 32;
         public const int WINDOW_ID_GAUGE_DISP = WINDOW_ID_BASE + 33;
         public const int WINDOW_ID_GAUGE_SETS = WINDOW_ID_BASE + 34;
         public const int WINDOW_ID_GAUGE_ISPE = WINDOW_ID_BASE + 35;
         public const int WINDOW_ID_GAUGE_TEMP = WINDOW_ID_BASE + 36;
         public const int WINDOW_ID_GAUGE_O2 = WINDOW_ID_BASE + 37;
         public const int WINDOW_ID_GAUGE_CO2 = WINDOW_ID_BASE + 38;
         public const int WINDOW_ID_GAUGE_H2O = WINDOW_ID_BASE + 39;
         public const int WINDOW_ID_GAUGE_WH2O = WINDOW_ID_BASE + 40;
         public const int WINDOW_ID_GAUGE_WASTE = WINDOW_ID_BASE + 41;
         public const int WINDOW_ID_GAUGE_FOOD = WINDOW_ID_BASE + 42;
         public const int WINDOW_ID_GAUGE_EVAMP = WINDOW_ID_BASE + 43;
         public const int WINDOW_ID_GAUGE_KETHANE = WINDOW_ID_BASE + 44;
         public const int WINDOW_ID_GAUGE_KAIRIN = WINDOW_ID_BASE + 45;
         public const int WINDOW_ID_GAUGE_MAXG = WINDOW_ID_BASE + 46;
         public const int WINDOW_ID_GAUGE_SRB = WINDOW_ID_BASE + 47;
         public const int WINDOW_ID_GAUGE_OSPD = WINDOW_ID_BASE + 48;
         public const int WINDOW_ID_GAUGE_GRAV = WINDOW_ID_BASE + 49;
         public const int WINDOW_ID_GAUGE_VAI = WINDOW_ID_BASE + 50;
         public const int WINDOW_ID_GAUGE_VVI = WINDOW_ID_BASE + 51;
         public const int WINDOW_ID_GAUGE_VT = WINDOW_ID_BASE + 52;
         public const int WINDOW_ID_GAUGE_VTGT = WINDOW_ID_BASE + 53;
         public const int WINDOW_ID_GAUGE_CAM = WINDOW_ID_BASE + 54;
         public const int WINDOW_ID_GAUGE_MACH = WINDOW_ID_BASE + 55;
         public const int WINDOW_ID_GAUGE_SHIELD = WINDOW_ID_BASE + 56;
         public const int WINDOW_ID_GAUGE_Q = WINDOW_ID_BASE + 57;
         public const int WINDOW_ID_GAUGE_HEAT = WINDOW_ID_BASE + 58;
         public const int WINDOW_ID_GAUGE_IMPACT = WINDOW_ID_BASE + 59;
         public const int WINDOW_ID_GAUGE_ALTIMETER = WINDOW_ID_BASE + 60;
         public const int WINDOW_ID_GAUGE_ACCL = WINDOW_ID_BASE + 61;
         public const int WINDOW_ID_GAUGE_HACCL = WINDOW_ID_BASE + 62;
         public const int WINDOW_ID_GAUGE_VACCL = WINDOW_ID_BASE + 63;
         public const int WINDOW_ID_GAUGE_EXTTEMP = WINDOW_ID_BASE + 64;
         public const int WINDOW_ID_GAUGE_ATMTEMP = WINDOW_ID_BASE + 65;
         public const int WINDOW_ID_GAUGE_ABLAT = WINDOW_ID_BASE + 66;
         public const int WINDOW_ID_GAUGE_ORE = WINDOW_ID_BASE + 67;
         public const int WINDOW_ID_GAUGE_DRILLTEMP = WINDOW_ID_BASE + 68;
         public const int WINDOW_ID_GAUGE_BIOME = WINDOW_ID_BASE + 69;
         public const int WINDOW_ID_GAUGE_LATITUDE = WINDOW_ID_BASE + 70;
         public const int WINDOW_ID_GAUGE_LONGITUDE = WINDOW_ID_BASE + 71;
         public const int WINDOW_ID_GAUGE_INDICATOR = WINDOW_ID_BASE + 72;
         public const int WINDOW_ID_GAUGE_PROPELLANT = WINDOW_ID_BASE + 73;

         public const String RESOURCE_NAME_LIQUID_FUEL = "LiquidFuel";
         public const String RESOURCE_NAME_XENON_GAS = "XenonGas";
         public const String RESOURCE_NAME_SOLID_FUEL = "SolidFuel";
         public const String RESOURCE_NAME_ELECTRIC_CHARGE = "ElectricCharge";
         public const String RESOURCE_NAME_INTAKE_AIR = "IntakeAir";
         public const String RESOURCE_NAME_OXIDIZER = "Oxidizer";
         public const String RESOURCE_NAME_MONOPROPELLANT = "MonoPropellant";
         public const String RESOURCE_NAME_EVA_PROPELLANT = "EVA Propellant";
         public const String RESOURCE_NAME_ABLATOR = "Ablator";
         public const String RESOURCE_NAME_ORE = "Ore";
         // TAC Life Support
         public const String RESOURCE_NAME_FOOD = "Food";
         public const String RESOURCE_NAME_WATER = "Water";
         public const String RESOURCE_NAME_OXYGEN = "Oxygen";
         public const String RESOURCE_NAME_CARBONDIOXIDE = "CarbonDioxide";
         public const String RESOURCE_NAME_WASTE = "Waste";
         public const String RESOURCE_NAME_WASTEWATER = "WasteWater";
         // Kethane
         public const String RESOURCE_NAME_KETHANE = "Kethane";
         public const String RESOURCE_NAME_KINTAKE_AIR = "KIntakeAir";
         // Deadly Reentry
         public const String RESOURCE_NAME_ABLATIVE_SHIELDING = "AblativeShielding";


         public const double GEE_KERBIN = 9.81;

         public const double G = 6.674e-11;
         public const double ATM = 1.2230948554874;
         public const double MIN_TEMP = -273.15;

         public const long SECONDS_PER_MINUTE = 60;
         public const long MINUTES_PER_HOUR = 60;
         public const long SECONDS_PER_HOUR = MINUTES_PER_HOUR * SECONDS_PER_MINUTE;
      }
   }
}
