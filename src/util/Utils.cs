using System;
using UnityEngine;
using System.Collections.Generic;

namespace Nereid
{
   namespace NanoGauges
   {
      public static class Utils
      {
         public static Texture2D GetTexture(String pathInGameData)
         {
            Log.Detail("get texture " + pathInGameData + "(texture quality is " + GameSettings.TEXTURE_QUALITY + ")");
            // load texture directly if texture settings are to low
            if (GameSettings.TEXTURE_QUALITY != 0)
            {
               Log.Detail("loading texture directly from " + pathInGameData);
               return LoadImageFromFile(pathInGameData);
            }
            // texture is at fullres and we want to reuse the ingame textures (slightly lower quality)
            Texture2D texture = GameDatabase.Instance.GetTexture(pathInGameData, false);
            if (texture != null)
            {
               return texture;
            }
            else
            {
               Log.Error("texture '" + pathInGameData + "' not found");
               return null;
            }
         }


         public static Texture2D LoadImageFromFile(String pathInGameData)
         {
            String fullPath = Constants.GAMEDATA_PATH + System.IO.Path.DirectorySeparatorChar + pathInGameData + ".png";

            Log.Detail("loading texture from file " + fullPath);

            Texture2D tex = new Texture2D(1, 1);

            try
            {
               if (System.IO.File.Exists(fullPath))
               {
                  try
                  {
                     tex.LoadImage(System.IO.File.ReadAllBytes(fullPath));
                  }
                  catch (Exception ex)
                  {
                     Log.Error(ex.Message);
                  }
               }
               else
               {
                  Log.Error("cannot find image file '" + fullPath + "'");
               }
            }
            catch (Exception ex)
            {
               Log.Error(ex.Message);
            }
            Log.Trace("size of texture is " + tex.width + "x" + tex.height);
            return tex;
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

      }
   }
}
