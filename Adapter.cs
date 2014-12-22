using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

namespace Nereid
{
   namespace NanoGauges
   {
      public abstract class Adapter
      {
         private bool isInstalled  = false;

         public bool IsInstalled()
         {
            return this.isInstalled;
         }

         protected void SetInstalled(bool installed)
         {
            this.isInstalled = installed;
         }

         protected Type GetType(String name)
         {
            return AssemblyLoader.loadedAssemblies
               .SelectMany(x => x.assembly.GetExportedTypes())
               .SingleOrDefault(t => t.FullName == name);
         }

         protected bool IsTypeLoaded(String name)
         {
            return GetType(name) != null;
         }

         public abstract void Plugin();

         public virtual void Unplug()
         {
            SetInstalled(false);
         }
      }

      public class FARAdapter : Adapter
      {
         private MethodInfo methodGetMachNumber = null;
         private PropertyInfo instanceProp;
         private FieldInfo fieldQ;

         public override void Plugin()
         {
            Type typeAeoUtil = GetType("ferram4.FARAeroUtil");
            if (typeAeoUtil == null) return;
            Log.Detail("FARAdpater plugin of type ferram4.FARAeroUtil succesful");

            methodGetMachNumber = typeAeoUtil.GetMethod("GetMachNumber", BindingFlags.Public | BindingFlags.Static);
            if (methodGetMachNumber == null) return;
            Log.Detail("FARAdpater plugin of method GetMachNumber succesful");

            Type typeControlSys = GetType("ferram4.FARControlSys");
            if (typeControlSys == null) return;
            Log.Detail("FARAdpater plugin of type ferram4.FARControlSys succesful");

            instanceProp = typeControlSys.GetProperty("ActiveControlSys", BindingFlags.Static | BindingFlags.Public);
            if (instanceProp == null) return;
            Log.Detail("FARAdpater plugin of instance ActiveControlSys succesful");

            fieldQ = typeControlSys.GetField("q", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (fieldQ == null) return;
            Log.Detail("FARAdpater plugin of field ActiveControlSys.q succesful");

            SetInstalled(true);
         }


         public double GetMachNumber(CelestialBody body, double altitude, Vector3 velocity)
         {
            if(IsInstalled())
            {
               return (double)methodGetMachNumber.Invoke(null, new object[] { body, altitude, velocity });
            }
            return 0.0;
         }

         public double GetQ()
         {
            if (IsInstalled())
            {
               object farControlSys = instanceProp.GetValue(null, null);

               if (farControlSys != null)
                  return (double)fieldQ.GetValue(farControlSys);
            }

            return 0.0;
         }

         public double ApproximateMachNumber(CelestialBody body, double atmDensity, double altitude, Vector3 velocity)
         {
            if (altitude < 0) altitude = 0;
            // a technical constant for speed of sound appromixation 
            // experimental resolved; feel free to make better suggestions
            double c1 = (altitude / 16000);
            double c2 = (altitude / 39000);
            double c = 1.05 + (altitude / 15000) * (1 + altitude / 10000) + Math.Pow(c1, 4.15) + Math.Pow(c2, 5.58);

            return velocity.magnitude / (300 * Math.Sqrt(atmDensity)) / c;
         }

      }

   }
}
