using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {
      class CameraCompassGauge : AbstractCompassGauge        
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/CAMERA-skin");

         public CameraCompassGauge()
            : base(Constants.WINDOW_ID_GAUGE_CAMERA, SKIN)
         {
            Absolut();
            EnableBlueNeedle();
            SetBlueNeedleTo(8);
            EnableRedNeedle();
            SetRedNeedleTo(30);
            EnableYellowNeedle();
            SetYellowNeedleTo(45);
         }

         public override string GetName()
         {
            return "Camera";
         }

         public override string GetDescription()
         {
            return "Direction of camera view.";
         }

         private bool IsOrbitCamera(FlightCamera camera)
         {
            if (camera.mode == FlightCamera.Modes.ORBITAL) return true;
            if(camera.mode == FlightCamera.Modes.AUTO)
            {
               if (camera.autoMode == FlightCamera.Modes.ORBITAL) return true;
            }
            return false;
         }

         private bool IsRelativeCamera(FlightCamera camera)
         {
            if (camera.mode == FlightCamera.Modes.CHASE || camera.mode == FlightCamera.Modes.LOCKED) return true;
            if (camera.mode == FlightCamera.Modes.AUTO)
            {
               if (camera.autoMode == FlightCamera.Modes.CHASE || camera.autoMode == FlightCamera.Modes.LOCKED) return true;
            }
            return false;
         }

         protected override float GetDegrees()
         {
            FlightCamera camera = FlightCamera.fetch;
            Vessel vessel = FlightGlobals.ActiveVessel;

            if (camera == null
            || CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.Map
            || CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.IVA
            || IsOrbitCamera(camera))
            {
               Off();
               return 180.0f;
            }

            if(IsRelativeCamera(camera))
            {
               Relative();
            }
            else
            {
               Absolut();
            }

            if (vessel == null || vessel.mainBody == null || vessel.altitude > vessel.mainBody.Radius/2)
            {
               OutOfLimits();
            }
            else
            {
               InLimits();
            }

            //
            On();

            float hdg = (180.0f * FlightCamera.CamHdg / (float)Math.PI) % 360;

            if(hdg<0)
            {
               hdg = 360 + hdg;
            }

            return hdg; 
         }

      }
   }
}
