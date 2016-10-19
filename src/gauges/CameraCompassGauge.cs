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
         }

         public override string GetName()
         {
            return "Camera";
         }

         public override string GetDescription()
         {
            return "Direction of camera view.";
         }

         protected override float GetDegrees()
         {
            FlightCamera camera = FlightCamera.fetch;

            if (camera == null || CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.Map)
            {
               Off();
               return 0.0f;
            }
            //
            On();

            float hdg = (180.0f*FlightCamera.CamHdg/(float)Math.PI) % 360;

            if(hdg<0)
            {
               hdg = 360 + hdg;
            }

            return hdg; 
         }

      }
   }
}
