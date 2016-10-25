using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nereid
{
   namespace NanoGauges
   {
      public static class NavGlobals
      {
         public static readonly Runway RUNWAY_090_SPACECENTER = new Runway("RWY 090", Coords.Create(-74.7130, -0.0486), 65.75f, 90.0f);
         public static readonly Runway RUNWAY_270_SPACECENTER = new Runway("RWY 270", Coords.Create(-74.5039, -0.0501), 65.75f, 270.0f);

         public static readonly Airport AIRPORT_SPACECENTER = new Airport("Space Center", RUNWAY_090_SPACECENTER, RUNWAY_270_SPACECENTER);

         public static Airport destinationAirport { get; private set; }
         public static Runway landingRunway { get; private set; }
         public static bool ILS { get; private set; }

         public static double distanceToRunway { get; private set; }
         public static double bearingToRunway { get; private set; }
         public static double verticalGlideslopeDeviation { get; private set; }
         public static double horizontalGlideslopeDeviation { get; private set; }

         static NavGlobals()
         {
            destinationAirport = AIRPORT_SPACECENTER;
         }
         

         public static void ResetNavigation()
         {
            Log.Info("reset of nav point");
            destinationAirport = null;
            landingRunway = null;
            ILS = false;
            distanceToRunway = double.MaxValue;
            verticalGlideslopeDeviation = double.MaxValue;
            horizontalGlideslopeDeviation = double.MaxValue;
         }

         public static void Update()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel == null) return;

            if(destinationAirport == null)  return;

            double distanceToAirport = NavUtils.DistanceToAirport(vessel, destinationAirport);
            double bearingToAirport = NavUtils.InitialBearingToAirport(vessel, destinationAirport);

            // do this not to often
            if(landingRunway==null || distanceToAirport> 2000)
            {
               landingRunway = destinationAirport.GetLandingRunwayForBearing(bearingToAirport);
            }

            // Bearing to runway
            bearingToRunway = NavUtils.InitialBearingToRunway(vessel, landingRunway);
            if (bearingToRunway < 0)
            {
               bearingToRunway = 360 - bearingToRunway;
            }

            // distance to runway
            distanceToRunway = NavUtils.DistanceToRunway(vessel, landingRunway);

            // glide slope
            horizontalGlideslopeDeviation = NavUtils.HorizontalGlideSlopeDeviation(vessel, landingRunway);
            if (NavUtils.HeadingDeviation(landingRunway.To, bearingToRunway) < 0)
            {
               horizontalGlideslopeDeviation = -horizontalGlideslopeDeviation;
            }
            verticalGlideslopeDeviation = NavUtils.VerticalGlideSlopeDeviation(vessel, landingRunway);

         }

         public static void SetDestinationAirport(Airport airport)
         {
            if(airport!=null)
            {
               Log.Info("set navigation airport to "+airport);
               destinationAirport = airport;
               Update();
            }
            else
            {
               Log.Info("set navigation airport cleared");
               ResetNavigation();
            }
         }
      }
   }

}
