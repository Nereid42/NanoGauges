using System;
using System.Collections.Generic;
using UnityEngine;
using KSP.UI.Screens;
using NanoGaugesAdapter;
using System.Diagnostics;

namespace Nereid
{
   namespace NanoGauges
   {
      [KSPAddon(KSPAddon.Startup.Flight, false)]
      public class NanoGauges : MonoBehaviour
      {
         public static readonly String RESOURCE_PATH = "Nereid/NanoGauges/Resource/";

         public static readonly Gauges gauges;
         public static readonly Configuration configuration = new Configuration();

         public static readonly SnapinManager snapinManager;

         // Profiles
         public static readonly ProfileManager profileManager;
         // HotKeys
         public static readonly HotkeyManager hotkeyManager;

         private IButton toolbarButton;
         private ApplicationLauncherButton stockToolbarButton = null;

         private ConfigWindow configWindow;

         private volatile bool destroyed = false;

         // for timed logging
         private readonly Stopwatch timer = new Stopwatch();
         private long lastPerformanceLog = 0;

         // debug
         private readonly TimedStatistics.Timer updateTimer = TimedStatistics.instance.GetTimer("Update");
         private readonly TimedStatistics.Timer gaugeUpdateTimer = TimedStatistics.instance.GetTimer("Gauge updates");
         private readonly TimedStatistics.Timer navUpdateTimer = TimedStatistics.instance.GetTimer("Navigation updates");


         static NanoGauges()
         {
            Log.SetLevel(Log.LEVEL.INFO);
            //
            Log.Info("static init of NanoGauges");
            gauges = new Gauges();
            configuration.ResetAllWindowPositions(gauges);
            configuration.SetGaugeSet(GaugeSet.ID.STANDARD);
            //
            profileManager = new ProfileManager();
            //
            hotkeyManager = new HotkeyManager();
            //
            configuration.Load();
            Log.Info("log level is " + Log.GetLogLevel());
            //
            gauges.ReflectGaugeSetChange();
            gauges.UpdateGaugeScaling();
            //
            snapinManager = new SnapinManager(gauges);

         }

         public NanoGauges()
         {
            Log.Info("new instance of NanoGauges");
            // 
            timer.Start();
         }

         public void Awake()
         {
            Log.Info("awaking NanoGauges");
            gauges.ShowGauges();
            // use of stock toolbar
            GameEvents.onGUIApplicationLauncherReady.Add(OnGUIAppLauncherReady);
            Log.Info("NanoGauges is awake");
         }

         private void OnGUI()
         {
            gauges.DrawGauges();
            WindowManager.instance.OnGUI();
        }

         public void Start()
         {
            Log.Info("starting NanoGauges");

            Log.Info("Texture Quality: " + GameSettings.TEXTURE_QUALITY);

            // show dialog if texture settings are to low
            // not longer necessary
            /*if (GameSettings.TEXTURE_QUALITY!=0)
            {
               PopupDialog.SpawnPopupDialog(new Vector2(0, 0), new Vector2(0, 0), "TEXTURE", "NanoGauges Warning!", "The used texture quality is not supported by Nanogauges. Please change the texture quality to fullres in the graphic settings.", "OK", true, null);
            }*/

            if (!configuration.useStockToolbar)
            {
               Log.Info("stock toolbar button disabled");
               AddToolbarButtons();
            }
            else
            {
               Log.Info("stock toolbar button enabled");
               CreateStockToolbarButton();
            }
            
            // scheduling navigation Updates
            InvokeRepeating("UpdateNavGlobals", 0.0f, 0.1f);
            
            Log.Info("NanoGauges started");
         }

         private void CreateStockToolbarButton()
         {
            if (ApplicationLauncher.Instance != null && ApplicationLauncher.Ready)
            {
               Log.Detail("ApplicationLauncher is ready");
               OnGUIAppLauncherReady();
            }
            else
            {
               Log.Detail("ApplicationLauncher is not ready");
               GameEvents.onGUIApplicationLauncherReady.Add(OnGUIAppLauncherReady);
            }
         }


         private void OnGUIAppLauncherReady()
         {
            if (destroyed) return;
            if (configuration.useStockToolbar)
            {
               if (ApplicationLauncher.Ready && stockToolbarButton == null)
               {
                  Log.Info("creating stock toolbar button");
                  stockToolbarButton = ApplicationLauncher.Instance.AddModApplication(
                  OnAppLaunchToggleOn,
                  OnAppLaunchToggleOff,
                  DummyVoid,
                  DummyVoid,
                  DummyVoid,
                  DummyVoid,
                  ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.MAPVIEW,
                  (Texture)GameDatabase.Instance.GetTexture(RESOURCE_PATH + "ToolbarIcon", false));
                  if (stockToolbarButton == null) Log.Warning("no stock toolbar button registered");
               }

            }
         }

         void OnAppLaunchToggleOn()
         {
            createConfigOnce();
            if (configWindow != null)
            {
               configWindow.SetVisible(true);
            }
         }

         void OnAppLaunchToggleOff()
         {
            if (configWindow != null)
            {
               configWindow.SetVisible(false);
            }
         }

         private void DummyVoid() { }


         private void AddToolbarButtons()
         {
            Log.Detail("adding toolbar buttons");
            String iconOn = RESOURCE_PATH + "IconOn_24";
            String iconOff = RESOURCE_PATH + "IconOff_24";
            toolbarButton = ToolbarManager.Instance.add("NanoGauges", "button");
            if (toolbarButton != null)
            {
               toolbarButton.TexturePath = iconOff;
               toolbarButton.ToolTip = "Open NanoGauges Configuration";
               toolbarButton.OnClick += (e) =>
               {
                  createConfigOnce();
                  if (configWindow != null) configWindow.registerToolbarButton(toolbarButton, iconOn, iconOff);
                  toggleConfigVisibility();
               };

               toolbarButton.Visibility = new GameScenesVisibility( GameScenes.FLIGHT );
            }
            else
            {
               Log.Error("toolbar button was null");
            }
         }

         private void toggleConfigVisibility()
         {
            configWindow.SetVisible(!configWindow.IsVisible());
         }

         private void createConfigOnce()
         {
            if (configWindow == null)
            {
               configWindow = new ConfigWindow(gauges);
               configWindow.CallOnWindowClose(OnConfigClose);
            }
         }

         public void OnConfigClose()
         {
            if (stockToolbarButton != null)
            {
               stockToolbarButton.toggleButton.Value = false;
            }
         }

         internal void OnDestroy()
         {
            Log.Info("destroying Nano Gauges");
            destroyed = true;
            Log.Info("log level is " + Log.GetLogLevel());
            if (toolbarButton!=null)
            {
               toolbarButton.Destroy();
            }
            if (stockToolbarButton != null)
            {
               Log.Detail("removing stock toolbar button");
               ApplicationLauncher.Instance.RemoveModApplication(stockToolbarButton);
            }
            gauges.SaveWindowPositions();
            configuration.Save();
         }


         private void SetGaugeSet(GaugeSet.ID id)
         {
            Log.Detail("SetGaugeSet to "+id);
            configuration.SetGaugeSet(id);
            gauges.ReflectGaugeSetChange();
         }

         public static HashSet<AbstractGauge> GetCluster(AbstractGauge gauge)
         {
            return gauges.GetCluster(gauge);
         }

         private void UpdateGauges()
         {
            gaugeUpdateTimer.Start();
            try
            {
               gauges.Update();
            }
            finally
            {
               gaugeUpdateTimer.Stop();
            }
         }

         private void UpdateNavGlobals()
         {
            navUpdateTimer.Start();
            try
            {
               NavGlobals.Update();
            }
            finally
            {
               navUpdateTimer.Stop();
            }
         }

         public void Update()
         {
            UpdateGauges();

            updateTimer.Start();
            try
            {
               // if log level is at least INFO and performance statistics are enabled 
               // write statistical data in the log every 10 seconds
               if (configuration.performanceStatisticsEnabled && Log.IsLogable(Log.LEVEL.INFO) && HighLogic.LoadedSceneIsFlight)
               {
                  if (lastPerformanceLog + 10000 < timer.ElapsedMilliseconds)
                  {
                     lastPerformanceLog = timer.ElapsedMilliseconds;
                     Log.Info("Nanogauges performance statistics:\n" + TimedStatistics.instance.ToString());
                  }
               }

               // check for Profile Hotkeys
               profileManager.Update();

               // check for keys
               //
               gauges.ShowCloseButtons(hotkeyManager.GetKey(HotkeyManager.HOTKEY_CLOSE_AND_DRAG));

               //
               bool hotkeyPressed = hotkeyManager.GetKey(HotkeyManager.HOTKEY_MAIN); // Input.GetKey(hotkey);

               // Hotkeys for Gaugesets
               if (!hotkeyPressed)
               {
                  // simple Hotkeys 
                  if (hotkeyManager.GetKeyDown(HotkeyManager.HOTKEY_HIDE))
                  {
                     if (gauges.Hidden())
                     {
                        gauges.Unhide();
                     }
                     else
                     {
                        gauges.Hide();
                     }
                  }
                  else if (hotkeyManager.GetKeyDown(HotkeyManager.HOTKEY_PREVSET))
                  {
                     SetGaugeSet(configuration.GetGaugeSetId().decrement());
                  }
                  else if (hotkeyManager.GetKeyDown(HotkeyManager.HOTKEY_NEXTSET))
                  {
                     SetGaugeSet(configuration.GetGaugeSetId().increment());
                  }
                  else if (hotkeyManager.GetKeyDown(HotkeyManager.HOTKEY_LOCK_PROFILE))
                  {
                     profileManager.ToggleLock();
                  }
                  else if (hotkeyManager.GetKeyDown(HotkeyManager.HOTKEY_NEXT_NAV))
                  {
                     NavGlobals.SelectNextAirfield();
                  }
                  else if (hotkeyManager.GetKeyDown(HotkeyManager.HOTKEY_PREV_NAV))
                  {
                     NavGlobals.SelectPrevAirfield();
                  }
               }
               else
               {
                  // Hotkeys in chord with main hotkey
                  //
                  if (hotkeyManager.GetKeyDown(HotkeyManager.HOTKEY_SET_STANDARD))
                  {
                     SetGaugeSet(GaugeSet.ID.STANDARD);
                  }
                  else if (hotkeyManager.GetKeyDown(HotkeyManager.HOTKEY_SET_LAUNCH))
                  {
                     SetGaugeSet(GaugeSet.ID.LAUNCH);
                  }
                  else if (hotkeyManager.GetKeyDown(HotkeyManager.HOTKEY_SET_LAND))
                  {
                     SetGaugeSet(GaugeSet.ID.LAND);
                  }
                  else if (hotkeyManager.GetKeyDown(HotkeyManager.HOTKEY_SET_DOCK))
                  {
                     SetGaugeSet(GaugeSet.ID.DOCK);
                  }
                  else if (hotkeyManager.GetKeyDown(HotkeyManager.HOTKEY_SET_ORBIT))
                  {
                     SetGaugeSet(GaugeSet.ID.ORBIT);
                  }
                  else if (hotkeyManager.GetKeyDown(HotkeyManager.HOTKEY_SET_FLIGHT))
                  {
                     SetGaugeSet(GaugeSet.ID.FLIGHT);
                  }
                  else if (hotkeyManager.GetKeyDown(HotkeyManager.HOTKEY_SET_SET1))
                  {
                     SetGaugeSet(GaugeSet.ID.SET1);
                  }
                  else if (hotkeyManager.GetKeyDown(HotkeyManager.HOTKEY_SET_SET2))
                  {
                     SetGaugeSet(GaugeSet.ID.SET2);
                  }
                  else if (hotkeyManager.GetKeyDown(HotkeyManager.HOTKEY_SET_SET3))
                  {
                     SetGaugeSet(GaugeSet.ID.SET3);
                  }
                  else if (hotkeyManager.GetKeyDown(HotkeyManager.HOTKEY_SET_ENABLE_ALL))
                  {
                     configuration.EnableAllGauges(gauges);
                  }
                  else if (hotkeyManager.GetKeyDown(HotkeyManager.HOTKEY_WINDOW_CONFIG))
                  {
                     createConfigOnce();
                     toggleConfigVisibility();
                  }
                  else if (hotkeyManager.GetKeyDown(HotkeyManager.HOTKEY_STANDARDLAYOUT))
                  {
                     gauges.LayoutCurrentGaugeSet(new StandardLayout(NanoGauges.gauges, configuration));
                  }
                  else if (hotkeyManager.GetKeyDown(HotkeyManager.HOTKEY_ALT_HIDE))
                  {
                     if (gauges.Hidden())
                     {
                        gauges.Unhide();
                     }
                     else
                     {
                        gauges.Hide();
                     }
                  }
                  else if (hotkeyManager.GetKeyDown(HotkeyManager.HOTKEY_AUTOLAYOUT))
                  {
                     gauges.AutoLayout();
                  }
                  else if (hotkeyManager.GetKeyDown(HotkeyManager.HOTKEY_ALIGNMENT_GAUGE))
                  {
                     gauges.ShowAligmentGauge(!gauges.IsAligmentGaugeVisible());
                  }
               }
            }
            finally
            {
               updateTimer.Stop();
            }
            hotkeyManager.ignoring = false;
         }
      }
   }
}