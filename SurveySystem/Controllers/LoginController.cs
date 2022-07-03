using SurveySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveySystem.Controllers
{
    public class LoginController : Controller
    {
        SurveyEntities db = new SurveyEntities();
        // GET: Login
        public ActionResult SignIn(string Code,string Password)
        {
            if (Code == null)
            {
                return View();
            }
            else
            {
                var person = db.Person.FirstOrDefault(m => m.Code == Code && m.Password == Password);
                if (person != null)
                {
                    Session["Code"] = person.Code;
                    Session["NameSurname"] = person.NameSurname;
                    return RedirectToAction("Create", "Answer");
                }
                else
                {
                    ViewBag.Error = "Code or Password is not matching";
                    return View();
                }
            } 
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("SignIn", "Login");
        }
    }
}