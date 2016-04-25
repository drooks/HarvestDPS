using System.Linq;
using System.Collections.Generic;

namespace HarvestDPS.Models
{
    public class PersistedContent
    {
        public PersistedContent()
        {
            DpsDayEntries = new List<DpsDayEntry>();

        }
        public List<DpsDayEntry> DpsDayEntries { get; set; }

        public void AddOrUpdate(DpsDayEntry dpsDayEntry) {

            var entity = DpsDayEntries.SingleOrDefault(x => x.Id == dpsDayEntry.Id);
            if (entity == null)
            {
                DpsDayEntries.Add(dpsDayEntry);
                return;
            }

            entity = dpsDayEntry;
        }
    }
}