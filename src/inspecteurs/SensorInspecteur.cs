using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Nereid
{
   namespace NanoGauges
   {
      public class SensorInspecteur : Inspecteur
      {
         private static readonly double MIN_INTERVAL = 0.2;

         private double temperature;
         private double gravity;
         private double pressure;
         private bool sensorTempEnabled = false;
         private bool sensorGravEnabled = false;
         private bool sensorPressureEnabled = false;
         private int sensorTempCount = 0;
         private int sensorGravCount = 0;
         private int sensorPressureCnt = 0;

         private readonly List<ModuleEnviroSensor> sensors = new List<ModuleEnviroSensor>();

         public SensorInspecteur()
            : base(5, MIN_INTERVAL)
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

         protected override void ScanVessel(Vessel vessel)
         {
            sensors.Clear();
            if (vessel == null || vessel.Parts==null) return;
            foreach (Part part in vessel.Parts)
            {
               if (part.packed)
               {
                  part.Unpack();
               }
               if (part.name != null)
               {
                  if (part.name.StartsWith("sensor") )
                  {
                     foreach (PartModule m in part.Modules)
                     {
                        if (m is ModuleEnviroSensor)
                        {
                           ModuleEnviroSensor sensor = m as ModuleEnviroSensor;
                           sensors.Add(sensor);
                        }
                     }
                  }
               }
            }
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

         public bool IsTempSensorEnabled()
         {
            return sensorTempEnabled;
         }

         public bool IsGravSensorEnabled()
         {
            return sensorGravEnabled;
         }

         public bool IsPressureSensorEnable()
         {
            return sensorPressureEnabled;
         }


         private void InspectSensor(ModuleEnviroSensor sensor)
         {
            if (sensor == null || sensor.sensorType == null) return;
            if (!sensor.isEnabled) return;
            if (sensor.sensorType == "TEMP")
            {
               String readout = sensor.readoutInfo;
               if (readout != null && readout != "Off")
               {
                  String temp = Regex.Replace(readout, "[^-.0-9]", "");
                  if(temp.Length>0)
                  {
                     try
                     {
                        temperature += double.Parse(temp);
                        sensorTempEnabled = true;
                        sensorTempCount++;
                     }
                     catch
                     {
                        if(Log.IsLogable(Log.LEVEL.DETAIL))
                        {
                           Log.Detail("invalid temp sensor value");
                        }
                     }
                  }
               }
            }
            else if (sensor.sensorType == "GRAV")
            {
               String readout = sensor.readoutInfo;
               if (readout != null && readout.Length > 5 && readout != "Off")
               {
                  String grav = readout.Substring(0, readout.Length - 5);
                  try
                  {
                     gravity += double.Parse(grav);
                     sensorGravEnabled = true;
                     sensorGravCount++;
                  }
                  catch
                  {
                     if (Log.IsLogable(Log.LEVEL.DETAIL))
                     {
                        Log.Detail("invalid temp sensor value");
                     }
                  }

               }
            }
            else if (sensor.sensorType == "PRES")
            {
               String readout = sensor.readoutInfo;
               if (readout != null && readout!="Off")
               {
                  readout = Regex.Replace(readout, "[^-.0-9]", "");
                  if (readout.Length == 0) readout = "0";
                  try
                  {
                     pressure += double.Parse(readout);
                     sensorPressureEnabled = true;
                     sensorPressureCnt++;
                  }
                  catch
                  {
                     if (Log.IsLogable(Log.LEVEL.DETAIL))
                     {
                        Log.Detail("invalid temp sensor value");
                     }
                  }

               }
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
            

         }
      }
   }
}
