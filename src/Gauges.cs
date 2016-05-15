using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nereid
{
   namespace NanoGauges
   {
      public class Gauges : IEnumerable<AbstractGauge>
      {
         private Dictionary<int, AbstractGauge> gauges = new Dictionary<int, AbstractGauge>();

         private readonly ResourceInspecteur resourceInspecteur = new ResourceInspecteur();
         private readonly EngineInspecteur engineInspecteur = new EngineInspecteur();
         private readonly SensorInspecteur sensorInspecteur = new SensorInspecteur();
         private readonly VesselInspecteur vesselInspecteur = new VesselInspecteur();
         private readonly AccelerationInspecteur velocityInspecteur = new AccelerationInspecteur();
         private readonly BiomeInspecteur biomeInspecteur = new BiomeInspecteur();

         public const int LAYOUT_GAP = 8;

         private volatile bool hidden = false;

         private volatile CameraManager.CameraMode currentCamMode = CameraManager.CameraMode.Flight;
         private volatile bool isEvaCam = false;

         private long updateCycle = 0;

         // debug
         private readonly TimedStatistics.Timer guiTimer = TimedStatistics.instance.GetTimer("GUI");


         public Gauges()
         {
            Log.Info("creating gaues");
            AddGauge(new SelectorGauge(this));
            AddGauge(new VsiGauge());
            AddGauge(new HorizontalVelocityGauge());
            AddGauge(new RadarAltimeter());
            AddGauge(new MassGauge(vesselInspecteur));
            AddGauge(new FuelGauge(resourceInspecteur));
            AddGauge(new FuelFlowGauge(resourceInspecteur));
            AddGauge(new ElectricChargeGauge(resourceInspecteur));
            AddGauge(new AmpereMeter(resourceInspecteur));
            AddGauge(new AcceleroMeter());
            AddGauge(new OrbitGauge());
            AddGauge(new MonopropellantGauge(resourceInspecteur));
            AddGauge(new OxidizerGauge(resourceInspecteur));
            AddGauge(new AtmosphereGauge());
            AddGauge(new PeriapsisGauge());
            AddGauge(new ApoapsisGauge());
            AddGauge(new AirIntakeGauge(resourceInspecteur));
            AddGauge(new AirIntakePctGauge(resourceInspecteur));
            AddGauge(new XenonGauge(resourceInspecteur));
            AddGauge(new ThrustGauge(engineInspecteur));
            AddGauge(new ThrustWeightRatioGauge(engineInspecteur));
            AddGauge(new VelocityGauge());
            AddGauge(new AngleOfAttackGauge());
            AddGauge(new VerticalAttitudeIndicatorGauge());
            AddGauge(new VerticalVelocityIndicatorGauge());
            AddGauge(new DistanceToTargetGauge());
            AddGauge(new OrbitInclinationGauge());
            AddGauge(new IspDeltaGauge(engineInspecteur));
            AddGauge(new IspPerEngineGauge(engineInspecteur));
            AddGauge(new TempGauge(sensorInspecteur));
            AddGauge(new GravGauge(sensorInspecteur));
            AddGauge(new BarometricGauge(sensorInspecteur));
            AddGauge(new SeismicGauge(sensorInspecteur));
            AddGauge(new EvaMonopropellantGauge(resourceInspecteur));
            AddGauge(new MaxGeeGauge());
            AddGauge(new SolidFuelGauge(resourceInspecteur));
            AddGauge(new OrbitalVelocityGauge());
            AddGauge(new TerminalVelocityGauge(vesselInspecteur));
            AddGauge(new VelocityToTargetGauge());
            AddGauge(new CameraGauge());
            AddGauge(new MachGauge());
            AddGauge(new QGauge());
            AddGauge(new HeatGauge(vesselInspecteur));
            AddGauge(new ImpactTimeGauge());
            AddGauge(new Altimeter());
            AddGauge(new AccelerationGauge(velocityInspecteur));
            AddGauge(new HorizontalAccelerationGauge(velocityInspecteur));
            AddGauge(new VerticalAccelerationGauge(velocityInspecteur));
            AddGauge(new ExternalTempGauge()); 
            AddGauge(new AtmosphereTempGauge());
            AddGauge(new AblatorGauge(resourceInspecteur));
            AddGauge(new OreGauge(resourceInspecteur));
            AddGauge(new DrillTempGauge(vesselInspecteur));
            // NOT WORKING in KSP1.1.0: AddGauge(new IndicatorGauge(vesselInspecteur,engineInspecteur));
            AddGauge(new PropellantPctGauge(engineInspecteur));
            AddGauge(new TimeToApoapsisGauge());
            AddGauge(new TimeToPeriapsisGauge());
            // not working
            //AddGauge(new TimeToTransistionGauge());

            // horizontal gauges
            AddGauge(new BiomeGauge(biomeInspecteur));
            AddGauge(new LatitudeGauge());
            AddGauge(new LongitudeGauge());

            // TAC life support and Kerbalism (only added if TAC installed)
            AddOptionalResourceGauge(new OxygenGauge(resourceInspecteur));
            AddOptionalResourceGauge(new CarbonDioxideGauge(resourceInspecteur));
            AddOptionalResourceGauge(new WaterGauge(resourceInspecteur));
            AddOptionalResourceGauge(new WasteWaterGauge(resourceInspecteur));
            AddOptionalResourceGauge(new WasteGauge(resourceInspecteur));
            AddOptionalResourceGauge(new FoodGauge(resourceInspecteur));
            AddOptionalResourceGauge(new KethaneGauge(resourceInspecteur));
            AddOptionalResourceGauge(new KethaneAirIntakeGauge(resourceInspecteur));
            AddOptionalResourceGauge(new ShieldGauge(resourceInspecteur));

            // Kerbalism
            AddOptionalResourceGauge(new CO2Gauge(resourceInspecteur));
            AddOptionalResourceGauge(new CrapGauge(resourceInspecteur));
            
            // Real Fuels
            AddOptionalResourceGauge(new KarboniteGauge(resourceInspecteur));
         }

         public AbstractGauge GetGauge(int id)
         {
            return gauges[id];
         }

         public void DrawGauges()
         {
            guiTimer.Start();
            if (!hidden)
            {
               foreach (AbstractGauge gauge in gauges.Values)
               {
                  try
                  {
                     if (NanoGauges.configuration.IsGaugeEnabled(gauge.GetWindowId()))
                     {
                        gauge.OnDraw();
                     }
                  }
                  catch
                  {
                     Log.Error("Exception in OnDraw() in " + gauge.GetType());
                  }
               }
            }
            guiTimer.Stop();
         }


         private void SetEnabledGaugesVisible(bool visible)
         {
            Log.Detail("set gauges visible if enabled to " + visible);
            foreach (AbstractGauge gauge in gauges.Values)
            {
               bool enabled = NanoGauges.configuration.IsGaugeEnabled(gauge.GetWindowId());
               gauge.SetVisible(enabled && visible);
            }
            hidden = !visible;
         }

         public void Hide()
         {
            SetEnabledGaugesVisible(false);
         }

         public void Unhide()
         {
            SetEnabledGaugesVisible(true);
         }

         public bool Hidden()
         {
            return hidden;
         }


         public System.Collections.IEnumerator GetEnumerator()
         {
            return gauges.Values.GetEnumerator();
         }

         IEnumerator<AbstractGauge> IEnumerable<AbstractGauge>.GetEnumerator()
         {
            return gauges.Values.GetEnumerator();
         }

         public void ResetGauges()
         {
            foreach (AbstractGauge gauge in gauges.Values)
            {
               gauge.Reset();
            }
         }

         public void ShowGauges()
         {
            ResetInspecteurs();
            //
            this.currentCamMode = CameraManager.Instance.currentCameraMode;
            this.isEvaCam = IsEvaCamera();
            this.hidden = false;
            //
            foreach (AbstractGauge gauge in gauges.Values)
            {
               if (NanoGauges.configuration.IsGaugeEnabled(gauge.GetWindowId()) && IsEnabledInCamera(this.currentCamMode,IsEvaCamera()))
               {
                  gauge.Reset();
                  gauge.SetVisible(true);
               }
            }
         }

         private bool IsEvaCamera()
         {
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel == null) return false;
            return vessel.isEVA;
         }

         public void SaveWindowPositions()
         {
            Log.Info("save of gauge screen positions");
            foreach (AbstractGauge gauge in gauges.Values)
            {
               Pair<int,int> position = new Pair<int,int>(gauge.GetX(),gauge.GetY());
               NanoGauges.configuration.SetWindowPosition(gauge.GetWindowId(),position);
            }
         }

         public void LayoutCurrentGaugeSet(GaugeLayout layout)
         {
            Log.Info("reset of gauge screen positions");
            NanoGauges.configuration.LayoutCurrentGaugeSet(layout);
            foreach (AbstractGauge gauge in gauges.Values)
            {
               try
               {
                  Pair<int, int> position = NanoGauges.configuration.GetWindowPosition(gauge.GetWindowId());
                  gauge.SetPosition(position);
               }
               catch(Exception e)
               {
                  Log.Error("layout exception for gauge "+gauge.GetWindowId()+": "+e.GetType().Name+", "+e.Message);
               }
            }
         }

         public void CopySelectorPositionFrom(GaugeSet.ID id)
         {
            GaugeSet source = GaugeSetPool.instance.GetGaugeSet(id);
            Pair<int, int> position = source.GetWindowPosition(Constants.WINDOW_ID_GAUGE_SETS);
            foreach(GaugeSet set in GaugeSetPool.instance)
            {
               set.SetWindowPosition(Constants.WINDOW_ID_GAUGE_SETS, position);
            }
         }

         private void AddGauge(AbstractGauge gauge)
         {
            int windowId = gauge.GetWindowId();
            if(!gauges.ContainsKey(windowId))
            {
               this.gauges.Add(windowId, gauge);
            }
            else
            {
               Log.Error("gauge id "+windowId+" registered multiple times");
            }
         }

         private void AddOptionalResourceGauge(AbstractResourceGauge gauge)
         {
            AddOptionalGauge(gauge, gauge.GetResource() != null);
         }

         private void AddOptionalGauge(AbstractResourceGauge gauge, bool enabled)
         {
            if (enabled)
            {
               this.gauges.Add(gauge.GetWindowId(), gauge);
            }
         }

         // called about 50 times per second
         public void Update()
         {
            // check camera not that often
            if (updateCycle % 10 == 0) CheckCamera();
            //
            // try to spread updates
            if ( updateCycle % 5 == 0 ) biomeInspecteur.Update();
            if ( updateCycle % 5 == 0 ) resourceInspecteur.Update();
            if ( updateCycle % 5 == 1 ) engineInspecteur.Update();
            if ( updateCycle % 5 == 2 ) sensorInspecteur.Update();
            if ( updateCycle % 5 == 3 ) vesselInspecteur.Update();
            if ( updateCycle % 5 == 4 ) velocityInspecteur.Update();
            //
            updateCycle++;
         }

         public void ResetInspecteurs()
         {
            resourceInspecteur.Reset();
            engineInspecteur.Reset();
            sensorInspecteur.Reset();
            vesselInspecteur.Reset();
            velocityInspecteur.Reset();
            biomeInspecteur.Reset();
         }

         private bool IsEnabledInCamera(CameraManager.CameraMode camMode, bool isEva=false)
         {
            Configuration config = NanoGauges.configuration;
            switch (camMode)
            {
               case CameraManager.CameraMode.External:
               case CameraManager.CameraMode.Flight:
                  if(!isEva) return config.IsGaugesInFlightEnabled();
                  return config.IsGaugesInEvaEnabled();
               case CameraManager.CameraMode.IVA:
                  return config.IsGaugesInIvaEnabled();
               case CameraManager.CameraMode.Map:
                  return config.IsGaugesInMapEnabled();
            }
            return true;
         }

         private void CheckCamera(bool force = false)
         {
            CameraManager.CameraMode camMode = CameraManager.Instance.currentCameraMode;
            bool evaCam = IsEvaCamera();
            //

            if (force || camMode != this.currentCamMode || (camMode == CameraManager.CameraMode.Flight && evaCam!=this.isEvaCam))
            {
               if(force)
               {
                  if(camMode == CameraManager.CameraMode.Flight && evaCam)
                  {
                     bool visible = NanoGauges.configuration.IsGaugesInEvaEnabled();
                     Log.Detail("forced test if gauges visible in EVA, gauges visible: " + visible);
                     SetEnabledGaugesVisible(visible);
                  }
                  else
                  {
                     bool visible = IsEnabledInCamera(camMode);
                     Log.Detail("forced test if gauges visible in non EVA, gauges visible: " + visible);
                     SetEnabledGaugesVisible(visible);
                  }
               }
               else if(camMode == CameraManager.CameraMode.Flight && evaCam)
               {
                  // switch to EVA in flight
                  bool visible = NanoGauges.configuration.IsGaugesInEvaEnabled();
                  Log.Detail("camera changed to EVA, gauges visible: " + visible);
                  SetEnabledGaugesVisible(visible);
               }
               else if (camMode == CameraManager.CameraMode.Flight && !evaCam)
               {
                  // switch from EVA in flight
                  bool visible = IsEnabledInCamera(camMode);
                  Log.Detail("camera changed to non EVA, gauges visible: " + visible);
                  SetEnabledGaugesVisible(visible);
               }
               else if(camMode != this.currentCamMode)
               {
                  // camer changed
                  bool visible = IsEnabledInCamera(camMode);
                  Log.Detail("camera changed to " + camMode + ", gauges visible: " + visible);
                  SetEnabledGaugesVisible(visible);
               }
               this.isEvaCam = evaCam;
               this.currentCamMode = camMode;
            }
         }

         public void SetEnabledInCamera(CameraManager.CameraMode camMode, bool enabled)
         {
            Configuration config = NanoGauges.configuration;
            switch (camMode)
            {
               case CameraManager.CameraMode.External:
               case CameraManager.CameraMode.Flight:
                  if (config.IsGaugesInFlightEnabled() != enabled) 
                  {
                     config.SetGaugesInFlightEnabled(enabled);
                     CheckCamera(true);
                  }
                  break;
               case CameraManager.CameraMode.IVA:
                  if (config.IsGaugesInIvaEnabled() != enabled)
                  {
                     config.SetGaugesInIvaEnabled(enabled);
                     CheckCamera(true);
                  }
                  break;
               case CameraManager.CameraMode.Map:
                  if (config.IsGaugesInMapEnabled() != enabled)
                  {
                     config.SetGaugesInMapEnabled(enabled);
                     CheckCamera(true);
                  }
                  break;
            }
         }

         public void SetEnabledInEva( bool enabled )
         {
            Configuration config = NanoGauges.configuration;
            if (config.IsGaugesInEvaEnabled() != enabled)
            {
               config.SetGaugesInEvaEnabled(enabled);
               CheckCamera(true);
            }

         }


         public bool IsGaugeEnabled(int id)
         {
            return NanoGauges.configuration.IsGaugeEnabled(id);
         }

         public void AutoLayout()
         {
            Log.Info("autolayout of gauges on screen");

            int LAYOUT_CELL_X = NanoGauges.configuration.verticalGaugeWidth + Gauges.LAYOUT_GAP;
            int LAYOUT_CELL_Y = NanoGauges.configuration.verticalGaugeHeight + Gauges.LAYOUT_GAP;
            int LAYOUT_RANGE_X = 3 * LAYOUT_CELL_X / 2;
            int LAYOUT_RANGE_Y = 3 * LAYOUT_CELL_Y / 2;

            List<AbstractGauge> leftToRight = new List<AbstractGauge>();
            foreach(AbstractGauge gauge in gauges.Values)
            {
               if(gauge.IsVisible())
               {
                  leftToRight.Add(gauge);
               }
            }
            
            leftToRight.Sort(
               delegate(AbstractGauge left, AbstractGauge right)
               { 
                  if( left.GetY().In(right.GetY(),right.GetY()+right.GetHeight())
                     || right.GetY().In(left.GetY(),left.GetY()+left.GetHeight()) )
                  {
                     return (left.GetX().CompareTo(right.GetX())); 
                  }
                  return (left.GetY().CompareTo(right.GetY())); 
               }
            );

            int x0 = -1;
            int x = -1;
            int y = -1;

            Log.Trace("starting autolayout");
            foreach (AbstractGauge gauge in leftToRight)
            {
               Log.Detail("autolayout for gauge " + gauge.GetWindowId());
               Log.Trace("LAYOUT " + gauge);
               if (x < 0)
               {
                  Log.Trace(" FIRST NEW LINE "+gauge);
                  x = gauge.GetX();
                  y = gauge.GetY();
                  x0 = x;
               }
               else
               {
                  // next line?
                  if (gauge.GetY() > y + gauge.GetHeight())
                  {
                     Log.Trace(" NEXT LINE " + gauge);
                     // next line
                     x = x0;
                     // independent column?
                     if (!gauge.GetX().In(x0 - LAYOUT_RANGE_X, x0 + 1 * LAYOUT_RANGE_X))
                     {
                        Log.Trace(" INDEPENDENT COLUMN " + gauge);
                        // yes, indepedent column
                        x0 = x = gauge.GetX();
                     }
                     //
                     // independent line?
                     if (gauge.GetY() > y + 1 * LAYOUT_RANGE_Y)
                     {
                        Log.Trace(" INDEPENDENT LINE " + gauge);
                        // yes
                        y = gauge.GetY();
                     }
                     else
                     {
                        Log.Trace(" NEW LINE " + gauge);
                        // no
                        y += LAYOUT_CELL_Y;
                     }
                  }
                  else
                  {
                     // line continues
                     // independent column?
                     if (gauge.GetX().In(x, x + 1 * LAYOUT_RANGE_X))
                     {
                        Log.Trace(" CONT LINE " + gauge);
                        // no
                        x += LAYOUT_CELL_X;
                     }
                     else
                     {
                        Log.Trace(" CONT LINE GAP " + gauge);
                        // yes, indepedent column
                        x = gauge.GetX();
                        y = gauge.GetY();
                     }
                     // overflow?
                     if (x+gauge.GetWidth() > Screen.width)
                     {
                        Log.Trace(" OVERFLOW " + gauge);
                        x = x0;
                        y -= LAYOUT_CELL_Y;
                     }
                  }
                  NanoGauges.configuration.SetWindowPosition(gauge, x, y);
                  gauge.SetPosition(x, y);
               }
            }
            Log.Trace("autolayout finished");

         }

         public void SetGaugeEnabled(AbstractGauge gauge, bool enabled)
         {
            SetGaugeEnabled(gauge.GetWindowId(), enabled);
         }


         public void SetGaugeEnabled(int id, bool enabled)
         {
            try
            {
               AbstractGauge gauge = gauges[id];
               if (enabled != gauge.IsVisible())
               {
                  gauge.SetVisible(enabled && IsEnabledInCamera(this.currentCamMode, IsEvaCamera()));
               }
               NanoGauges.configuration.SetGaugeEnabled(id, enabled);
            }
            catch(KeyNotFoundException)
            {
               Log.Warning("cannot enbable gauge; id "+id+" not found");
            }
         }


         public void ShowCloseButtons(bool enabled)
         {
            foreach (AbstractGauge gauge in gauges.Values)
            {
               gauge.EnableCloseButton(enabled);
            }
         }

         public bool ContainsId(int windowId)
         {
            return gauges.ContainsKey(windowId);
         }

         public HashSet<AbstractGauge> GetNeighBours(AbstractGauge gauge)
         {
            int LAYOUT_CELL_X = NanoGauges.configuration.verticalGaugeWidth + Gauges.LAYOUT_GAP;
            int LAYOUT_CELL_Y = NanoGauges.configuration.verticalGaugeHeight + Gauges.LAYOUT_GAP;
            int LAYOUT_RANGE_X = 3 * LAYOUT_CELL_X / 2;
            int LAYOUT_RANGE_Y = 3 * LAYOUT_CELL_Y / 2;

            HashSet<AbstractGauge> result = new HashSet<AbstractGauge>();
            int x0 = gauge.GetX();
            int y0 = gauge.GetY();
            foreach (AbstractGauge g in gauges.Values)
            {
               if(gauge.GetWindowId()!=g.GetWindowId())
               {
                  int x = g.GetX();
                  int y = g.GetY();
                  if (x.In(x0 - LAYOUT_RANGE_X, x0 + LAYOUT_RANGE_X) && y.In(y0 - LAYOUT_RANGE_Y, y0 + LAYOUT_RANGE_Y))
                  {
                     result.Add(g);
                  }
               }
            }
            return result;
         }


         public HashSet<AbstractGauge> GetCluster(AbstractGauge gauge)
         {
            HashSet<AbstractGauge> result = new HashSet<AbstractGauge>();
            GetCluster(result,gauge);
            return result;
         }

         private void GetCluster(HashSet<AbstractGauge> cluster, AbstractGauge gauge)
         {
            HashSet<AbstractGauge> neighbours = GetNeighBours(gauge);
            foreach (AbstractGauge neighbour in neighbours)
            {
               if(!cluster.Contains(neighbour))
               {
                  cluster.Add(neighbour);
                  GetCluster(cluster, neighbour);
               }
            }
         }

         public void ReflectGaugeSetChange()
         {
            Log.Detail("ReflectGaugeSetChange called");
            foreach (AbstractGauge gauge in gauges.Values)
            {
               int id = gauge.GetWindowId();
               Pair<int, int> position = NanoGauges.configuration.GetWindowPosition(id);
               gauge.SetPosition(position);
               gauge.SetVisible(NanoGauges.configuration.IsGaugeEnabled(id));
            }
         }


         public void UpdateGaugeScaling()
         {
            Log.Detail("UpdateGaugeScaling on gauges called");
            foreach (AbstractGauge gauge in gauges.Values)
            {
               gauge.OnGaugeScalingChanged();
            }
         }
      }
   }
}
