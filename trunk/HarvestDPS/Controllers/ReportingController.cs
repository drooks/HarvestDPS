using System;
using System.Linq;
using System.Web.Mvc;
using HarvestDPS.Extensions;
using HarvestDPS.Models;
using System.Collections.Generic;
using Harvest.Net.Models;


namespace HarvestDPS.Controllers
{
    [Authorize]
    /// <summary>
    /// The purpose of this controller is to dishup results for /reporting
    /// </summary>
    public class ReportingController : SiteController
    {
        // GET: Reporting view
        public ActionResult Index()
        {
            var model = new ReportingIndexModel
            {
                Clients = Client.ListClients() ?? new List<Client>(),
                Projects = Client.ListProjects() ?? new List<Project>(),
                //TimeEntries = DataStorage.Instance.PersistedContent.TimeEntries,
            };

            return View(model);
        }

        /// <summary>
        /// get a projects partial week view
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="date">if null, use today to fetch</param>
        /// <returns></returns>
        public ActionResult GetWeekDpsDayEntries(long projectId, DateTime? date = null)
        {
            if (date == null) date = DateTime.Now;

            var model = GetGetDayEntriesModel2(projectId, (DateTime)date);
            
            return PartialView("_GetDayEntries", model);
        }

        public GetDayEntriesModel GetGetDayEntriesModel2(long projectId, DateTime target)
        {
            var weekdates = new List<WeekDate>();

            var daysFromMonday = ((int)target.DayOfWeek + 6) % 7;
            var monday = target.AddDays(-daysFromMonday);

            var currentDay = monday;

            var dpsDayEntries = new List<DpsDayEntry>();

            for (int i = 0; i < 7; i++)
            {
                var dayEntries = DataStorage.Instance.PersistedContent.DpsDayEntries.Where(x => x.ProjectId == projectId && x.SpentAt.Date == currentDay.Date).ToList();

                weekdates.Add(new WeekDate
                {
                    DateTime = currentDay,
                    TotalHours = dayEntries.Sum(x=>x.Hours), //todo get hours logged to this day
                });

                if (currentDay.Date == target.Date)
                {
                    dpsDayEntries = dayEntries;
                }

                currentDay += new TimeSpan(1, 0, 0, 0);
            }

            return new GetDayEntriesModel
            {
                ProjectId = projectId,
                SelectedDateTime = target,
                ProjectCode = "",
                ProjectName = "",
                ProjectClient = "",
                DpsDayEntries = dpsDayEntries,
                WeekDates = weekdates
            };
        }
    }
}