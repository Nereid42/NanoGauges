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
         private MethodInfo methodActiveVesselDynPres = null;

         public override void Plugin()
         {
            try
            {
                Type typeFarApi = GetType ("FerramAerospaceResearch.FARAPI") ;
                if (typeFarApi == null)
                    return ;
                Log.Detail ("FARAdapter plugin of type FerramAerospaceResearch.FARAPI successful.") ;

                methodActiveVesselDynPres = typeFarApi.GetMethod ("ActiveVesselDynPres",
                                                                  BindingFlags.Public | BindingFlags.Static,
                                                                  null,
                                                                  null,
                                                                  null) ;
                if (methodActiveVesselDynPres == null)
                    return ;
               Log.Detail("FARAdapter plugin of method ActiveVesselDynPres succesful");

               Type typeControlSys = GetType("ferram4.FARControlSys");
               if (typeControlSys == null) return;
               Log.Detail("FARAdpater plugin of type ferram4.FARControlSys succesful");

               SetInstalled(true);
            }
            catch(Exception e)
            {
               Log.Error("plugin of F.A.R failed; exception: " + e.GetType() + " - " + e.Message);
               SetInstalled(false);
            }
         }

         public double GetQ()
         {
            if (IsInstalled())
            {
               return (double) methodActiveVesselDynPres.Invoke (null, null) ;
            }

            return 0.0;
         }
      }

   }
}
