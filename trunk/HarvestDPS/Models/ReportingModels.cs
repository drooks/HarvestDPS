using System;
using System.Collections.Generic;
using Harvest.Net.Models;

namespace HarvestDPS.Models
{
    public class ReportingIndexModel
    {
        public IList<Client> Clients { get; set; }
        public IList<Project> Projects { get; set; }
        //public IList<TimeEntry> TimeEntries { get; set; }
    }

    public class GetDayEntriesModel {
        public long ProjectId { get; set; }
        public DateTime SelectedDateTime { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string ProjectClient { get; set; }
        public List<WeekDate> WeekDates { get; set; }
        public IList<DpsDayEntry> DpsDayEntries { get; set; }
    }

}