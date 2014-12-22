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

         public static String GetRootPath()
         {
            String path = KSPUtil.ApplicationRootPath;
            path = path.Replace("\\", "/");
            if (path.EndsWith("/")) path = path.Substring(0, path.Length - 1);
            //
            return path;
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
      }
   }
}
