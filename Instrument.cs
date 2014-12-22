using System;
using UnityEngine;
using KSP.IO;

namespace Nereid
{
   namespace NanoGauges
   {
      class Instrument : AbstractInstrument
      {
         public Instrument()
            : base(19888)
         {
            SetPosition(200, 200);
         }

         protected override void OnWindow(int id)
         {
            Log.Test("OnWindow");
            GUILayout.Label("TEST");

            GUI.DragWindow();
         }
      }
   }
}
