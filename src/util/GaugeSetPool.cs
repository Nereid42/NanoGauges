using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Nereid
{
   namespace NanoGauges
   {
      class GaugeSetPool : IEnumerable<GaugeSet>
      {
         public static readonly GaugeSetPool instance = new GaugeSetPool();

         private readonly Dictionary<GaugeSet.ID, GaugeSet> sets = new Dictionary<GaugeSet.ID, GaugeSet>();

         private readonly GaugeSet clipboard = new GaugeSet(GaugeSet.ID.CLIPBOARD);

         private GaugeSetPool()
         {
            Add(new GaugeSet(GaugeSet.ID.STANDARD));
            Add(new GaugeSet(GaugeSet.ID.LAND));
            Add(new GaugeSet(GaugeSet.ID.DOCK));
            Add(new GaugeSet(GaugeSet.ID.FLIGHT));
            Add(new GaugeSet(GaugeSet.ID.ORBIT));
            Add(new GaugeSet(GaugeSet.ID.LAUNCH));
            Add(new GaugeSet(GaugeSet.ID.SET1));
            Add(new GaugeSet(GaugeSet.ID.SET2));
            Add(new GaugeSet(GaugeSet.ID.SET3));
         }

         private void Add(GaugeSet set)
         {
            sets.Add(set.GetId(), set);
         }

         public GaugeSet GetGaugeSet(GaugeSet.ID id)
         {
            return sets[id];
         }

         public int Count()
         {
            return sets.Count;
         }

         public GaugeSet GetClipboard()
         {
            return clipboard;
         }

         public void CopyToClipboard(GaugeSet set)
         {
            clipboard.copyFrom(set);
         }

         public void CopyToClipboard(GaugeSet.ID id)
         {
            GaugeSet set = GetGaugeSet(id);
            CopyToClipboard(set);
         }

         public System.Collections.IEnumerator GetEnumerator()
         {
            return sets.Values.GetEnumerator();
         }

         IEnumerator<GaugeSet> IEnumerable<GaugeSet>.GetEnumerator()
         {
            return sets.Values.GetEnumerator();
         }
      }
   }
}
