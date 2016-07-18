using CryptoSystemDissertation.Common;
using CryptoSystemDissertation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CryptoSystemDissertation.Controllers
{
    public class ChatboardController : Controller
    {
        // GET: Chatboard
        [CryptoAuthorize]
        [ExitHttpsIfNotRequiredAttribute]
        public ActionResult Index()
        {
            using (CryptoDbContext db = new CryptoDbContext())
            {
                var crtUser = SessionManager.ReturnSessionObject("User") as UserAccount;
                List<UserAccount> registeredUsers = new List<UserAccount>();
                registeredUsers = db.UserAccount.Where(usr => usr.UserID != crtUser.UserID).ToList();
                return View(registeredUsers);
            }
        }

        [ExitHttpsIfNotRequiredAttribute]
        public ActionResult ChatOn(int userId)
        {
            using (CryptoDbContext db = new CryptoDbContext())
            {
                var user = db.UserAccount.Where(u => u.UserID == userId).FirstOrDefault();
                ViewBag.User = user;
                return View();
            }
           
        }
    }
}