using System;
using UnityEngine;


namespace Nereid
{
   namespace NanoGauges
   {

      public class CameraGauge : VerticalGauge
      {
         private static readonly Texture2D SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/CAM-skin");
         private static readonly Texture2D SCALE = Utils.GetTexture("Nereid/NanoGauges/Resource/CAM-scale");


         public CameraGauge()
            : base(Constants.WINDOW_ID_GAUGE_CAM, SKIN, SCALE,true, 0.003f)
         {
         }

         public override void Reset()
         {
            
         }

         public override string GetName()
         {
            return "Camera";
         }

         public override string GetDescription()
         {
            return "Displays the selected camera mode.";
         }

         public override string ToString()
         {
            return "Gauge:CAM";
         }


         protected override float GetScaleOffset()
         {

            CameraManager.CameraMode mode = CameraManager.Instance.currentCameraMode;
            if(mode!=CameraManager.CameraMode.IVA)
            {
               FlightCamera camera = FlightCamera.fetch;
               if (camera == null) return 0.0f / 400.0f;
               switch(camera.mode)
               {
                  case FlightCamera.Modes.AUTO: return 200.0f / 400.0f;
                  case FlightCamera.Modes.FREE: return 150.0f / 400.0f;
                  case FlightCamera.Modes.ORBITAL: return 100.0f / 400.0f;
                  case FlightCamera.Modes.CHASE: return 50.0f / 400.0f;
                  default: return 0.0f / 400.0f;
               }
            }
            else
            {
               return 250.0f / 400.0f;
            }
         }
      }
   }
}
