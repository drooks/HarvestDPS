using System;
using Harvest.Net.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HarvestDPS.Models
{
    public class DayModel2
    {
        public long UserId { get; set; }
        public DateTime SelectedDay { get; set; }
        public List<WeekDate> WeekDates { get; set; }
    }


    public class DayModel
    {
        public List<WeekDate> WeekDates { get; set; }
        public Daily Daily { get; set; }
    }

    public class WeekDate
    {
        public DateTime DateTime { get; set; }

        public decimal TotalHours { get; set; }
    }

    public class DpsDayEntry : DayEntry
    {
        public DpsDayEntry() { }
        public DpsDayEntry(DayEntry dayEntry)
        {
            Id = dayEntry.Id;
            UpdatedAt = dayEntry.UpdatedAt;
            CreatedAt = dayEntry.CreatedAt;
            Notes = dayEntry.Notes;
            SpentAt = dayEntry.SpentAt;
            Hours = dayEntry.Hours;
            UserId = dayEntry.UserId;
            ProjectId = dayEntry.ProjectId;
            TaskId = dayEntry.TaskId;
            AdjustmentRecord = dayEntry.AdjustmentRecord;
            IsBilled = dayEntry.IsBilled;
            IsClosed = dayEntry.IsClosed;
            Task = dayEntry.Task;
            Project = dayEntry.Project;
        }

        public string Who { get; set; }

        [Required,Display(Name ="Today's tasks")]
        public string WhatsNew { get; set; }

        [Required, Display(Name = "Tomorrow's tasks")]
        public string WhatsNext { get; set; }

        [Required, Display(Name = "Challenges")]

        public string Blockers { get; set; }
    }
}