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
         public static float InitialBearingFromTo(double longitudeFrom, double latitudeFrom, double longitudeTo, double latitudeTo)
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
            float bearing = (float)(Utils.RadiansToDegree(theta) + 360.0f) % 360.0f;
            return bearing;
         }

         public static float BearingFromTo(double longitudeFrom, double latitudeFrom, double longitudeTo, double latitudeTo)
         {
            double phi1 = Utils.DegreeToRadians(latitudeFrom);
            double lambda1 = Utils.DegreeToRadians(longitudeFrom);
            double phi2 = Utils.DegreeToRadians(latitudeTo);
            double lambda2 = Utils.DegreeToRadians(longitudeTo);

            double dphi = Math.Log(Math.Tan(Math.PI / 4 + phi2 / 2) / Math.Tan(Math.PI / 4 + phi1 / 2));
            double dlambda = Math.Abs(lambda2 - lambda1);

            double theta = Math.Atan2(dlambda, dphi);
            float bearing = (float)(Utils.RadiansToDegree(theta) + 360.0f) % 360.0f;
            return bearing;
         }

         public static float DistanceFromTo(double longitudeFrom, double latitudeFrom, double longitudeTo, double latitudeTo, double radius)
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
            double d = radius * c;
            */
            

            // less complex sperical law of Cosinus
            double d = Math.Acos ( Math.Sin(phi1)*Math.Sin(phi2) +Math.Cos(phi1)*Math.Cos(phi2)*Math.Cos(lambda2 - lambda1) ) * radius;

            return (float)d;
         }

         public static float DistanceToRunway(Vessel vessel, Runway runway)
         {
            if (vessel == null) return float.MaxValue;
            return DistanceFromTo(vessel.longitude, vessel.latitude, runway.coords.longitude, runway.coords.latitude, vessel.mainBody.Radius);
         }

      }
   }

}
