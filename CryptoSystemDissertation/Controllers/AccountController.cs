using CryptoSystemDissertation.Common;
using CryptoSystemDissertation.Models;
using System.Linq;
using System.Web.Mvc;

namespace CryptoSystemDissertation.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [CryptoAuthorize]
        public ActionResult Index()
        {            
            using (CryptoDbContext db = new CryptoDbContext())
            {
                return View(db.UserAccount.ToList());
            }
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        //[RequireHttps]
        //[ExitHttpsIfNotRequiredAttribute]
        public ActionResult Register(UserAccount account)
        {
            if (ModelState.IsValid)
            {
                using (CryptoDbContext db = new CryptoDbContext())
                {
                    var usr = db.UserAccount.Where(u => u.Username == account.Username).FirstOrDefault();
                    if(usr != null)
                    {
                        ViewBag.RegisterError = account.Username + " already exists, please choose a different username.";
                        ModelState.Clear();
                        return View();
                    }

                    db.UserAccount.Add(account);
                    db.SaveChanges();
                    ModelState.Clear();
                    ViewBag.Message = account.FirstName + " " + account.LastName + " successfully registered!";
                }
            }
            return View();
        }

        //Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        //[RequireHttps]
        //[ExitHttpsIfNotRequiredAttribute]
        public ActionResult Login(UserAccount user)
        {

            using (CryptoDbContext db = new CryptoDbContext())
            {
                var usr = db.UserAccount.Where(u => u.Username == user.Username && u.Password == user.Password).FirstOrDefault();
                if (usr != null)
                {
                    SessionManager.RegisterSession("User", usr);
                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    SessionManager.FreeSession("User");
                    ModelState.AddModelError("", "Username or password is wrong");
                }
            }
            return View();
        }

        public ActionResult LoggedIn()
        {
            if (SessionManager.CheckSession("User"))
            {
                return RedirectToAction("Index", "Chatboard");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult UnauthorizedUser()
        {
            return View();
        }
    }
}