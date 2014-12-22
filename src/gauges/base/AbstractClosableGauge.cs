using System;
using System.Collections.Generic;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {
      public abstract class AbstractClosableGauge : AbstractGauge
      {
         private static readonly Texture2D BUTTON_CLOSE_SKIN = Utils.GetTexture("Nereid/NanoGauges/Resource/Close");
         private const int CLOSE_BUTTON_SIZE = 16;

         private static readonly GUIStyle BUTTON_CLOSE_STYLE = new GUIStyle(HighLogic.Skin.label);

         private volatile bool closeButtonVisible = false;
         private readonly Rect closeButtonPosition;

         static AbstractClosableGauge()
         {
            BUTTON_CLOSE_STYLE.border = new RectOffset(0, 0, 0, 0);
            BUTTON_CLOSE_STYLE.margin = new RectOffset(0, 0, 0, 0);
            BUTTON_CLOSE_STYLE.padding = new RectOffset(0, 0, 0, 0);
         }



         public AbstractClosableGauge(int id) 
            : base(id)
         {
            this.closeButtonPosition = new Rect(GetWidth()-CLOSE_BUTTON_SIZE, 0, CLOSE_BUTTON_SIZE, CLOSE_BUTTON_SIZE);
         }


         protected override void OnDecoration(int id)
         {
            // close button
            if (this.closeButtonVisible)
            {
               if (GUI.Button(closeButtonPosition, BUTTON_CLOSE_SKIN, BUTTON_CLOSE_STYLE))
               {
                  SetVisible(false);
                  NanoGauges.configuration.SetGaugeEnabled(GetWindowId(), false);
               }
            }
         }

         public override void EnableCloseButton(bool enabled)
         {
            this.closeButtonVisible = enabled;
         }

      }

   }
}
