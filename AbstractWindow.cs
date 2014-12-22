using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {
      public abstract class AbstractWindow
      {
         public delegate void OnWindowClose();

         public static readonly int AUTO_HEIGHT = -1;
         private static readonly int DEFAULT_WIDTH = 400;
         //
         private readonly int id;
         private string title;
         private bool visible = false;
         protected Rect bounds = new Rect();

         // called on WindowClose
         OnWindowClose onWindowClose;

         public AbstractWindow(int id, string title) 
         {
            Log.Detail("creating window "+id+" with title '"+title+"'");
            this.id = id;
            this.title = title;

            try
            {
               RenderingManager.AddToPostDrawQueue(0, OnDraw);
            }
            catch
            {
               Log.Error("error creating window "+id+" "+title);
            }
         }

         public int GetWindowId()
         {
            return id;
         }

         protected void DragWindow()
         {
               GUI.DragWindow();
         }

         protected virtual void OnOpen()
         {
         }

         protected virtual void OnClose()
         {
         }

         private void OnDraw()
         {
            if (visible)
            {
               if (GetInitialHeight() == AUTO_HEIGHT)
               {
                  bounds = GUILayout.Window(id, bounds, OnWindowInternal, title, HighLogic.Skin.window, GUILayout.Width(GetInitialWidth()));
               }
               else
               {
                  bounds = GUILayout.Window(id, bounds, OnWindowInternal, title, HighLogic.Skin.window, GUILayout.Width(GetInitialWidth()), GUILayout.Height(GetInitialHeight()));
               }
            }
         }

         private void OnWindowInternal(int id)
         {
            try
            {
               OnWindow(id);
            }
            catch
            {
               Log.Error("Exception in window "+GetType()+", id:"+id);
            }
            Event e = Event.current;
            if (e.type == EventType.MouseDown && e.button == 0)
            {
               e.Use();
            }
         }

         protected abstract void OnWindow(int id);


         public void SetVisible(bool visible, float x, float y)
         {
            bounds.x = x;
            bounds.y = y;
            SetVisible(visible);
         }

         public void CallOnWindowClose(OnWindowClose method)
         {
            onWindowClose = method;
         }

         public virtual void SetVisible(bool visible)
         {
            if (!this.visible && visible) OnOpen();
            if (this.visible && !visible) OnClose();
            if (this.visible && !visible && onWindowClose != null) onWindowClose();
            this.visible = visible;
         }

         public bool IsVisible()
         {
            return this.visible;
         }

         // TODO: make protected and add GetWidth(), GetHeight()
         public virtual int GetInitialWidth()
         {
            return DEFAULT_WIDTH;
         }

         // TODO: make protected and add GetWidth(), GetHeight()
         protected virtual int GetInitialHeight()
         {
            return AUTO_HEIGHT;
         }

         public int GetX()
         {
            return (int)bounds.xMin;
         }

         public int GetY()
         {
            return (int)bounds.yMin;
         }


         public void SetPosition(Pair<int, int> coords)
         {
            SetPosition(coords.first, coords.second);
         }

         public void SetPosition(int x, int y)
         {
            if (Log.IsLogable(Log.LEVEL.TRACE)) Log.Trace("moving window " + id + " to " + x + "/" + y);
            bounds.Set(x, y, bounds.width, bounds.height);
         }

         public void SetSize(int w, int h)
         {
            if(Log.IsLogable(Log.LEVEL.TRACE)) Log.Trace("window dimension for " + id + " changed to  " + w + "/" + h);
            bounds.Set(bounds.x, bounds.y, w, h);
         }


         public void SetTitle(String title)
         {
            this.title = title;
         }

         public void CenterWindow()
         {
            if (!visible) return;
            int x = (Screen.width - (int)bounds.width) / 2;
            int y = (Screen.height - (int)bounds.height) / 2;
            SetPosition(x, y);
         }
      }

   }
}
