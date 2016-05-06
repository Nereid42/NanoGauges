using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Nereid
{
   namespace NanoGauges
   {
      public class SensorInspecteur : Inspecteur
      {
         private double temperature;
         private double gravity;
         private double pressure;
         private double seismic;
         private bool sensorTempEnabled = false;
         private bool sensorGravEnabled = false;
         private bool sensorPressureEnabled = false;
         private bool sensorSeismicEnabled = false;
         private int sensorTempCount = 0;
         private int sensorGravCount = 0;
         private int sensorPressureCnt = 0;
         private int sensorSeismicCnt = 0;

         private readonly List<ModuleEnviroSensor> sensors = new List<ModuleEnviroSensor>();

         public SensorInspecteur()
            : base(3)
         {
            Reset();
         }


         public override void Reset()
         {
            this.temperature = 0.0;
            this.gravity = 0.0;
            this.pressure = 0.0;
            this.sensorTempEnabled = false;
            this.sensorTempCount = 0;
            this.sensorGravEnabled = false;
            this.sensorGravCount = 0;
            this.sensorPressureCnt = 0;
         }

         private void ScanPart(Part part)
         {
            if (part.name != null)
            {
               //if (part.name.StartsWith("sensor") )
               {
                  foreach (PartModule m in part.Modules)
                  {
                     if (m is ModuleEnviroSensor)
                     {
                        ModuleEnviroSensor sensor = m as ModuleEnviroSensor;
                        if(!sensors.Contains(sensor))
                        {
                           sensors.Add(sensor);
                           if (Log.IsLogable(Log.LEVEL.TRACE)) Log.Trace("Added sensor module of type " + sensor.sensorType);
                        }
                     }
                  }
               }
            }
         }

         protected override void PartUnpacked(Part part)
         {
            ScanPart(part);
         }

         protected override void ScanVessel(Vessel vessel)
         {
            sensors.Clear();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            if (Log.IsLogable(Log.LEVEL.DETAIL)) Log.Detail("scanning vessel "+vessel.name);
            if (vessel == null || vessel.Parts==null) return;
            foreach (Part part in vessel.Parts)
            {
               if (!part.packed) ScanPart(part);
            }
            sw.Stop();
            if (Log.IsLogable(Log.LEVEL.DETAIL)) Log.Detail("vessel scanned in" + sw.ElapsedMilliseconds+"ms ("+sw.ElapsedTicks+" ticks)");
         }

         public double GetTemperature()
         {
            return temperature;
         }

         public double GetGravity()
         {
            return gravity;
         }

         public double GetPressure()
         {
            return pressure;
         }

         public double GetSeismic()
         {
            return seismic;
         }

         public bool IsTempSensorEnabled()
         {
            return sensorTempEnabled;
         }

         public bool IsGravSensorEnabled()
         {
            return sensorGravEnabled;
         }

         public bool IsPressureSensorEnabled()
         {
            return sensorPressureEnabled;
         }

         public bool IsSeismicSensorEnabled()
         {
            return sensorSeismicEnabled;
         }

         private double ParseReadout(String readout)
         {
            try
            {
               for (int i = 0; i < readout.Length; i++)
               {
                  if(!char.IsDigit(readout[i]) && readout[i]!='.')
                  {
                     return double.Parse(readout.Substring(0,i));
                  }
               }
               return double.Parse(readout);
            }
            catch
            {
               if (Log.IsLogable(Log.LEVEL.DETAIL))
               {
                  Log.Detail("invalid readout '"+readout+"'");
               }
               return 0.0;
            }
         }

         private bool IsOff(ModuleEnviroSensor sensor)
         {
            return sensor.readoutInfo == Constants.STRING_OFF;
         }

         private void InspectSensor(ModuleEnviroSensor sensor)
         {
            if (sensor == null || sensor.sensorType == null) return;
            if (IsOff(sensor)) return;
            if (!sensor.isEnabled) return;
            String readout = sensor.readoutInfo;
            if ( readout == null || readout == "" ) return;

            if (sensor.sensorType == Constants.SENSOR_TEMPERATURE)
            {
               temperature += ParseReadout(readout);
               sensorTempEnabled = true;
               sensorTempCount++;
            }
            else if (sensor.sensorType == Constants.SENSOR_GRAVIMETRIC)
            {
               gravity += ParseReadout(readout);
               sensorGravEnabled = true;
               sensorGravCount++;
            }
            else if (sensor.sensorType == Constants.SENSOR_BAROMETRIC)
            {
               pressure += ParseReadout(readout);
               sensorPressureEnabled = true;
               sensorPressureCnt++;
            }
            else if (sensor.sensorType == Constants.SENSOR_SEISMIC)
            {
               seismic += ParseReadout(readout);
               sensorSeismicEnabled = true;
               sensorSeismicCnt++;
            }
         }

         protected override void Inspect(Vessel vessel)
         {
            temperature = 0.0;
            sensorTempEnabled = false;
            sensorTempCount = 0;
            gravity = 0.0;
            sensorGravEnabled = false;
            sensorGravCount = 0;
            pressure = 0.0;
            sensorPressureCnt = 0;
            sensorPressureEnabled = false;
            seismic = 0.0;
            sensorSeismicCnt = 0;
            sensorSeismicEnabled = false;

            foreach (ModuleEnviroSensor sensor in this.sensors)
            {
               InspectSensor(sensor);
            }

            if(sensorTempCount>0)
            {
               temperature = temperature / sensorTempCount;
            }
            if (sensorGravCount > 0)
            {
               gravity = gravity / sensorGravCount;
            }
            if (sensorPressureCnt > 0)
            {
               pressure = pressure / sensorPressureCnt;
            }
            if (sensorSeismicCnt > 0)
            {
               seismic = seismic / sensorSeismicCnt;
            }
         }
      }
   }
}
