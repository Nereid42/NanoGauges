using System;
using UnityEngine;
using NanoGaugesAdapter;

namespace Nereid
{
   namespace NanoGauges
   {
      class ConfigWindow : AbstractWindow
      {
         private static readonly int GAUGES_HEIGHT = 270;
         private static readonly GUIStyle STYLE_ENABLE_DISABLE_ALL_TOGGLE = new GUIStyle(HighLogic.Skin.button);
         private static readonly GUIStyle STYLE_COPYPASTE_BUTTONS = new GUIStyle(HighLogic.Skin.button);
         private static readonly GUIStyle STYLE_TOGGLE_2_PER_ROW = new GUIStyle(HighLogic.Skin.toggle);
         private static readonly GUIStyle STYLE_TOGGLE_4_PER_ROW = new GUIStyle(HighLogic.Skin.toggle);
         private static readonly GUIStyle STYLE_LABEL = new GUIStyle(HighLogic.Skin.label);
         private static readonly GUIStyle STYLE_SCROLLVIEW = new GUIStyle(HighLogic.Skin.scrollView);

         static ConfigWindow()
         {
            STYLE_ENABLE_DISABLE_ALL_TOGGLE.stretchWidth = false;
            STYLE_ENABLE_DISABLE_ALL_TOGGLE.fixedWidth = 120;
            STYLE_COPYPASTE_BUTTONS.stretchWidth = false;
            STYLE_COPYPASTE_BUTTONS.fixedWidth = 70;
            STYLE_TOGGLE_2_PER_ROW.margin = new RectOffset(0, 150, 0, 0);
            STYLE_TOGGLE_4_PER_ROW.margin = new RectOffset(0, 50, 0, 0);
            STYLE_LABEL.stretchWidth = true;
            STYLE_SCROLLVIEW.stretchWidth = true;
         }


         private IButton toolbarButton;
         private String toolbarButtonTextureOn;
         private String toolbarButtonTextureOff;

         private Vector2 scrollPosGauges = Vector2.zero;

         private readonly Gauges gauges;

         private bool allGaugesEnables;

         private bool clipboardNotEmpty = false;

         public ConfigWindow(Gauges gauges)
            : base(Constants.WINDOW_ID_CONFIG, "NanoGauges Configuration")
         {
            this.gauges = gauges;

            SetSize(350, 300);
            CenterWindow();
         }

         protected override void OnWindow(int id)
         {
            Configuration config = NanoGauges.configuration;

            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Close", HighLogic.Skin.button)) SetVisible(false);
            GUILayout.EndHorizontal();

            GUILayout.Label("Log Level:", STYLE_LABEL);
            GUILayout.BeginHorizontal();
            Log.LEVEL level = Log.GetLevel();
            LogLevelButton(Log.LEVEL.OFF, "OFF");
            LogLevelButton(Log.LEVEL.ERROR, "ERROR");
            LogLevelButton(Log.LEVEL.WARNING, "WARNING");
            LogLevelButton(Log.LEVEL.INFO, "INFO");
            LogLevelButton(Log.LEVEL.DETAIL, "DETAIL");
            LogLevelButton(Log.LEVEL.TRACE, "TRACE");
            GUILayout.EndHorizontal();

            GUILayout.Label("Hotkey:", STYLE_LABEL);
            GUILayout.BeginHorizontal();
            KeyCodeButton(KeyCode.LeftControl, "LEFT CTRL");
            KeyCodeButton(KeyCode.RightControl, "RIGHT CTRL");
            KeyCodeButton(KeyCode.LeftShift, "LEFT SHIFT");
            KeyCodeButton(KeyCode.RightShift, "RIGHT SHIFT");
            KeyCodeButton(KeyCode.LeftAlt, "LEFT ALT");
            KeyCodeButton(KeyCode.RightAlt, "RIGHT ALT");
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            // Reset Window Postions
            if (GUILayout.Button("Reset Gauge Positions", HighLogic.Skin.button))
            {
               gauges.ResetPositions();
            }
            // layout gauges
            if (GUILayout.Button("Automatic Layout", HighLogic.Skin.button))
            {
               gauges.AutoLayout();
               config.Save();
            }            //
            GUILayout.EndHorizontal();
            // Save Window Postions
            if (GUILayout.Button("Save Gauge Positions", HighLogic.Skin.button))
            {
               gauges.SaveWindowPositions();
               config.Save();
            }
            // selector gauge
            if (GUILayout.Button("Selector Gauge Location From Current Profile", HighLogic.Skin.button))
            {
               gauges.CopySelectorPositionFrom(config.GetGaugeSetId());
            }

            GUILayout.Label("Settings:", STYLE_LABEL);
            // Positions Locked
            GUILayout.BeginVertical();
            config.gaugePositionsLocked = GUILayout.Toggle(config.gaugePositionsLocked, "Gauge positions locked", HighLogic.Skin.toggle);
            config.gaugeMarkerEnabled = GUILayout.Toggle(config.gaugeMarkerEnabled, "Gauge marker enabled", HighLogic.Skin.toggle);
            // tooltips & exact readout
            GUILayout.BeginHorizontal();
            if (config.exactReadoutEnabled) config.tooltipsEnabled = false;
            config.tooltipsEnabled = GUILayout.Toggle(config.tooltipsEnabled, "Tooltips enabled", STYLE_TOGGLE_2_PER_ROW);
            if (config.tooltipsEnabled) config.exactReadoutEnabled = false;
            config.exactReadoutEnabled = GUILayout.Toggle(config.exactReadoutEnabled, "Exact readout enabled", STYLE_TOGGLE_2_PER_ROW);
            GUILayout.EndHorizontal();
            config.snapinEnabled = GUILayout.Toggle(config.snapinEnabled, "Snapin enabled", HighLogic.Skin.toggle);
            // Stock Toolbar
            if (ToolbarManager.ToolbarAvailable)
            {
               config.useStockToolbar = GUILayout.Toggle(config.useStockToolbar, "Use Stock Toolbar (needs a restart to take effekt)", HighLogic.Skin.toggle);
            }
            // Cam
            DrawCameraModeToggles();
            GUILayout.EndVertical();
            //
            // GAUGES ON/OFF
            GUILayout.BeginHorizontal();
            GUILayout.Label("Gauges:", STYLE_LABEL);
            GUILayout.FlexibleSpace();
            DrawCopyPasteButtons();
            DrawEnableDisableAllButton();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GaugeSetButton(GaugeSet.ID.STANDARD, "STANDARD");
            GaugeSetButton(GaugeSet.ID.LAUNCH, "LAUNCH");
            GaugeSetButton(GaugeSet.ID.LAND, "LAND");
            GaugeSetButton(GaugeSet.ID.DOCK, "DOCK");
            GaugeSetButton(GaugeSet.ID.ORBIT, "ORBIT");
            GaugeSetButton(GaugeSet.ID.FLIGHT, "FLIGHT");
            GaugeSetButton(GaugeSet.ID.SET1 , "SET 1");
            GaugeSetButton(GaugeSet.ID.SET2, "SET 2");
            GaugeSetButton(GaugeSet.ID.SET3, "SET 3");
            GUILayout.EndHorizontal();
            scrollPosGauges = GUILayout.BeginScrollView(scrollPosGauges, false, true, HighLogic.Skin.horizontalScrollbar, HighLogic.Skin.verticalScrollbar, STYLE_SCROLLVIEW, GUILayout.Height(GAUGES_HEIGHT));
            this.allGaugesEnables = true;
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_SETS, "Gaugeset selector gauge enabled");
            config.trimIndicatorsEnabled = GUILayout.Toggle(config.trimIndicatorsEnabled, "Trim indicators enabled (restart required)", HighLogic.Skin.toggle);
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_CAM, "Camera indicator gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_ALTIMETER, "Altimeter gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_RADAR_ALTIMETER, "Radar altimeter gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_VSI, "VSI (vertical speed indicator) gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_VT, "Terminal velocity deviation gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_HSPD, "Horizontal speed gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_SPD, "Speed gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_MASS, "Vessel mass gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_OSPD, "Orbital speed gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_FUEL, "Fuel gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_FLOW, "Fuel flow gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_CHARGE, "Electric charge gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_AMP, "Amperemeter gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_SRB, "Solid fuel gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_G, "Accelerometer gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_MAXG, "Max g gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_ORBIT, "Orbit gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_INCL, "Orbit inclination gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_MONO, "Monopropellant gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_EVAMP, "EVA monopropellant gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_XENON, "Xenon gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_OXID, "Oxidizer gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_ATM, "Atmosphere gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_PEA, "Periapsis gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_APA, "Apoapsis gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_AIRIN, "Absolute air-intake gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_AIRPCT, "Relative air-intake gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_THRUST, "Thrust gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_TWR, "TWR gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_ISPE, "ISP/E gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_DISP, "ISP rate gauge (Delta Isp/s) enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_AOA, "AoA (angle of attack) gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_VAI, "VAI (vertical attidute indicator) gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_VVI, "VVI (vertical velocity indicator) gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_DTGT, "Distance to target gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_VTGT, "Velocity to target gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_TEMP, "Temperature gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_GRAV, "Gravity gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_O2, "Oxygen gauge enabled (TAC life suppord required)");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_CO2, "CO2 gauge enabled (TAC life suppord required)");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_H2O, "Water gauge enabled (TAC life suppord required)");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_FOOD, "Food gauge enabled (TAC life suppord required)");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_WH2O, "Wastewater gauge enabled (TAC life suppord required)");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_WASTE, "Waste gauge enabled (TAC life suppord required)");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_KETHANE, "Kethane gauge enabled (Kethane plugin required)");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_KAIRIN, "Kethane air intake gauge enabled (Kethane plugin required)");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_MACH, "Mach gauge enabled (FAR plugin required)");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_Q, "Dynamic pressure (Q) gauge enabled (FAR plugin required)");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_SHIELD, "Ablative shielding gauge enabled (Deadly Reentry plugin required)");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_HEAT, "Temperatur gauge for heat shield enabled (Deadly Reentry plugin required)");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_IMPACT, "Impact gauge");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_ACCL, "Acceleration gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_HACCL, "Horizontal acceleration gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_VACCL, "Vertical acceleration gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_EXTTEMP, "External temperature gauge enabled");
            GaugeEnabledToggle(Constants.WINDOW_ID_GAUGE_ATMTEMP, "Atmosphere temperature gauge enabled");
            GUILayout.EndScrollView();
            
            GUILayout.EndVertical();
            DragWindow();
         }

         private void DrawCameraModeToggles()
         {
            Configuration config = NanoGauges.configuration;
            GUILayout.BeginHorizontal();
            gauges.SetEnabledInCamera(CameraManager.CameraMode.Flight, GUILayout.Toggle(config.IsGaugesInFlightEnabled(), "Flight", STYLE_TOGGLE_4_PER_ROW));
            gauges.SetEnabledInCamera(CameraManager.CameraMode.Map, GUILayout.Toggle(config.IsGaugesInMapEnabled(), "Map", STYLE_TOGGLE_4_PER_ROW));
            gauges.SetEnabledInCamera(CameraManager.CameraMode.IVA, GUILayout.Toggle(config.IsGaugesInIvaEnabled(), "IVA", STYLE_TOGGLE_4_PER_ROW));
            gauges.SetEnabledInEva(GUILayout.Toggle(config.IsGaugesInEvaEnabled(), "EVA", STYLE_TOGGLE_4_PER_ROW));
            GUILayout.EndHorizontal();
         }

         private void DrawCopyPasteButtons()
         {
            Configuration config = NanoGauges.configuration;
            GaugeSet currentSet = GaugeSetPool.instance.GetGaugeSet(config.GetGaugeSetId());
            GaugeSet clipboard = GaugeSetPool.instance.GetClipboard();

            if (GUILayout.Button("Copy", STYLE_COPYPASTE_BUTTONS))
            {
               clipboard.copyFrom(currentSet);
               gauges.ReflectGaugeSetChange();
               clipboardNotEmpty = true;
            }
            if (GUILayout.Button("Paste", STYLE_COPYPASTE_BUTTONS) && clipboardNotEmpty)
            {
               currentSet.copyFrom(clipboard);
               gauges.ReflectGaugeSetChange();
            }
            /*if (GUILayout.Button("Paste/Pos", STYLE_COPYPASTE_BUTTONS) && clipboardNotEmpty)
            {
               currentSet.copyArrangementFrom(clipboard);
               gauges.ReflectGaugeSetChange();
            }*/
         }

         private void DrawEnableDisableAllButton()
         {
            if (GUILayout.Button(allGaugesEnables ? "Disable all gauges" : "Enable all gauges", STYLE_ENABLE_DISABLE_ALL_TOGGLE))
            {
               if (allGaugesEnables)
               {
                  gauges.DisableAllGauges();
               }
               else
               {
                  gauges.EnableAllGauges();
               }
            }
         }

         private void GaugeEnabledToggle(int windowId, String text)
         {
            if (!gauges.ContainsId(windowId)) return;
            try
            {
               Configuration config = NanoGauges.configuration;
               bool enabled =gauges.IsGaugeEnabled(windowId);
               gauges.SetGaugeEnabled(windowId, GUILayout.Toggle(enabled, text, STYLE_TOGGLE_4_PER_ROW));
               this.allGaugesEnables = this.allGaugesEnables && enabled;

            }
            catch
            {
               Log.Error("failed to create gauge toggle for '"+text+"'");
               throw;
            }
         }


         private void LogLevelButton(Log.LEVEL level, String text)
         {
            if (GUILayout.Toggle(Log.GetLevel() == level, text, HighLogic.Skin.button) && Log.GetLevel() != level)
            {
               NanoGauges.configuration.SetLogLevel(level);
               Log.SetLevel(level);
            }
         }

         private void KeyCodeButton(KeyCode code, String text)
         {
            Configuration config = NanoGauges.configuration;
            if (GUILayout.Toggle(config.GetKeyCodeForHotkey() == code, text, HighLogic.Skin.button) )
            {
               config.SetKeyCodeForHotkey(code);
            }
         }

         private void GaugeSetButton(GaugeSet.ID id, String text)
         {
            Configuration config = NanoGauges.configuration;
            if (GUILayout.Toggle(id==config.GetGaugeSetId(), text, HighLogic.Skin.button))
            {
               if(config.GetGaugeSetId()!=id)
               {
                  Log.Detail("gauge set in config changed to "+id);
                  config.SetGaugeSet(id);
                  gauges.ReflectGaugeSetChange();
               }
            }
         }


         protected override void OnOpen()
         {
            base.OnOpen();
            CenterWindow();
            if (toolbarButton != null)
            {
               toolbarButton.TexturePath = toolbarButtonTextureOn;
            }
         }

         protected override void OnClose()
         {
            base.OnClose();
            if (toolbarButton != null)
            {
               toolbarButton.TexturePath = toolbarButtonTextureOff;
            }
         }

         public override int GetInitialWidth()
         {
            return 470;
         }

         protected override int GetInitialHeight()
         {
            return 300;
         }

         public void registerToolbarButton(IButton button, String textureOn, String textureOff)
         {
            this.toolbarButton = button;
            this.toolbarButtonTextureOn = textureOn;
            this.toolbarButtonTextureOff = textureOff;
         }
      }


   }
}
