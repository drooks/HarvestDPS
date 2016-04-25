using System;
using System.Linq;
using System.Web.Mvc;
using Harvest.Net;
using Harvest.Net.Models;
using HarvestDPS.Extensions;
using HarvestDPS.Models;
using System.Collections.Generic;

namespace HarvestDPS.Controllers
{
    [Authorize]
    public class TimeController : SiteController
    {
        // GET: Time -- Displays time entry form
        public ActionResult Index()
        {
            return Redirect(HarvestHelper.UrlDateTime(DateTime.Now));
        }

        //url: "time/day/{year}/{month}/{day}",
        public ActionResult Day(int year, int month, int day)
        {
            var date = new DateTime(year, month, day);

            //var user = Client.WhoAmI().User;

            //var model = new DayModel
            //{
            //    Daily = Client.Daily((short)date.DayOfYear, (short)date.Year, user.Id)
            //};

            return View(date);
        }

        public ActionResult GetAddEntry(DateTime date)
        {
            var user = Client.WhoAmI().User;

            var daily = Client.Daily((short)date.DayOfYear, (short)date.Year, user.Id);

            return PartialView("_AddEntry", daily);
        }

        public ActionResult GetWeek(DateTime date, long? projectId =null)
        {
            var user = Client.WhoAmI().User;

            var model = new DayModel
            {
                WeekDates = GetWeekDates(date),
                Daily = Client.Daily((short)date.DayOfYear, (short)date.Year, user.Id)
            };
            return PartialView("_Week", model);
        }

        private List<WeekDate> GetWeekDates(DateTime target)
        {
            var result = new List<WeekDate>();
            var daysFromMonday = ((int)target.DayOfWeek + 6) % 7;
            var monday = target.AddDays(-daysFromMonday);

            var user = Client.WhoAmI().User;
            List<DayEntry> dayEntries = (List<DayEntry>)Client.ListUserEntries(user.Id, monday, monday.AddDays(7));

            var currentDay = monday;
            for (int i = 0; i < 7; i++)
            {
                result.Add(new WeekDate
                {
                    DateTime = currentDay,
                    Selected = target == currentDay,
                    TotalHours = dayEntries.Where(x => x.SpentAt.Date == currentDay).Sum(x => x.Hours)
                });
                currentDay = currentDay.AddDays(1);
            }
            return result;
        }
    }
}
