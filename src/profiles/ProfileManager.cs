using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace Nereid
{
   namespace NanoGauges
   {
      public class ProfileManager
      {
         public readonly Profile LAUNCH = new Profile("Launch");
         public readonly Profile FLIGHT = new Profile("Flight");
         public readonly Profile ORBIT_TO_SUBORBITAL = new Profile("Orbit to Suborbital");
         public readonly Profile FLIGHT_TO_SUBORBITAL = new Profile("Flight to Suborbital");
         public readonly Profile OTHER_TO_SUBORBITAL = new Profile("Other to Suborbital");
         public readonly Profile LANDED = new Profile("Landed");
         public readonly Profile ORBIT = new Profile("Orbit");
         public readonly Profile DOCKED = new Profile("Docked");
         public readonly Profile DOCKING = new Profile("Docking (UI)");
         public readonly Profile ESCAPE = new Profile("Escape");
         public readonly Profile EVA = new Profile("EVA");
         public readonly Profile HOTKEY1 = new Profile("Hotkey 1");
         public readonly Profile HOTKEY2 = new Profile("Hotkey 2");
         public readonly Profile HOTKEY3 = new Profile("Hotkey 3");

         public bool enabled = false;
         // ignore hotkeys in the current UI frame
         private bool ignoreHotkeysInFrame = false;

         public KeyCode Hotkey1 = KeyCode.P;
         public KeyCode Hotkey2 = KeyCode.None;
         public KeyCode Hotkey3 = KeyCode.None;

         // kerbin time until profile changes are locked (0=off)
         private double profileChangeIsLockedUntil = 0;
         // temporay lock
         private bool locked = false;

         public ProfileManager()
         {
            SetDefaults();

            GameEvents.onVesselSituationChange.Add(this.OnVesselSituationChange);
            GameEvents.onVesselChange.Add(this.OnVesselChange);
            GameEvents.onCrewOnEva.Add(this.OnCrewOnEva);
            GameEvents.OnFlightUIModeChanged.Add(this.OnFlightUIModeChanged);
         }

         private void SetDefaults()
         {
            LAUNCH.SetBehaviour(ProfileBehaviour.LAUNCH);
            FLIGHT.SetBehaviour(ProfileBehaviour.NOTHING);
            ORBIT_TO_SUBORBITAL.SetBehaviour(ProfileBehaviour.LAND);
            FLIGHT_TO_SUBORBITAL.SetBehaviour(ProfileBehaviour.ORBIT);
            OTHER_TO_SUBORBITAL.SetBehaviour(ProfileBehaviour.LAND);
            LANDED.SetBehaviour(ProfileBehaviour.LAUNCH);
            ORBIT.SetBehaviour(ProfileBehaviour.ORBIT);
            ESCAPE.SetBehaviour(ProfileBehaviour.FLIGHT);
            DOCKED.SetBehaviour(ProfileBehaviour.ORBIT);
            DOCKING.SetBehaviour(ProfileBehaviour.DOCK);
            EVA.SetBehaviour(ProfileBehaviour.STANDARD);
            HOTKEY1.SetBehaviour(ProfileBehaviour.FLIGHT);
            HOTKEY2.SetBehaviour(ProfileBehaviour.NOTHING);
            HOTKEY3.SetBehaviour(ProfileBehaviour.NOTHING);
            Hotkey1 = KeyCode.None;
            Hotkey2 = KeyCode.None;
            Hotkey3 = KeyCode.None;
         }

         public void ResetDefaults()
         {
            Log.Info("resetting profiles to defaults");
            SetDefaults();
         }

         private void UnlockProfileChange()
         {
            profileChangeIsLockedUntil = 0;
         }

         private void LockProfileChange()
         {
            profileChangeIsLockedUntil = Planetarium.GetUniversalTime()+NanoGauges.configuration.minProfileInterval;
            Log.Info("locking profile switch until " + profileChangeIsLockedUntil + ", now=" + Planetarium.GetUniversalTime());
         }

         public bool IsProfileChangeLocked()
         {
            if (locked) return true;
            return (profileChangeIsLockedUntil > 0) && (profileChangeIsLockedUntil > Planetarium.GetUniversalTime());
         }

         public void Lock()
         {
            locked = true;
         }

         public void Unlock()
         {
            profileChangeIsLockedUntil = 0;
            locked = false;
         }

         public void ToggleLock()
         {
            if (!locked && !IsProfileChangeLocked())
            {
               locked = true;
            }
            else
            {
               locked = false;
            }
            profileChangeIsLockedUntil = 0;
         }

         public void IgnoreHotkeyInFrame()
         {
            ignoreHotkeysInFrame = true;
         }

         private void SwitchProfile(Profile profile)
         {
            // no gauges, no profile...
            if (NanoGauges.gauges == null) return;
            //
            Log.Info("switching to profile "+profile.name);
            if (enabled && profile.enabled)
            {
               ProfileBehaviour behaviour = profile.GetBehaviour();
               behaviour.ActivateProfile();
            }
         }

         private void SwitchToSituation(Vessel.Situations situation)
         {
            SwitchSituation(situation, situation);
         }

         private void SwitchSituation(Vessel.Situations from, Vessel.Situations to)
         {
            Log.Info("switching profile from "+from+" to "+to);
            switch (to)
            {
               case Vessel.Situations.DOCKED:
                  SwitchProfile(DOCKED);
                  break;
               case Vessel.Situations.ESCAPING:
                  SwitchProfile(ESCAPE);
                  break;
               case Vessel.Situations.PRELAUNCH:
                  SwitchProfile(LAUNCH);
                  break;
               case Vessel.Situations.SPLASHED:
               case Vessel.Situations.LANDED:
                  SwitchProfile(LANDED);
                  break;
               case Vessel.Situations.ORBITING:
                  SwitchProfile(ORBIT);
                  break;
               case Vessel.Situations.SUB_ORBITAL:
                  if(from==Vessel.Situations.ORBITING)
                  {
                     SwitchProfile(ORBIT_TO_SUBORBITAL);
                  }
                  else if (from == Vessel.Situations.FLYING)
                  {
                     SwitchProfile(FLIGHT_TO_SUBORBITAL);
                  }
                  else 
                  {
                     SwitchProfile(OTHER_TO_SUBORBITAL);
                  }
                  break;
               case Vessel.Situations.FLYING:
                  SwitchProfile(FLIGHT);
                  break;
            }
         }


         private void OnFlightUIModeChanged(FlightUIMode mode)
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel == null) return;
            if (DOCKING.enabled && vessel.situation == Vessel.Situations.ORBITING)
            {
               if (mode == FlightUIMode.DOCKING)
               {
                  SwitchProfile(DOCKING);
               }
               else
               {
                  if (mode == FlightUIMode.ORBITAL)
                  {
                     SwitchProfile(ORBIT);
                  }
                  else
                  {
                     SwitchToSituation(vessel.situation);
                  }
               }
            }
            UnlockProfileChange();
         }

         private void OnCrewOnEva(GameEvents.FromToAction<Part, Part> action)
         {
            SwitchProfile(EVA);
            UnlockProfileChange();
         }

         private void OnGameStateCreated(Game game)
         {
            Unlock();
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel == null) return;

            if(vessel.situation==Vessel.Situations.PRELAUNCH)
            {
               SwitchProfile(LAUNCH);
            }
            UnlockProfileChange();
         }

         private void OnVesselSituationChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> action)
         {
            Vessel vessel = action.host;
            if (vessel == null || vessel != FlightGlobals.ActiveVessel) return;
            if (Log.IsLogable(Log.LEVEL.DETAIL))  Log.Detail("situation changed for vessel " + vessel.name);

            // is the automatic profile change locked?
            if (IsProfileChangeLocked())
            {
               if (Log.IsLogable(Log.LEVEL.DETAIL)) Log.Detail("profile change locked");
               // extend lock
               LockProfileChange();
               return;
            }
            if (Log.IsLogable(Log.LEVEL.DETAIL)) Log.Detail("profile change not locked");

            Vessel.Situations from = action.from;
            Vessel.Situations to = action.to;

            // no change, no profile
            if (to == from) return;

            if (!vessel.isEVA)
            {
               SwitchSituation(from,to);
            }

            // decide about locking
            if(!vessel.isEVA && to!=Vessel.Situations.PRELAUNCH && vessel.missionTime>1)
            {
               // lock profile change caused by vessel situations for a few seconds
               LockProfileChange();
            }
            else
            {
               // EVA or PRELAUNCH and any change in the first 1 second unlocks profile change
               UnlockProfileChange();
            }
         }


         private void OnVesselChange(Vessel vessel)
         {
            if (vessel == null || vessel != FlightGlobals.ActiveVessel) return;

            Unlock();

            if(vessel.isEVA)
            {
               SwitchProfile(EVA);
            }
            else
            {
               SwitchToSituation(vessel.situation);
            }
            UnlockProfileChange();
         }

         public void Update()
         {
            if(!ignoreHotkeysInFrame)
            {
               if (Hotkey1 != KeyCode.None && Input.GetKeyDown(Hotkey1))
               {
                  SwitchProfile(HOTKEY1);
               }
               else if (Hotkey2 != KeyCode.None && Input.GetKeyDown(Hotkey2))
               {
                  SwitchProfile(HOTKEY2);
               }
               else if (Hotkey3 != KeyCode.None && Input.GetKeyDown(Hotkey3))
               {
                  SwitchProfile(HOTKEY3);
               }
            }
            ignoreHotkeysInFrame = false;
         }

         public void Read(BinaryReader reader)
         {
            Log.Info("reading profiles");
            //
            try
            {
               enabled = reader.ReadBoolean();
               //
               Hotkey1 = (KeyCode)reader.ReadInt16();
               Hotkey2 = (KeyCode)reader.ReadInt16();
               Hotkey3 = (KeyCode)reader.ReadInt16();
               // reserved
               reader.ReadInt16();
               reader.ReadInt16();
               // profiles
               LAUNCH.Read(reader);
               FLIGHT.Read(reader);
               ORBIT_TO_SUBORBITAL.Read(reader);
               FLIGHT_TO_SUBORBITAL.Read(reader);
               OTHER_TO_SUBORBITAL.Read(reader);
               LANDED.Read(reader);
               ORBIT.Read(reader);
               DOCKED.Read(reader);
               DOCKING.Read(reader);
               ESCAPE.Read(reader);
               EVA.Read(reader);
               HOTKEY1.Read(reader);
               HOTKEY2.Read(reader);
               HOTKEY3.Read(reader);
            }
            catch(IOException e)
            {
               Log.Warning("error reading profiles from config");
               SetDefaults();
               throw e;
            }
         }

         public void Write(BinaryWriter writer)
         {
            Log.Info("writing profiles");
            //
            writer.Write(enabled);
            //
            writer.Write((Int16)Hotkey1);
            writer.Write((Int16)Hotkey2);
            writer.Write((Int16)Hotkey3);
            writer.Write((Int16)0); // Reserved
            writer.Write((Int16)0); // Reserved
            // profiles
            LAUNCH.Write(writer);
            FLIGHT.Write(writer);
            ORBIT_TO_SUBORBITAL.Write(writer);
            FLIGHT_TO_SUBORBITAL.Write(writer);
            OTHER_TO_SUBORBITAL.Write(writer);
            LANDED.Write(writer);
            ORBIT.Write(writer);
            DOCKED.Write(writer);
            DOCKING.Write(writer);
            ESCAPE.Write(writer);
            EVA.Write(writer);
            HOTKEY1.Write(writer);
            HOTKEY2.Write(writer);
            HOTKEY3.Write(writer);
         }


      }
   }
}
