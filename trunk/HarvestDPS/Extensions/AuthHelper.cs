using System;
using System.Web;
using HarvestDPS.Models;
using System.Web.Security;

namespace HarvestDPS.Extensions
{
    public static class AuthHelper
    {
        public static HttpCookie GetAuthCookie(this LoginViewModel model)
        {
            string userData = Newtonsoft.Json.JsonConvert.SerializeObject(model);

            var ticket = new FormsAuthenticationTicket(1,
              model.Email,
              DateTime.Now,
              DateTime.Now.AddMinutes(30),
              false,
              userData,
              FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
            string encTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie.
            //Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
            return new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
        }

        public static void RemoveAuthCookie() {
            FormsAuthentication.SignOut();
        }
    }
}