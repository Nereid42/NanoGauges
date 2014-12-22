using System;
using System.Collections.Generic;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {
      public abstract class AbstractGauge
      {
         //
         public const int HEIGHT = 100;
         public const int WIDTH = 42;
         //
         private readonly int id;
         private bool visible = false;
         protected Rect bounds = new Rect(0,0,WIDTH,HEIGHT);
         protected float x;
         protected float y;

         private readonly TooltipWindow tooltip;

         private HashSet<AbstractGauge> cluster = null;

         public AbstractGauge(int id) 
         {
            Log.Detail("creating gauge window for gauge id "+id);
            this.id = id;

            Log.Info("position for gauge id "+id+": "+NanoGauges.configuration.GetWindowPosition(id));
            SetPosition(NanoGauges.configuration.GetWindowPosition(id));

            this.tooltip = new TooltipWindow(this);
         }

         public int GetWindowId()
         {
            return id;
         }

         protected void DragWindow()
         {
            GUI.DragWindow();
            if(NanoGauges.configuration.snapinEnabled)
            {
               // window was dragged?
               if (this.x != bounds.x || this.y != bounds.y)
               {
                  NanoGauges.snapinManager.snap(this);
               }
            }
         }

         protected virtual void OnOpen()
         {
         }

         protected virtual void OnClose()
         {
         }

         public void OnDraw()
         {
            if (visible)
            {
               bounds = GUI.Window(id, bounds, OnWindowInternal, "", GUI.skin.label);
            }
            else
            {
               if (tooltip != null) tooltip.SetVisible(false);
            }
         }

         protected virtual void OnDecoration(int id)
         {
         }

         private void OnWindowInternal(int id)
         {
            OnWindow(id);
            OnDecoration(id);
            if (!NanoGauges.configuration.gaugePositionsLocked)
            {
               DragWindow();
               // window was dragged?
               if (this.x != bounds.x || this.y != bounds.y)
               {
                  if (Log.IsLogable(Log.LEVEL.DETAIL))
                  {
                     Log.Detail("window " + id + " was dragged to " + bounds.x + "/" + bounds.y);
                  }
                  NanoGauges.configuration.SetWindowPosition(this);
                  //
                  if (Input.GetKey(NanoGauges.configuration.GetKeyCodeForHotkey()))
                  {
                     DragClusterOfGauges((int)(bounds.x - this.x), (int)(bounds.y - this.y));
                  }
                  //
                  this.x = bounds.x;
                  this.y = bounds.y;
               }
            }
            //
            if (!Input.GetKey(NanoGauges.configuration.GetKeyCodeForHotkey()))
            {
               cluster = null;
            }

            DrawTooltip();
            DrawExactReadout();

            Event e = Event.current;
            if (e.type == EventType.MouseDown && e.button == 0)
            {
               e.Use();
            }
         }

         private void DragClusterOfGauges(int dx, int dy)
         {
            if(cluster==null)
            {
               cluster = NanoGauges.GetCluster(this);
            }
            foreach(AbstractGauge gauge in cluster)
            {
               if(gauge.GetWindowId()!=id)
               {
                  gauge.SetPosition(gauge.GetX() + dx, gauge.GetY() + dy);
                  NanoGauges.configuration.SetWindowPosition(gauge);
               }
            }
         }

         public abstract String GetName();
         public abstract String GetDescription();


         protected void DrawTooltip()
         {
            if (NanoGauges.configuration.tooltipsEnabled)
            {
               float x = Input.mousePosition.x - bounds.x;
               float y = (Screen.height - Input.mousePosition.y) - bounds.y;
               if(x>=0 && x<bounds.width && y>=0 && y<bounds.height)
               {
                  this.tooltip.SetVisible(true);
               }
               else
               {
                  this.tooltip.SetVisible(false);
               }
            }
         }

         protected void DrawExactReadout()
         {
            // not implemented
         }

         protected abstract void OnWindow(int id);


         public void SetVisible(bool visible, float x, float y)
         {
            this.x = bounds.x = x;
            this.y = bounds.y = y;
            SetVisible(visible);
         }


         public void SetVisible(bool visible)
         {
            if (!this.visible && visible) OnOpen();
            if (this.visible && !visible) OnClose();
            this.visible = visible;
            if(!visible)
            {
               if (tooltip != null) tooltip.SetVisible(false);
            }
         }

         public bool IsVisible()
         {
            return this.visible;
         }


         public int GetX()
         {
            return (int)bounds.xMin;
         }

         public int GetY()
         {
            return (int)bounds.yMin;
         }

         public int GetWidth()
         {
            return (int)bounds.width;
         }

         public int GetHeight()
         {
            return (int)bounds.height;
         }

         public void SetPosition(Pair<int, int> coords)
         {
            SetPosition(coords.first, coords.second);
         }

         public void SetPosition(int x, int y)
         {
            if (Log.IsLogable(Log.LEVEL.DETAIL))  Log.Detail("moving window " + id + " to " + x + "/" + y);
            bounds.Set(x, y, bounds.width, bounds.height);
            this.x = x;
            this.y = y;
         }

         // Status, Flags,...
         public abstract void On();
         public abstract void Off();
         public abstract bool IsOn();
         public abstract void InLimits();
         public abstract void OutOfLimits();
         public abstract bool IsInLimits();
         //
         public abstract void Reset();


         public abstract void EnableCloseButton(bool enabled);

         protected bool InBounds(Rect bounds, float x, float y)
         {
            return (x >= bounds.x && x < bounds.x + bounds.width && y >= bounds.y && y < bounds.y + bounds.height);
         }


         public void SetEnabled (bool enabled)
         {
            if(enabled)
            {
               On();
            }
            else
            {
               Off();
            }
         }

         public virtual bool IsEnabledByDefault()
         {
            return true;
         }

         public override int GetHashCode()
         {
            return GetName().GetHashCode();
         }

      }

   }
}
