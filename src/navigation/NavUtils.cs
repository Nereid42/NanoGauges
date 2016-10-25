using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nereid
{
   namespace NanoGauges
   {
      public static class NavUtils
      {
         public static double InitialBearingFromTo(double longitudeFrom, double latitudeFrom, double longitudeTo, double latitudeTo)
         {
            double phi1 = Utils.DegreeToRadians(latitudeFrom);
            double lambda1 = Utils.DegreeToRadians(longitudeFrom);
            double phi2 = Utils.DegreeToRadians(latitudeTo);
            double lambda2 = Utils.DegreeToRadians(longitudeTo);


            double dy = Math.Sin(lambda2 - lambda1) * Math.Cos(phi2);
            double dx = Math.Cos(phi1) * Math.Sin(phi2) - Math.Sin(phi1) * Math.Cos(phi2) * Math.Cos(lambda2 - lambda1);

            if (dx == 0)
            {
               if (lambda2 - lambda1 > 0) return 0.0f;
               return 180.0f;
            }

            double theta = Math.Atan2(dy, dx);
            return (Utils.RadiansToDegree(theta) + 360.0f) % 360.0f;
         }

         public static double InitialBearingToRunway(Vessel vessel, Runway runway)
         {
            return InitialBearingFromTo(vessel.longitude, vessel.latitude, runway.coords.longitude, runway.coords.latitude);
         }

         public static double InitialBearingToAirport(Vessel vessel, Airport airport)
         {
            return InitialBearingFromTo(vessel.longitude, vessel.latitude, airport.coords.longitude, airport.coords.latitude);
         }

         public static double BearingFromTo(double longitudeFrom, double latitudeFrom, double longitudeTo, double latitudeTo)
         {
            double phi1 = Utils.DegreeToRadians(latitudeFrom);
            double lambda1 = Utils.DegreeToRadians(longitudeFrom);
            double phi2 = Utils.DegreeToRadians(latitudeTo);
            double lambda2 = Utils.DegreeToRadians(longitudeTo);

            double dphi = Math.Log(Math.Tan(Math.PI / 4 + phi2 / 2) / Math.Tan(Math.PI / 4 + phi1 / 2));
            double dlambda = Math.Abs(lambda2 - lambda1);

            double theta = Math.Atan2(dlambda, dphi);
            double bearing = (Utils.RadiansToDegree(theta) + 360.0f) % 360.0f;
            return bearing;
         }

         public static double DistanceFromTo(double longitudeFrom, double latitudeFrom, double longitudeTo, double latitudeTo, double radius)
         {
            double phi1 = Utils.DegreeToRadians(latitudeFrom);
            double lambda1 = Utils.DegreeToRadians(longitudeFrom);
            double phi2 = Utils.DegreeToRadians(latitudeTo);
            double lambda2 = Utils.DegreeToRadians(longitudeTo);

            double dphi = phi2 - phi1;
            double dlamda = lambda2 - lambda1;
            double sphi = Math.Sin(dphi/2.0f);
            double slamda = Math.Sin(dlamda/2.0f);

            /*
            // haversine formula
            double a = sphi*sphi + Math.Cos(phi1)*Math.Cos(phi2)*slamda*slamda;
            // handle a=1
            double c = 2*Math.Atan2(Math.Sqrt(a),Math.Sqrt(1-a));
            return radius * c;
            */

            // less complex sperical law of Cosinus
            return Math.Acos ( Math.Sin(phi1)*Math.Sin(phi2) +Math.Cos(phi1)*Math.Cos(phi2)*Math.Cos(lambda2 - lambda1) ) * radius;
         }

         public static double DistanceFromVesselTo(Vessel vessel, Coords to)
         {
            return DistanceFromTo(vessel.longitude, vessel.latitude, to.longitude, to.latitude, vessel.mainBody.Radius);
         }


         public static double DistanceToRunway(Vessel vessel, Runway runway)
         {
            if (vessel == null) return float.MaxValue;
            return DistanceFromTo(vessel.longitude, vessel.latitude, runway.coords.longitude, runway.coords.latitude, vessel.mainBody.Radius);
         }

         public static double DistanceToAirport(Vessel vessel, Airport airport)
         {
            if (vessel == null) return float.MaxValue;
            return DistanceFromTo(vessel.longitude, vessel.latitude, airport.coords.longitude, airport.coords.latitude, vessel.mainBody.Radius);
         }

         public static Coords DestinationFromBearingAtDistance(double longitudeFrom, double latitudeFrom, double bearing, double distance, double radius)
         {
            /*double phi1 = Utils.DegreeToRadians(latitudeFrom);
            double lambda1 = Utils.DegreeToRadians(longitudeFrom);
            double theta = Utils.DegreeToRadians(bearing);
            double rho = distance / radius;

            double phi2 = Math.Asin( Math.Sin(phi1) * Math.Cos(rho) + Math.Cos(phi1) * Math.Sin(rho) * Math.Cos(theta) );
            double lambda2 = lambda1 + Math.Atan2(Math.Sin(theta) * Math.Sin(rho) * Math.Cos(phi1), Math.Cos(rho) - Math.Sin(phi1) * Math.Sin(phi2));*/

            double lambda1 = Utils.DegreeToRadians(longitudeFrom);
            double phi1 = Utils.DegreeToRadians(latitudeFrom);
            double theta = Utils.DegreeToRadians(bearing);
            double d = distance;
            double R = radius;

            double phi2 = Math.Asin(Math.Sin(phi1) * Math.Cos(d / R) + Math.Cos(phi1) * Math.Sin(d / R) * Math.Cos(theta));

            double y = Math.Sin(theta) * Math.Sin(d / R) * Math.Cos(phi1);
            double x = Math.Cos(d / R) - Math.Sin(phi1) * Math.Sin(phi2);

            double lambda2 = lambda1 + Math.Atan2(y, x);
           

            return new Coords( Utils.RadiansToDegree(lambda2), Utils.RadiansToDegree(phi2) );
         }

         public static Coords DestinationFromRunwayAtDistance(Runway runway, double distance, double radius)
         {
            return DestinationFromBearingAtDistance(runway.coords.longitude, runway.coords.latitude, runway.From, distance, radius);
         }

         public static double VerticalGlideSlopeDeviation(Vessel vessel, Runway runway)
         {
            double d = DistanceToRunway(vessel, runway);
            double slopeAltitude = runway.slopeTangens * d + runway.elevation;
            return slopeAltitude - vessel.altitude;
         }

         public static double HorizontalGlideSlopeDeviation(Vessel vessel, Runway runway)
         {
            if(vessel==null) return 0.0;

            double d = DistanceToRunway(vessel, runway);
            Coords onSlope = DestinationFromRunwayAtDistance(runway, d, vessel.mainBody.Radius);
            double deviation = DistanceFromVesselTo(vessel, onSlope);
            return deviation;
         }

         // deviation from heading from to heading to in degrees (-180..180)
         public static double HeadingDeviation(double from, double to)
         {
            // assume that from is less or equal to
            if(from>to)
            {
               // otherwise return the opposite
               return -HeadingDeviation(to, from);
            }
            // normalize from to 0 degree (North)
            to = to - from;
            // right
            if(to <= 180.0) return to;
            // left
            return -(360.0 - to);
         }

         public static double InverseHeading(double heading)
         {
            if (heading > 180.0) return heading - 180.0;
            if (heading == 0.0f) return 180.0;
            return 360.0 - heading;
         }

      }
   }

}
