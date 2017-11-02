using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nereid
{
   namespace NanoGauges
   {
      public abstract class AbstractGauge
      {
         private readonly int id;
         private bool visible = false;
         protected Rect bounds;
         protected float x;
         protected float y;

         private readonly TooltipWindow tooltip;

         private HashSet<AbstractGauge> cluster = null;

         public AbstractGauge(int id) 
         {
            Log.Detail("creating gauge window for gauge id "+id+" of size "+GetWidth()+"x"+GetHeight());
            this.id = id;
            this.bounds = new Rect(0, 0, GetWidth(), GetHeight());

            Log.Detail("position for gauge id "+id+": "+NanoGauges.configuration.GetWindowPosition(id));
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

         protected virtual void OnTooltip()
         {
         }

         // adjust values like internal needles, etc
         // called once per Update before the gauge is drawn
         protected virtual void AjustValues()
         {
            // overwritten by subclasses
         }


         public void OnDraw()
         {
            if (visible)
            {
               AjustValues();
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
                  if (NanoGauges.hotkeyManager.GetKey(HotkeyManager.HOTKEY_CLOSE_AND_DRAG))
                  {
                     DragClusterOfGauges((int)(bounds.x - this.x), (int)(bounds.y - this.y));
                  }
                  //
                  this.x = bounds.x;
                  this.y = bounds.y;
               }
            }
            //
            if (!NanoGauges.hotkeyManager.GetKey(HotkeyManager.HOTKEY_CLOSE_AND_DRAG))
            {
               cluster = null;

               // don't draw tooltips or exact readout (zoom) if hotkey is pressed!
               DrawTooltip();
               DrawExactReadout();
            }
            else
            {
               HideTooltip();
            }


            Event e = Event.current;
            if (e.type == EventType.MouseDown && e.button == 0)
            {
               e.Use();
            }
         }

         // draw a rectagle at (x,y) of with w, height h of color c:
         private readonly Texture2D RECT_TEXTURE = new Texture2D(1, 1);
         private Rect RECT_BOUNDS = new Rect();
         protected void DrawRectagle(float x, float y, float w, float h, Color c)
         {
            RECT_TEXTURE.SetPixel(0, 0, c);
            RECT_TEXTURE.Apply();
            RECT_BOUNDS.x = x;
            RECT_BOUNDS.y = y;
            RECT_BOUNDS.width = w;
            RECT_BOUNDS.height = h;
            GUI.DrawTexture(RECT_BOUNDS, RECT_TEXTURE, ScaleMode.StretchToFill);
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
            float x = Input.mousePosition.x - bounds.x;
            float y = (Screen.height - Input.mousePosition.y) - bounds.y;
            if(x>=0 && x<bounds.width && y>=0 && y<bounds.height)
            {
               OnTooltip();
               this.tooltip.SetVisible(NanoGauges.configuration.tooltipsEnabled);
            }
            else
            {
               HideTooltip();
            }
         }

         protected void HideTooltip()
         {
            this.tooltip.SetVisible(false);
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

         public abstract int GetWidth();

         public abstract int GetHeight();

         public abstract void OnGaugeScalingChanged();

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
         public abstract void NotInLimits();
         public abstract bool IsInLimits();
         //
         public abstract void Reset();
         //
         // derived
         public bool IsOff() { return !IsOn(); }
         public bool IsNotInLimits() { return !IsInLimits(); }


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
