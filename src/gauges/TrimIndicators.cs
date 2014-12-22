/*
 * Trim Indicators based on an idea by dazoe 
 * 
 * */

using System;
using UnityEngine;

namespace Nereid
{
   namespace NanoGauges
   {
      public class TrimIndicators
      {
         // the scaled size of the trim indicators
         private const float SIZE = 0.75f;
         // String constants
         private const String STAGING_QUADRANT_NAME = "staging Quadrant";
         private const String COMPONENT_ROLL_NAME = "roll";
         private const String COMPONENT_PITCH_NAME = "pitch";
         private const String COMPONENT_YAW_NAME = "yaw";

         // constants for gauge limit scaling (by dazoe)
         private static readonly Vector3 LIMIT_SCALE_ROLL = new Vector3(1f, 1f - 0.22f, 1f);
         private static readonly Vector3 LIMIT_SCALE_PITCH = new Vector3(1f - 0.11f, 1f, 1f);
         private static readonly Vector3 LIMIT_SCALE_YAW = new Vector3(1f, 1f - 0.50f, 1f);

         // the new trim gauges
         private LinearGauge trimRollGauge;
         private LinearGauge trimPitchGauge;
         private LinearGauge trimYawGauge;

         // the trim gauges get globally disabled if any exception is thrown
         private static bool disabled = false;

         private GameObject CreateIndicator(String name, GameObject component)
         {
            GameObject indicator = (GameObject)MonoBehaviour.Instantiate(component, Vector3.zero, Quaternion.identity);
            indicator.name = name;
            // set parent
            indicator.transform.SetParent(component.transform.parent);
            // set rotations
            indicator.transform.rotation = component.transform.rotation;
            // set size of the indicator by scaling them down and reverting the direction
            indicator.transform.localScale = Vector3.Scale(component.transform.localScale, new Vector3(-SIZE, SIZE, SIZE));
            return indicator;
         }

         private void AdjustGaugeLimits(LinearGauge gauge, Vector3 scale)
         {
            gauge.minPos = Vector3.Scale(gauge.minPos, scale);
            gauge.maxPos = Vector3.Scale(gauge.maxPos, scale);
         }

         private LinearGauge CreateGauge(String name, GameObject component, Vector3 scale)
         {
            // create indicator
            GameObject indicator = CreateIndicator(name, component);
            // get the LinearGauge component
            LinearGauge gauge = indicator.GetComponent<LinearGauge>();
            // update gauges limits 
            AdjustGaugeLimits(gauge, scale);
            // set gauge pointer.
            gauge.pointer = indicator.transform;
            // return the gauge
            return gauge;
         }

         public void Init()
         {
            // try to catch any exception if compatibility issues with KSP occur
            try
            {
               // get the original control input gauges
               GameObject stagingQuadrant = GameObject.Find(STAGING_QUADRANT_NAME);
               GameObject roll = stagingQuadrant.transform.FindChild(COMPONENT_ROLL_NAME).gameObject;
               GameObject pitch = stagingQuadrant.transform.FindChild(COMPONENT_PITCH_NAME).gameObject;
               GameObject yaw = stagingQuadrant.transform.FindChild(COMPONENT_YAW_NAME).gameObject;

               // create gauges
               this.trimRollGauge = CreateGauge("Roll Trim", roll, LIMIT_SCALE_ROLL);
               this.trimPitchGauge = CreateGauge("Pitch Trim", pitch, LIMIT_SCALE_PITCH);
               this.trimYawGauge = CreateGauge("Yaw Trim", yaw, LIMIT_SCALE_YAW);
            }
            catch
            {
               Log.Warning("exception caught in trim indicator init");
               disabled = true;
               NanoGauges.configuration.trimIndicatorsEnabled = false;
            }
         }

         public void Update()
         {
            if (disabled) return;
            // try to catch any exception if compatibility issues with KSP occur
            try
            {
               // update the trim gauges
               FlightCtrlState ctrlState = FlightGlobals.ActiveVessel.ctrlState;
               trimRollGauge.setValue(ctrlState.rollTrim);
               trimPitchGauge.setValue(ctrlState.pitchTrim);
               trimYawGauge.setValue(ctrlState.yawTrim);
            } 
            catch
            {
               Log.Warning("exception caught in trim indicator update");
               disabled = true;
               NanoGauges.configuration.trimIndicatorsEnabled = false;
            }
         }
      }
   }
}
