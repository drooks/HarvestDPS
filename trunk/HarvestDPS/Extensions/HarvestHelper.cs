using System;
using System.Configuration;
using Harvest.Net;
using HarvestDPS.Models;
using System.Web;
using System.Web.Security;

namespace HarvestDPS.Extensions
{
    public static class HarvestHelper
    {
        // Assumes user has been logged in
        public static HarvestRestClient GetHarvestClient(HttpRequestBase request)
        {
            var encTicket = request.Cookies[FormsAuthentication.FormsCookieName].Value;

            var ticket = FormsAuthentication.Decrypt(encTicket);

            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginViewModel>(ticket.UserData);
            
            return GetClient(model.Email, model.Password);
        }

        public static HarvestRestClient GetClient(string email, string password)
        {
            var subDomain = ConfigurationManager.AppSettings.Get("HarvestSubdomain");

            return new HarvestRestClient(subDomain, email, password);
        }

        public static string UrlDateTime(DateTime datetime)
        {
            return string.Format("/time2/day/{0:yyyy}/{0:MM}/{0:dd}", datetime);
        }

        public static string UrlGetDaily(DateTime datetime, long userId)
        {
            return string.Format("/time2/GetDaily?date={0:yyyy}/{0:MM}/{0:dd}&userid={1}", datetime, userId);
        }
    }
}