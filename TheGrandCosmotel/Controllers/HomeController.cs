using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebGames.Models.ViewModels;
using Microsoft.AspNet.Identity;
using WebGames.Libs.Games.Games;
using WebGames.Libs.Games;

namespace WebGames.Controllers
{
    public class HomeController : Controller
    {     
        public HomeController()
        {
        }

        public HomeController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account"); 
            }
            ViewBag.Link = TempData["ViewBagLink"];

            if (User.IsInRole("sysadmin") || User.IsInRole("admin"))
            {

                return RedirectToAction("Index", "Dashboard");
            }
            var UserId = User.Identity.GetUserId();
            var model = new HomeIndexViewModel()
            {
                IsWinner = Winner_Manager.IsUserWinner(UserId),
                DailyData = GameDayScheduleManager.GetActiveDailyData(DateTime.UtcNow)
            };

            return View(model);
        }

        public ActionResult Info()
        {
            return View();
        }

        public ActionResult TnC()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

    }
}