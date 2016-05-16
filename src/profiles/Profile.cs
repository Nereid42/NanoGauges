using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Nereid
{
   namespace NanoGauges
   {
      public class Profile
      {
         public readonly String name;
         public bool enabled { get; set; }

         private ProfileBehaviour behaviour = ProfileBehaviour.NOTHING;

         public Profile(String name)
         {
            this.name = name;
            this.enabled = true;
         }

         public void SetBehaviour(ProfileBehaviour behaviour)
         {
            this.behaviour = behaviour;
         }

         public ProfileBehaviour GetBehaviour()
         {
            return behaviour;
         }


         public void Read(BinaryReader reader)
         {
            Log.Detail("reading profile " + name);
            String nameOfBehavior = reader.ReadString();
            Log.Trace("profile behavior name read: " + nameOfBehavior);
            behaviour = ProfileBehaviour.GetProfileBehaviourForName(nameOfBehavior);
            if(behaviour==null)
            {
               Log.Warning("behavior '"+nameOfBehavior+"' not found");
               behaviour = ProfileBehaviour.NOTHING;
            }
            enabled = reader.ReadBoolean();
         }

         public void Write(BinaryWriter writer)
         {
            Log.Detail("writing profile " + name);
            writer.Write(behaviour.GetName());
            writer.Write(enabled);
         }
      }
   }
}
