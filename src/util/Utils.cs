using System;
using UnityEngine;
using System.Collections.Generic;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {
      public static class Utils
      {
         public static Texture2D GetTexture(String pathInGameData)
         {
            Log.Detail("get texture " + pathInGameData);
            Texture2D texture = GameDatabase.Instance.GetTexture(pathInGameData, false);
            if (texture != null)
            {
               return texture;
            }
            else
            {
               Log.Error("texture " + pathInGameData + " not found");
               return null;
            }
         }

         public static Texture2D CreateColorTexture(Color color)
         {
            Log.Detail("creating texture of color " + color);
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
         }

         public static String GetRootPath()
         {
            String path = KSPUtil.ApplicationRootPath;
            path = path.Replace("\\", "/");
            if (path.EndsWith("/")) path = path.Substring(0, path.Length - 1);
            //
            return path;
         }

         public static KeyCode[] GetPressedKeys()
         {
            List<KeyCode> pressed = new List<KeyCode>();
            KeyCode[] keycodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));
            foreach (KeyCode key in keycodes)
            {
               if (Input.GetKeyDown(key))
               {
                  pressed.Add(key);
               }
            }
            KeyCode[] result = new KeyCode[pressed.Count];
            pressed.CopyTo(result);
            return result;
         }

         public static String ToString(this Vector3d v, String format)
         {
            return "(" + v[0].ToString(format) + "," + v[1].ToString(format) + "," + v[2].ToString(format) + ")";
         }

         public static String ToString<T>(List<T> list)
         {
            String result = "";
            foreach (T x in list)
            {
               if (result.Length > 0) result = result + ",";
               result = result + x.ToString();
            }
            return result + " (" + list.Count + " entries)";
         }

         public static void SetTextColor(GUIStyle style, Color color)
         {
            style.normal.textColor = color;
            style.hover.textColor = color;
            style.focused.textColor = color;
            style.active.textColor = color;
         }

         public static double DegreeToRadians(double deg)
         {
            return deg * (Math.PI / 180.0);
         }

         public static double RadiansToDegree(double rad)
         {
            return rad * (180.0 / Math.PI);
         }

         public static double ToDegrees(int degrees, int minutes, int seconds)
         {
            return (double)degrees + ((double)minutes) / 60.0 + ((double)seconds) / 3600.0;
         }

         public static float InitialBearingFromTo(double longitudeFrom, double latitudeFrom, double longitudeTo, double latitudeTo)
         {
            double phi1 = Utils.DegreeToRadians(latitudeFrom);
            double lambda1 = Utils.DegreeToRadians(longitudeFrom);
            double phi2 = Utils.DegreeToRadians(latitudeTo);
            double lambda2 = Utils.DegreeToRadians(longitudeTo);


            double dy = Math.Sin(lambda2-lambda1) * Math.Cos(phi2);
            double dx = Math.Cos(phi1) * Math.Sin(phi2) - Math.Sin(phi1) * Math.Cos(phi2) * Math.Cos(lambda2 - lambda1);

            if(dx==0)
            {
               if (lambda2 - lambda1 > 0) return 0.0f;
               return 180.0f;
            }

            double theta = Math.Atan2(dy,dx);
            float bearing = (float)(RadiansToDegree(theta) + 360.0f) % 360.0f;
            return bearing;
         }

         public static float BearingFromTo(double longitudeFrom, double latitudeFrom, double longitudeTo, double latitudeTo)
         {
            double phi1 = Utils.DegreeToRadians(latitudeFrom);
            double lambda1 = Utils.DegreeToRadians(longitudeFrom);
            double phi2 = Utils.DegreeToRadians(latitudeTo);
            double lambda2 = Utils.DegreeToRadians(longitudeTo);

            double dphi = Math.Log( Math.Tan(Math.PI/4 +phi2/2) / Math.Tan(Math.PI/4 +phi1/2) );
            double dlambda = Math.Abs(lambda2 - lambda1);

            double theta = Math.Atan2(dlambda, dphi);
            float bearing = (float)(RadiansToDegree(theta) + 360.0f) % 360.0f;
            return bearing;
         }



      }
   }
}
