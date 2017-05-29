using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebGames;
using WebGames.Helpers;
using WebGames.Models;

namespace WebGames.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View("Login");
        }

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if ((returnUrl ?? "") == "") returnUrl = "/Home/Index";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Require the user to have a confirmed email before they can log on.
            // var user = await UserManager.FindByNameAsync(model.Email);
            var user = UserManager.Find(model.UserName, model.Password);
            if (user != null)
            {
                if (!UserManager.IsInRole(user.Id, "player"))
                {
                    AddErrors( new IdentityResult("Ο λογαριασμός σου δεν αντιστοιχεί σε παίκτη."));
                    return View(model);
                }

                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Επιβεβαίωση Λογαριασμου - Επαναποστολή"); // "Confirm your account-Resend"

                    // Uncomment to debug locally  
                    //ViewBag.Link = callbackUrl;
                    ViewBag.errorMessage = @"Επιβεβαίωση λογαριασμού είναι αναγκαία πριν από την σύνδεση. 
                                             Μήνυμα επιβεβαίωσης έχει αποσταλεί στην ηλεκτρονική σας διεύθυνση.";
                    //ViewBag.errorMessage = "You must have a confirmed email to log on. "
                    //                     + "The confirmation token has been resent to your email account.";
                    return View("Error");
                }
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Μη έγκυρη προσπάθεια σύνδεσης.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {                   
                    UserName = model.UserName,
                    FullName = model.FullName,
                    Email = model.Email,
                    Shop = model.Shop,
                    MaritalStatus = model.MaritalStatus,
                    Hobby = model.Hobby,
                };

                bool EmailAllowed = true;
                // Check if email is correct
                //using (var db = ApplicationDbContext.Create())
                //{
                //    var emailToCheck = (user.Email ?? "").ToLower();
                //    EmailAllowed = (from email in db.Alowed_Emails where email.Email == emailToCheck select email).SingleOrDefault() != null;
                //}

                try
                {
                    var emailToCheck = (user.Email ?? "").ToLower();
                    MailAddress emailAddr = new MailAddress(emailToCheck);
                    EmailAllowed = emailAddr.Host == "cosmote.gr" || emailAddr.Host == "ote.gr";
                }
                catch
                {

                }
            

                if (EmailAllowed)
                {
                    IdentityResult result = await UserManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        result = await UserManager.AddToRoleAsync(user.Id, "player");
                        if (result.Succeeded)
                        {
                            //  Comment the following line to prevent log in until the user is confirmed.
                            //  await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                            string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Επιβεβαίωση Λογαριασμού");

                            ViewBag.Message = @"Ελέγξτε το email σας και επιβεβαιώστε τον λογαριασμό σας. Η επιβεβαίωση είναι αναγκαία για να συνδεθείτε!";

                            // For local debug only
                            ViewBag.Link = callbackUrl;

                            return View("Info");
                        }
                    }
                    // Fix errors
                    List<string> fixedErrors = ErrorMessageHelper.FixErrors(result.Errors);

                    AddErrors(new IdentityResult(fixedErrors));
                }
                else
                {
                    AddErrors(new IdentityResult("Παρακαλω χρησιμοποιήστε το εταιρικό σας Email"));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

       

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                var body = string.Format(Mailers.Recover(), callbackUrl, user.FullName);
                await UserManager.SendEmailAsync(user.Id, "Επαναφορά κωδικού", body);

                TempData["ViewBagLink"] = callbackUrl;
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            ViewBag.Link = TempData["ViewBagLink"];
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        //

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
               : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        private async Task<string> SendEmailConfirmationTokenAsync(string userID, string subject)
        {
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
            var callbackUrl = Url.Action("ConfirmEmail", "Account",
               new { userId = userID, code = code }, protocol: Request.Url.Scheme);
            var body = string.Format(Mailers.Register(), callbackUrl);
            await UserManager.SendEmailAsync(userID, subject, body);

            return callbackUrl;
        }
        #endregion
    }
}