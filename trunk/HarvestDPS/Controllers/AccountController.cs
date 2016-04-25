using System.Threading.Tasks;
using System.Web.Mvc;
using HarvestDPS.Extensions;
using HarvestDPS.Models;

namespace HarvestDPS.Controllers
{
    [Authorize]
    public class AccountController : SiteController
    {

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            var client = HarvestHelper.GetClient(model.Email, model.Password);

            var user = client.WhoAmI().User;

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");

                return View(model);
            }

            Response.Cookies.Add(Extensions.AuthHelper.GetAuthCookie(model));
            return RedirectToAction("Index", "Time");
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Extensions.AuthHelper.RemoveAuthCookie();
            return RedirectToAction("Index", "Time");
        }
    }
}