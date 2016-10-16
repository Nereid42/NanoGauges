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
         public const double GAUGE_SCALE_200 = 2.0;

         private Log.LEVEL logLevel = Log.LEVEL.INFO;

         public bool gaugePositionsLocked { get; set; } 
         public bool gaugeMarkerEnabled  { get; set; }
         public bool tooltipsEnabled { get; set; }
         public bool snapinEnabled { get; set; }
         public bool trimIndicatorsEnabled { get; set; }
         public bool useStockToolbar { get; set; }
         public bool exactReadoutEnabled { get; set; }
         public bool performanceStatisticsEnabled { get; set; }
         // need a restart to take effect
         public double gaugeScaling { get; set; }
         public int verticalGaugeWidth { get; private set; }
         public int verticalGaugeHeight { get; private set; }
         public int horizontalGaugeWidth { get; private set; }
         public int horizontalGaugeHeight { get; private set; }

         public const int snapinRange = Gauges.LAYOUT_GAP; // todo: remove constant and make configurable

         //public KeyCode hotkey  { get; set; }

         // this class manages the default gauge positions realtive to sceen objects, navball, ...
         private readonly DefaultGaugePositionManager defaultGaugePositionManager = new DefaultGaugePositionManager();

         private GaugeSet.ID gaugeSet = GaugeSet.ID.STANDARD;
         private GaugeSet currentGaugeSet = GaugeSetPool.instance.GetGaugeSet(GaugeSet.ID.STANDARD);

         public bool gaugesInFlightEnabled { get; set; }
         public bool gaugesInIvaEnabled { get; set; }
         public bool gaugesInEvaEnabled { get; set; }
         public bool gaugesInMapEnabled { get; set; }

         public Configuration()
         {
            // Defaults
            gaugePositionsLocked = true;
            gaugeMarkerEnabled = true;
            tooltipsEnabled = true;
            snapinEnabled = true;
            trimIndicatorsEnabled = true;
            useStockToolbar = !ToolbarManager.ToolbarAvailable;
            exactReadoutEnabled = false;
            performanceStatisticsEnabled = false;
            gaugeScaling = GAUGE_SCALE_100;
            verticalGaugeWidth = UNSCALED_VERTICAL_GAUGE_WIDTH;
            verticalGaugeHeight   = UNSCALED_VERTICAL_GAUGE_HEIGHT;
            horizontalGaugeWidth  = UNSCALED_HORIZONTAL_GAUGE_WIDTH;
            horizontalGaugeHeight = UNSCALED_HORIZONTAL_GAUGE_HEIGHT;
            gaugesInFlightEnabled = true;
            gaugesInIvaEnabled = true;
            gaugesInEvaEnabled = true;
            gaugesInMapEnabled = true;
         }

         public void EnableAllGauges(Gauges gauges)
         {
            foreach(AbstractGauge gauge in gauges)
            {
               SetGaugeEnabled(gauge.GetWindowId(), true);
            }
         }

         public void DisableAllGauges(Gauges gauges)
         {
            foreach (AbstractGauge gauge in gauges)
            {
               SetGaugeEnabled(gauge.GetWindowId(), false);
            }
         }

         public bool IsCurrentGaugeSet(GaugeSet set)
         {
            return IsCurrentGaugeSet(set.GetId());
         }

         public bool IsCurrentGaugeSet(GaugeSet.ID id)
         {
            return currentGaugeSet.GetId() == id;
         }

         public void ResetAllWindowPositions(Gauges gauges)
         {
            // The default layout only works well in screen width at 1900 pixels or above
            if(Screen.width>=1900)
            {
               ResetAllWindowPositions(gauges, new DefaultLayout(gauges, this));
            }
            else
            {
               // Screen is tiny, so prefer a clusterlayout at first
               ResetAllWindowPositions(gauges, new ClusterLayout(gauges, this));
            }
         }

         private void ResetAllWindowPositions(Gauges gauges, GaugeLayout layout)
         {
            foreach(GaugeSet set in GaugeSetPool.instance)
            {
               LayoutGaugeSet(set, layout);
            }
         }


         public void LayoutCurrentGaugeSet(GaugeLayout layout)
         {
            LayoutGaugeSet(currentGaugeSet, layout);
         }

         public void LayoutGaugeSet(GaugeSet set, GaugeLayout layout)
         {
            Log.Detail("layout of gauges in set "+set+" (screen: " + Screen.width + "x" + Screen.height + ") with "+layout);
            layout.Layout(set);
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

         public void ChangeGaugeScaling(double scaling)
         {
            Log.Detail("ChangeGaugeScaling on configuration called with scacle " + scaling);
            gaugeScaling = scaling;
            verticalGaugeWidth = (int)(UNSCALED_VERTICAL_GAUGE_WIDTH * gaugeScaling);
            verticalGaugeHeight = (int)(UNSCALED_VERTICAL_GAUGE_HEIGHT * gaugeScaling);
            horizontalGaugeWidth = (int)(UNSCALED_HORIZONTAL_GAUGE_WIDTH * gaugeScaling);
            horizontalGaugeHeight = (int)(UNSCALED_HORIZONTAL_GAUGE_HEIGHT * gaugeScaling);
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
            Log.Info("storing gauges enabled/disabled states for gauge set "+set);
            writer.Write((Int16)set.Count());
            Log.Detail("writing " + set.Count() + " gauge states");
            foreach (int id in set.Keys())
            {
               writer.Write((Int32)id);
               bool enabled = set.IsGaugeEnabled(id);
               writer.Write(enabled);
               Log.Trace("window state written for window id " + id + ": " + enabled);
            }
         }

         private void ReadGaugesEnabled(BinaryReader reader, GaugeSet set)
         {
            Log.Info("loading window enabled/disabled states for gauge set "+set);
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
            Log.Info("loading window positions for gauge set "+set);
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
                  // not used
                  writer.Write((UInt16)0);
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
                  writer.Write(performanceStatisticsEnabled);
                  //
                  // ProfileManager
                  NanoGauges.profileManager.Write(writer);
                  //
                  // HotkeyManager
                  NanoGauges.hotkeyManager.Write(writer);
               }
            }
            catch
            {
               Log.Error("saving configuration failed");
            }
            finally
            {
               Log.Detail("storing of configuration done");

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
                     Log.SetLevel(logLevel);
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
                     // not used
                     reader.ReadUInt16();
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
                     // currently not used
                     trimIndicatorsEnabled = reader.ReadBoolean();
                     //
                     exactReadoutEnabled = reader.ReadBoolean();
                     //
                     tooltipsEnabled = reader.ReadBoolean();
                     //
                     ChangeGaugeScaling(reader.ReadDouble());
                     //
                     performanceStatisticsEnabled = reader.ReadBoolean();
                     //
                     // ProfileManager
                     NanoGauges.profileManager.Read(reader);
                     //
                     // HotkeyManager
                     NanoGauges.hotkeyManager.Read(reader);
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
               Log.Detail("loading of configuration done");
            }
         }
      }
   }
}
