using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nereid
{
   namespace NanoGauges
   {
      public abstract class ProfileBehaviour
      {
         private static readonly LinkedList<ProfileBehaviour> AllBehaviours = new LinkedList<ProfileBehaviour>();

         public static readonly ProfileBehaviour NOTHING = new Nothing();
         public static readonly ProfileBehaviour STANDARD = new ChangeGaugeSet(GaugeSet.ID.STANDARD);
         public static readonly ProfileBehaviour LAUNCH = new ChangeGaugeSet(GaugeSet.ID.LAUNCH);
         public static readonly ProfileBehaviour LAND = new ChangeGaugeSet(GaugeSet.ID.LAND);
         public static readonly ProfileBehaviour ORBIT = new ChangeGaugeSet(GaugeSet.ID.ORBIT);
         public static readonly ProfileBehaviour DOCK = new ChangeGaugeSet(GaugeSet.ID.DOCK);
         public static readonly ProfileBehaviour FLIGHT = new ChangeGaugeSet(GaugeSet.ID.FLIGHT);
         public static readonly ProfileBehaviour SET1 = new ChangeGaugeSet(GaugeSet.ID.SET1);
         public static readonly ProfileBehaviour SET2 = new ChangeGaugeSet(GaugeSet.ID.SET2);
         public static readonly ProfileBehaviour SET3 = new ChangeGaugeSet(GaugeSet.ID.SET3);
         public static readonly ProfileBehaviour HIDE = new Hide();

         public abstract void ActivateProfile();

         public ProfileBehaviour prev { get; protected set; }
         public ProfileBehaviour next { get; protected set; }

         private ProfileBehaviour()
         {
            if(AllBehaviours.Count>0)
            {
               // next
               AllBehaviours.Last.Value.next = this;
               next = AllBehaviours.First.Value;
               // prev
               AllBehaviours.First.Value.prev = this;
               prev = AllBehaviours.Last.Value;
            }
            else
            {
               next = this;
               prev = this;
            }
            AllBehaviours.AddLast(this);
         }

         public abstract String GetName();

         private class Hide : ProfileBehaviour
         {
            public override String GetName()
            {
               return "Hide";
            }

            public override void ActivateProfile()
            {
               NanoGauges.gauges.Hide();
            }
         }

         private class Nothing : ProfileBehaviour
         {
            public override String GetName()
            {
               return "Nothing";
            }

            public override void ActivateProfile()
            {

            }
         }

         private class ChangeGaugeSet : ProfileBehaviour
         {
            private readonly GaugeSet.ID id;

            public override String GetName()
            {
               return "Set "+id;
            }

            public ChangeGaugeSet(GaugeSet.ID id)
            {
               this.id = id;
            }

            public override void ActivateProfile()
            {
               NanoGauges.configuration.SetGaugeSet(id);
               NanoGauges.gauges.ReflectGaugeSetChange();
            }
         }

         public static ProfileBehaviour GetProfileBehaviourForName(String name)
         {
            foreach(ProfileBehaviour behaviour in AllBehaviours)
            {
               if (behaviour.GetName() == name) return behaviour;
            }
            throw new InvalidOperationException("no profile behaviour " + name + " found");
         }

         public static LinkedList<ProfileBehaviour> GetAllBehaviours()
         {
            return AllBehaviours;
         }
      }
   }

}
