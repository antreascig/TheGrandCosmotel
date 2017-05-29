using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebGames.Libs.Games.Games;

namespace WebGames.Controllers
{
    public class FilesController: Controller
    {
        [Authorize(Roles = "player")]
        public ActionResult GetWinnerInfo()
        {
            var UserId = User.Identity.GetUserId();
            if (Winner_Manager.IsUserWinner(UserId))
            {
                var file = File("winner_info.pdf", "application/pdf");
                return file;
            }

            return RedirectToAction("Index", "Home");
        }
    }
}