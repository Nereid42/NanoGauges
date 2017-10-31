using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nereid
{
   namespace NanoGauges
   {
      public static class GameUtils
      {
         static public double TerminalVelocity(CelestialBody body, double m, double altitude, double cw)
         {
            if (FlightGlobals.ActiveVessel == null) return 0;
            // to avoid NREs while switching from FLIGHT to SPACECENTER
            if (HighLogic.LoadedScene != GameScenes.FLIGHT) return 0;

            double G = Constants.G;
            double M = body.Mass;
            // TODO: check if this call works like FlightGlobals.getAtmDensity(FlightGlobals.getStaticPressure(alt, body))
            double density = FlightGlobals.getAtmDensity(FlightGlobals.getStaticPressure(altitude, body), FlightGlobals.getExternalTemperature(), body);
            //double d0 = FlightGlobals.getAtmDensity(FlightGlobals.getStaticPressure(0, body), FlightGlobals.getExternalTemperature(), body);
            if (density > 0 && cw > 0)
            {
               double r = altitude + body.Radius;
               double vt =  Math.Sqrt((2 * m * G * M)) * Math.Sqrt( 1.0 / (r * r * cw * density));
               //Log.Test(body.name+" cw: " + cw + " alt "+altitude.ToString("0.0")+"  =>> vt = " + vt+"            // density="+density+"  // mass="+m);
               return vt;
            }
            //
            return double.NaN;
         }

      }
   }
}