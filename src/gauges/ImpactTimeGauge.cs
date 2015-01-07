/************************************************************************************
 * 
 * Nanogauges - by Nereid
 * 
 * 
 * 
 * - this gauge is based on an idea by daid
 * - special thanks to cybutek who has granted the permission to use part of his
 *   code from the "Engineer Redux" plugin
 * 
 * 
 ************************************************************************************/

using System;
using UnityEngine;


namespace Nereid
{
    namespace NanoGauges
    {

        public class ImpactTimeGauge : VerticalGauge
        {
            private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/IMPACT-skin");
            private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/IMPACT-scale");

            private const float NO_IMPACT_TIME = -1;
            private const float MAX_IMPACT_TIME = 2 * Constants.SECONDS_PER_HOUR; //2 hours is the maximum shown on the IMPACT-scale

            public ImpactTimeGauge()
                : base(Constants.WINDOW_ID_GAUGE_IMPACT, SKIN, SCALE, false)
            {
            }

            protected override void AutomaticOnOff()
            {
               Vessel vessel = FlightGlobals.ActiveVessel;
               if (vessel != null && vessel.parts.Count > 0 && (vessel.situation == Vessel.Situations.FLYING || vessel.situation == Vessel.Situations.ORBITING || vessel.situation == Vessel.Situations.SUB_ORBITAL))
               {
                  On();
               }
               else
               {
                  Off();
               }
            }


            public override string GetName()
            {
                return "Impact time";
            }

            public override string GetDescription()
            {
                return "\n\nTime till impact.";
            }

            protected override float GetScaleOffset()
            {
                float lower = GetLowerOffset();
                float upper = GetUpperOffset();
                double time = GetTimeToImpact();
                if (time == NO_IMPACT_TIME || time >= MAX_IMPACT_TIME)
                {
                    //No Impact happening, or impact time beyond MAX_IMPACT_TIME.
                    OutOfLimits();
                    return upper;
                }
                InLimits();
                return lower + 75.0f * (float)Math.Log10(1.0 + time) / 400.0f;
            }

            public override string ToString()
            {
                return "Gauge:IMPACT";
            }

            private float GetTimeToImpact()
            {
                Vessel vessel = FlightGlobals.ActiveVessel;
                if (vessel == null)
                   return NO_IMPACT_TIME;
                if (FlightGlobals.ActiveVessel.mainBody.pqsController == null)
                   return NO_IMPACT_TIME;

                if (!IsOn()) return NO_IMPACT_TIME;

                double impactTime = 0;
                double impactAltitude = 0;
                double e = FlightGlobals.ActiveVessel.orbit.eccentricity;
                //get current position direction vector
                Vector3d currentpos = this.RadiusDirection(FlightGlobals.ActiveVessel.orbit.trueAnomaly);
                //calculate longitude in inertial reference frame from that
                double currentirflong = 180 * Math.Atan2(currentpos.x, currentpos.y) / Math.PI;

                //experimentally determined; even for very flat trajectories, the errors go into the sub-millimeter area after 5 iterations or so
                const int impactiterations = 6;

                //do a few iterations of impact site calculations
                for (var i = 0; i < impactiterations; i++)
                {
                    if (FlightGlobals.ActiveVessel.orbit.PeA >= impactAltitude)
                    {
                        //periapsis must be lower than impact alt
                       return NO_IMPACT_TIME;
                    }
                    if ((FlightGlobals.ActiveVessel.orbit.eccentricity < 1) && (FlightGlobals.ActiveVessel.orbit.ApA <= impactAltitude))
                    {
                        //apoapsis must be higher than impact alt
                       return NO_IMPACT_TIME;
                    }
                    if ((FlightGlobals.ActiveVessel.orbit.eccentricity >= 1) && (FlightGlobals.ActiveVessel.orbit.timeToPe <= 0))
                    {
                        //if currently escaping, we still need to be before periapsis
                       return NO_IMPACT_TIME;
                    }

                    double impacttheta = 0;
                    if (e > 0)
                    {
                        //in this step, we are using the calculated impact altitude of the last step, to refine the impact site position
                        impacttheta = -180 * Math.Acos((FlightGlobals.ActiveVessel.orbit.PeR * (1 + e) / (FlightGlobals.ActiveVessel.mainBody.Radius + impactAltitude) - 1) / e) / Math.PI;
                    }

                    //calculate time to impact
                    impactTime = FlightGlobals.ActiveVessel.orbit.timeToPe - this.TimeToPeriapsis(impacttheta);
                    //calculate position vector of impact site
                    Vector3d impactpos = this.RadiusDirection(impacttheta);
                    //calculate longitude of impact site in inertial reference frame
                    double impactirflong = 180 * Math.Atan2(impactpos.x, impactpos.y) / Math.PI;
                    double deltairflong = impactirflong - currentirflong;
                    //get body rotation until impact
                    double bodyrot = 360 * impactTime / FlightGlobals.ActiveVessel.mainBody.rotationPeriod;
                    //get current longitude in body coordinates
                    double currentlong = FlightGlobals.ActiveVessel.longitude;
                    //finally, calculate the impact longitude in body coordinates
                    double impactLongitude = this.NormAngle(currentlong - deltairflong - bodyrot);
                    //calculate impact latitude from impact position
                    double impactLatitude = 180 * Math.Asin(impactpos.z / impactpos.magnitude) / Math.PI;
                    //calculate the actual altitude of the impact site
                    //altitude for long/lat code stolen from some ISA MapSat forum post; who knows why this works, but it seems to.
                    Vector3d rad = QuaternionD.AngleAxis(impactLongitude, Vector3d.down) * QuaternionD.AngleAxis(impactLatitude, Vector3d.forward) * Vector3d.right;
                    impactAltitude = FlightGlobals.ActiveVessel.mainBody.pqsController.GetSurfaceHeight(rad) - FlightGlobals.ActiveVessel.mainBody.pqsController.radius;
                    
                    /*
                    if ((impactAltitude < 0) && FlightGlobals.ActiveVessel.mainBody.ocean)
                    {
                        //When hitting sea level. Disabled to match the behaviour of the radar altitude gauge.
                        impactAltitude = 0;
                    }
                    */
                }
                return (float)impactTime;
            }

            private Vector3d RadiusDirection(double theta)
            {
                theta = Math.PI * theta / 180;
                double omega = Math.PI * FlightGlobals.ActiveVessel.orbit.argumentOfPeriapsis / 180;
                double incl = Math.PI * FlightGlobals.ActiveVessel.orbit.inclination / 180;

                double costheta = Math.Cos(theta);
                double sintheta = Math.Sin(theta);
                double cosomega = Math.Cos(omega);
                double sinomega = Math.Sin(omega);
                double cosincl = Math.Cos(incl);
                double sinincl = Math.Sin(incl);

                Vector3d result;

                result.x = cosomega * costheta - sinomega * sintheta;
                result.y = cosincl * (sinomega * costheta + cosomega * sintheta);
                result.z = sinincl * (sinomega * costheta + cosomega * sintheta);

                return result;
            }

            private double TimeToPeriapsis(double theta)
            {
                double e = FlightGlobals.ActiveVessel.orbit.eccentricity;
                double a = FlightGlobals.ActiveVessel.orbit.semiMajorAxis;
                double rp = FlightGlobals.ActiveVessel.orbit.PeR;
                double mu = FlightGlobals.ActiveVessel.mainBody.gravParameter;

                if (e == 1.0)
                {
                    var D = Math.Tan(Math.PI * theta / 360.0);
                    var M = D + D * D * D / 3.0;
                    return (Math.Sqrt(2.0 * rp * rp * rp / mu) * M);
                }
                if (a > 0)
                {
                    var cosTheta = Math.Cos(Math.PI * theta / 180.0);
                    var cosE = (e + cosTheta) / (1.0 + e * cosTheta);
                    var radE = Math.Acos(cosE);
                    var M = radE - e * Math.Sin(radE);
                    return (Math.Sqrt(a * a * a / mu) * M);
                }
                if (a < 0)
                {
                    var cosTheta = Math.Cos(Math.PI * theta / 180.0);
                    var coshF = (e + cosTheta) / (1.0 + e * cosTheta);
                    var radF = ACosh(coshF);
                    var M = e * Math.Sinh(radF) - radF;
                    return (Math.Sqrt(-a * a * a / mu) * M);
                }

                return 0;
            }

            private double NormAngle(double ang)
            {
                if (ang > 180)
                {
                    ang -= 360 * Math.Ceiling((ang - 180) / 360);
                }
                if (ang <= -180)
                {
                    ang -= 360 * Math.Floor((ang + 180) / 360);
                }

                return ang;
            }

            private static double ACosh(double x)
            {
                return (Math.Log(x + Math.Sqrt((x * x) - 1.0)));
            }
        }
    }
}
