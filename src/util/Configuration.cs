using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NanoGaugesAdapter;


namespace Nereid
{
   namespace NanoGauges
   {
      public class Configuration
      {
         private static readonly String ROOT_PATH = Utils.GetRootPath();
         private static readonly String CONFIG_BASE_FOLDER = ROOT_PATH + "/GameData/";
         private const String FILE_NAME = "NanoGauges.dat";
         private static readonly Int16 FILE_MARKER = 0x7A7A;
         private static readonly Int16 FILE_VERSION = 1;

         public const int UNSCALED_VERTICAL_GAUGE_WIDTH = 42;
         public const int UNSCALED_VERTICAL_GAUGE_HEIGHT = 100;
         public const int UNSCALED_HORIZONTAL_GAUGE_WIDTH = 120;
         public const int UNSCALED_HORIZONTAL_GAUGE_HEIGHT = 42;

         public const double GAUGE_SCALE_100 = 1.0;
         public const double GAUGE_SCALE_110 = 1.1;
         public const double GAUGE_SCALE_120 = 1.2;
         public const double GAUGE_SCALE_150 = 1.5;

         private Log.LEVEL logLevel = Log.LEVEL.INFO;

         public bool gaugePositionsLocked { get; set; } 
         public bool gaugeMarkerEnabled  { get; set; }
         public bool tooltipsEnabled { get; set; }
         public bool snapinEnabled { get; set; }
         public bool trimIndicatorsEnabled { get; set; }
         public bool useStockToolbar { get; set; }
         public bool exactReadoutEnabled { get; set; }
         public double gaugeScaling { get; set; }
         public bool disableStockHeatIndicators { get; set; }
         // need a restart to take effect
         public int verticalGaugeWidth { get; private set; }
         public int verticalGaugeHeight { get; private set; }
         public int horizontalGaugeWidth { get; private set; }
         public int horizontalGaugeHeight { get; private set; }

         private const int snapinRange = Gauges.LAYOUT_GAP; // todo: remove constant and make configurable

         private KeyCode hotkeyCloseButtons = KeyCode.RightControl;

         // this class manages the default gauge positions realtive to sceen objects, navball, ...
         private readonly DefaultGaugePositionManager defaultGaugePositionManager = new DefaultGaugePositionManager();

         private GaugeSet.ID gaugeSet = GaugeSet.ID.STANDARD;
         private GaugeSet currentGaugeSet = GaugeSetPool.instance.GetGaugeSet(GaugeSet.ID.STANDARD);

         private bool gaugesInFlightEnabled = true;
         private bool gaugesInIvaEnabled = true;
         private bool gaugesInEvaEnabled = true;
         private bool gaugesInMapEnabled = true;

         public Configuration()
         {

            // Defaults
            gaugeScaling = GAUGE_SCALE_100;
            gaugePositionsLocked = false;
            gaugeMarkerEnabled = true;
            tooltipsEnabled = true;
            snapinEnabled = true;
            trimIndicatorsEnabled = true;
            useStockToolbar = !ToolbarManager.ToolbarAvailable;
            exactReadoutEnabled = false;
            verticalGaugeWidth    = UNSCALED_VERTICAL_GAUGE_WIDTH;
            verticalGaugeHeight   = UNSCALED_VERTICAL_GAUGE_HEIGHT;
            horizontalGaugeWidth  = UNSCALED_HORIZONTAL_GAUGE_WIDTH;
            horizontalGaugeHeight = UNSCALED_HORIZONTAL_GAUGE_HEIGHT;
            //
            disableStockHeatIndicators = false;
            // default window positions
            ResetAllWindowPositions();
         }

         public void EnableAllGauges(Gauges gauges)
         {
            foreach(AbstractGauge gauge in gauges)
            {
               SetGaugeEnabled(gauge.GetWindowId(), true);
            }
         }

         public void ResetAllWindowPositions()
         {
            foreach(GaugeSet set in GaugeSetPool.instance)
            {
               ResetWindowPositions(set);
            }
         }

         public void ResetWindowPositions()
         {
            ResetWindowPositions(currentGaugeSet);
         }

         public void ResetWindowPositions(GaugeSet set)
         {
            int LAYOUT_CELL_X = verticalGaugeWidth + Gauges.LAYOUT_GAP;
            int LAYOUT_CELL_Y = verticalGaugeHeight + Gauges.LAYOUT_GAP;
            int LAYOUT_RANGE_X = 3 * LAYOUT_CELL_X / 2;
            int LAYOUT_RANGE_Y = 3 * LAYOUT_CELL_Y / 2;


            Log.Info("reseting window positions (screen: " + Screen.width + "x" + Screen.height+")");
            int x0 = Screen.width - LAYOUT_CELL_X;
            int y0 = Screen.height-(int)(560*gaugeScaling);
            int vDX = LAYOUT_CELL_X;
            int vDY = LAYOUT_CELL_Y;
            int hDX = (int)(horizontalGaugeWidth * gaugeScaling) + Gauges.LAYOUT_GAP;
            int hDY = (int)(verticalGaugeWidth * gaugeScaling) + Gauges.LAYOUT_GAP;

            int n;

            //
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_SETS, 250 + 0*vDX, 10);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_INDICATOR, 250 + 1*vDX, 10);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_CAM, 250+2*vDX, 10);
            //
            // TAC life support
            n = 0;
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_O2, x0 - 1 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_CO2, x0 - 2 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_H2O, x0 - 3 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_WH2O, x0 - 4 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_WASTE, x0 - 5 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_FOOD, x0 - 6 * vDX, y0 + n * vDY);
            // Kethane
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_KETHANE, x0 - 7 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_KAIRIN, x0 - 8 * vDX, y0 + n * vDY);
            //
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_OSPD, x0 - 9 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_VVI, x0 - 10 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_VAI, x0 - 11 * vDX, y0 + n * vDY);
            //
            n = 1;
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_RADAR_ALTIMETER, x0 - 11 * vDX, y0 +  n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_VSI, x0 - 10 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_HSPD, x0 - 9 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_MASS, x0 - 8 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_G, x0 - 7 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_ATM, x0 - 6 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_ORBIT, x0 - 5 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_PEA, x0 - 4 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_APA, x0 - 3 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_INCL, x0 - 2 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_THRUST, x0 - 1 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_TWR, x0 - 0 * vDX, y0 + n * vDY);
            n = 2;
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_FUEL, x0 - 11 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_OXID, x0 - 10 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_FLOW, x0 - 9 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_MONO, x0 - 8 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_CHARGE, x0 - 7 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_AMP, x0 - 6 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_XENON, x0 - 5 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_AIRIN, x0 - 4 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_AIRPCT, x0 - 3 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_AOA, x0 - 2 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_DTGT, x0 - 1 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_VTGT, x0 - 0 * vDX, y0 + n * vDY);
            n = 3;
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_VT, x0 - 11 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_SRB, x0 - 10 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_MAXG, x0 - 9 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_EVAMP, x0 - 8 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_DISP, x0 - 7 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_ISPE, x0 - 6 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_TEMP, x0 - 5 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_GRAV, x0 - 4 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_SPD, x0 - 3 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_MACH, x0 - 2 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_PROPELLANT, x0 - 1 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_SHIELD, x0 - 0 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_ABOUT, 50, 50);
            set.SetWindowPosition(Constants.WINDOW_ID_CONFIG, 50, 50);
            n = 4;
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_Q, x0 - 11 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_HEAT, x0 - 10 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_IMPACT, x0 - 9 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_ALTIMETER, x0 - 8 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_ACCL, x0 - 7 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_HACCL, x0 - 6 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_VACCL, x0 - 5 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_EXTTEMP, x0 - 4 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_ATMTEMP, x0 - 3 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_ABLAT, x0 - 2 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_ORE, x0 - 1 * vDX, y0 + n * vDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_DRILLTEMP, x0 - 0 * vDX, y0 + n * vDY);

            // horizontal gauges
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_BIOME, 50, 100 + 0 * hDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_LATITUDE, 50, 100 + 1 * hDY);
            set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_LONGITUDE, 50, 100 + 2 * hDY);
         }


         public int GetSnapinRange()
         {
            return snapinRange;
         }


         public bool IsGaugesInFlightEnabled()
         {
            return gaugesInFlightEnabled;
         }

         public bool IsGaugesInIvaEnabled()
         {
            return gaugesInIvaEnabled;
         }

         public bool IsGaugesInEvaEnabled()
         {
            return gaugesInEvaEnabled;
         }

         public bool IsGaugesInMapEnabled()
         {
            return gaugesInMapEnabled;
         }

         public void SetGaugesInFlightEnabled(bool enabled)
         {
            this.gaugesInFlightEnabled = enabled;
         }

         public void SetGaugesInIvaEnabled(bool enabled)
         {
            this.gaugesInIvaEnabled = enabled;
         }

         public void SetGaugesInEvaEnabled(bool enabled)
         {
            this.gaugesInEvaEnabled = enabled;
         }

         public void SetGaugesInMapEnabled(bool enabled)
         {
            this.gaugesInMapEnabled = enabled;
         }

         public GaugeSet.ID GetGaugeSetId()
         {
            return gaugeSet;
         }

         public void SetGaugeSet(GaugeSet.ID set)
         {
            this.gaugeSet = set;
            currentGaugeSet = GaugeSetPool.instance.GetGaugeSet(set);
         }

         public bool IsGaugeEnabled(int id)
         {
            return currentGaugeSet.IsGaugeEnabled(id);
         }

         public void SetGaugeEnabled(int id, bool enabled)
         {
            currentGaugeSet.SetGaugeEnabled(id, enabled);
         }

         public Pair<int, int> GetWindowPosition(AbstractWindow window)
         {
            return currentGaugeSet.GetWindowPosition(window.GetWindowId());
         }

         public Pair<int, int> GetWindowPosition(int windowId)
         {
            return currentGaugeSet.GetWindowPosition(windowId);
         }

         public void SetWindowPosition(AbstractGauge gauge)
         {
            currentGaugeSet.SetWindowPosition(gauge.GetWindowId(), gauge.GetX(), gauge.GetY());
         }

         public void SetWindowPosition(int windowId, Pair<int,int> position)
         {
            currentGaugeSet.SetWindowPosition(windowId, position);
         }

         public void SetWindowPosition(int windowId, int x, int y)
         {
            currentGaugeSet.SetWindowPosition(windowId, new Pair<int, int>(x, y));
         }

         public void SetWindowPosition(AbstractGauge gauge, int x, int y)
         {
            currentGaugeSet.SetWindowPosition(gauge.GetWindowId(), x, y);
         }


         public KeyCode GetKeyCodeForHotkey()
         {
            return hotkeyCloseButtons;
         }

         public void SetKeyCodeForHotkey(KeyCode code)
         {
            hotkeyCloseButtons = code;
         }

         public Log.LEVEL GetLogLevel()
         {
            return logLevel;
         }

         public void SetLogLevel(Log.LEVEL level)
         {
            logLevel = level;
         }


         private void ReadGaugeSets(BinaryReader reader)
         {
            int cnt = reader.ReadInt16();
            for(int i=0; i<cnt; i++)
            {
               GaugeSet.ID id = (GaugeSet.ID)reader.ReadInt16();
               Log.Detail("loading gaugeset " + id);
               GaugeSet set = GaugeSetPool.instance.GetGaugeSet(id);
               ReadGaugeSet(reader, set);
            }
         }

         private void WriteGaugeSets(BinaryWriter writer)
         {
            writer.Write((Int16)GaugeSetPool.instance.Count());
            foreach(GaugeSet set in GaugeSetPool.instance)
            {
               Log.Detail("writing gaugeset " + set.GetId());
               writer.Write((Int16)set.GetId());
               WriteGaugeSet(writer, set);
            }
         }

         private void WriteGaugeSet(BinaryWriter writer, GaugeSet set)
         {
            WriteWindowPositions(writer, set);
            WriteGaugesEnabled(writer, set);
         }

         private void ReadGaugeSet(BinaryReader reader, GaugeSet set)
         {
            ReadWindowPositions(reader, set);
            ReadGaugesEnabled(reader, set);
         }

         private void WriteGaugesEnabled(BinaryWriter writer, GaugeSet set)
         {
            Log.Info("storing gauges enabled/disabled states");
            writer.Write((Int16)set.Count());
            Log.Info("writing " + set.Count() + " gauge states");
            foreach (int id in set.Keys())
            {
               writer.Write((Int32)id);
               bool enabled = set.IsGaugeEnabled(id);
               writer.Write(enabled);
               Log.Detail("window state written for window id " + id + ": " + enabled);
            }
         }

         private void ReadGaugesEnabled(BinaryReader reader, GaugeSet set)
         {
            Log.Info("loading window enabled/disabled states");
            int count = reader.ReadInt16();
            Log.Detail("loading "+count + " gauge states");
            for (int i = 0; i < count; i++)
            {
               int id = reader.ReadInt32();
               bool enabled = reader.ReadBoolean();
               set.SetGaugeEnabled(id, enabled);
            }
         }

         private void WriteWindowPositions(BinaryWriter writer, GaugeSet set)
         {
            Log.Info("storing window positions");
            writer.Write((Int16)set.Count());
            Log.Detail("writing " + set.Count() + " window positions");
            foreach (int id in set.Keys())
            {
               Pair<int, int> position = set.GetWindowPosition(id);
               writer.Write((Int32)id);
               writer.Write((Int16)position.first);
               writer.Write((Int16)position.second);
               Log.Trace("window position for window id " + id + " written: " + position.first + "/" + position.second);
            }
         }

         private void ReadWindowPositions(BinaryReader reader, GaugeSet set)
         {
            Log.Info("loading window positions");
            int count = reader.ReadInt16();
            Log.Detail("loading "+count + " window positions");
            for(int i=0; i<count; i++)
            {
               int id = reader.ReadInt32();
               int x = reader.ReadInt16();
               int y = reader.ReadInt16();
               Log.Trace("read window position for window id "+id+": "+x+"/"+y);
               set.SetWindowPosition(id, x, y);
            }
         }

         public void Save()
         {
            String filename = CONFIG_BASE_FOLDER + FILE_NAME;
            Log.Info("storing configuration in "+filename);
            try
            {
               using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
               {
                  writer.Write((Int16)logLevel);
                  // File Version
                  writer.Write((Int16)FILE_MARKER);  // marker
                  writer.Write(FILE_VERSION);
                  //
                  writer.Write(gaugePositionsLocked);
                  WriteGaugeSets(writer);
                  //
                  writer.Write(gaugeMarkerEnabled);
                  //
                  writer.Write((UInt16)hotkeyCloseButtons);
                  //
                  writer.Write(gaugesInFlightEnabled);
                  writer.Write(gaugesInMapEnabled);
                  writer.Write(gaugesInIvaEnabled);
                  writer.Write(gaugesInEvaEnabled);
                  //
                  writer.Write(snapinEnabled);
                  //
                  writer.Write(useStockToolbar);
                  //
                  writer.Write(trimIndicatorsEnabled);
                  //
                  writer.Write(exactReadoutEnabled);
                  //
                  writer.Write(tooltipsEnabled);
                  //
                  writer.Write(gaugeScaling);
                  //
                  writer.Write(disableStockHeatIndicators);
               }
            }
            catch
            {
               Log.Error("saving configuration failed");
            }
         }

         public void Load()
         {
            String filename = CONFIG_BASE_FOLDER+FILE_NAME;
            try
            {
               if (File.Exists(filename))
               {
                  Log.Info("loading configuration from " + filename);
                  using (BinaryReader reader = new BinaryReader(File.OpenRead(filename)))
                  {
                     logLevel = (Log.LEVEL) reader.ReadInt16();
                     Log.Info("log level loaded: "+logLevel);
                     // File Version
                     Int16 marker = reader.ReadInt16();
                     if (marker != FILE_MARKER)
                     {
                        Log.Error("invalid file structure");
                        throw new IOException("invalid file structure");
                     }
                     Int16 version = reader.ReadInt16();
                     if (version != FILE_VERSION)
                     {
                        Log.Error("incompatible file version");
                        throw new IOException("incompatible file version");
                     }
                     //
                     gaugePositionsLocked = reader.ReadBoolean();
                     ReadGaugeSets(reader);
                     //
                     gaugeMarkerEnabled = reader.ReadBoolean();
                     //
                     hotkeyCloseButtons = (KeyCode)reader.ReadUInt16();
                     //
                     gaugesInFlightEnabled = reader.ReadBoolean();
                     gaugesInMapEnabled = reader.ReadBoolean();
                     gaugesInIvaEnabled = reader.ReadBoolean();
                     gaugesInEvaEnabled = reader.ReadBoolean();
                     //
                     snapinEnabled = reader.ReadBoolean();
                     //
                     useStockToolbar = reader.ReadBoolean()  || !ToolbarManager.ToolbarAvailable;
                     //
                     trimIndicatorsEnabled = reader.ReadBoolean();
                     //
                     exactReadoutEnabled = reader.ReadBoolean();
                     //
                     tooltipsEnabled = reader.ReadBoolean();
                     //
                     gaugeScaling = reader.ReadDouble();
                     verticalGaugeWidth =    (int)(UNSCALED_VERTICAL_GAUGE_WIDTH    * gaugeScaling);
                     verticalGaugeHeight =   (int)(UNSCALED_VERTICAL_GAUGE_HEIGHT   * gaugeScaling);
                     horizontalGaugeWidth =  (int)(UNSCALED_HORIZONTAL_GAUGE_WIDTH  * gaugeScaling);
                     horizontalGaugeHeight = (int)(UNSCALED_HORIZONTAL_GAUGE_HEIGHT * gaugeScaling);
                     //
                     disableStockHeatIndicators = reader.ReadBoolean();
                  }
               }
               else
               {
                  Log.Info("no config file: default configuration");
               }
            }
            catch
            {
               Log.Warning("loading configuration failed or incompatible file");
            }
            finally
            {
               // enabling both won't make sense
               if(tooltipsEnabled)
               {
                  exactReadoutEnabled = false;
               }
            }
         }
      }
   }
}
