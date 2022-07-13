
using System;
using WORK3.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WORK3.ViewModels;

namespace WORK3.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Register registerDetails)
        {
            if (ModelState.IsValid)
            {
                using (var databaseContext = new pavanEntities2())
                {

                    Form rg = new Form();


                    rg.FirstName = registerDetails.FirstName;
                    rg.LastName = registerDetails.LastName;
                    rg.PhNum = registerDetails.PhNum;
                    rg.Email = registerDetails.Email;
                    rg.Password = registerDetails.Password;


                    databaseContext.Forms.Add(rg);
                    databaseContext.SaveChanges();
                }

                ViewBag.Message = "User Details Saved";
                return View("Register");
            }
            else
            {

                return View("Register", registerDetails);
            }
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login model)
        {
            if (ModelState.IsValid)
            {

                var isValidUser = IsValidUser(model);

                if (isValidUser != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Failure", "Wrong Username and password combination !");
                    return View();
                }
            }
            else
            {
                return View(model);
            }
        }

        public Form IsValidUser(Login model)
        {
            using (var dataContext = new pavanEntities2())
            {
                Form user = dataContext.Forms.Where(query => query.Email.Equals(model.Email) && query.Password.Equals(model.Password)).SingleOrDefault();
                if (user == null)
                    return null;
                else
                    return user;
            }
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index");
        }




    }
}
    
