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
            Type type = null;
            AssemblyLoader.loadedAssemblies.TypeOperation(t =>
            {
               if (t.FullName == name)
               {
                  type = t;
               }
            });
            return type;
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
   }
}
