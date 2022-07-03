using SurveySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveySystem.Utils
{
    public class BaseController : Controller
    {
        public SurveyEntities db = new SurveyEntities();
        public string UsersCode { get; set; }
        public string NameSurname { get; set; }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["Code"] == null)
            {
                filterContext.Result = new RedirectResult("/Login/SignIn");
            }
            else
            {
                UsersCode = Session["Code"].ToString();
                NameSurname = Session["NameSurname"].ToString();
            }
            base.OnActionExecuting(filterContext);
        }
    }
}