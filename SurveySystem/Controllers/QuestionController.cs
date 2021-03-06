using SurveySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveySystem.Controllers
{
    public class QuestionController : Controller
    {
        // GET: Question
        SurveyEntities db = new SurveyEntities();
        public ActionResult Index()
        {
            var model = db.Question.ToList();
            return View(model);
        }

        public ActionResult Create(Question question)
        {
            if (question.QuestionLine != null)
            {
                question.CreateDate = DateTime.Now;
                question.CreateBy = "BySystem";
                db.Question.Add(question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return HttpNotFound();
            }
            var question = db.Question.Find(Id);
            return View(question);
        }

        [HttpPost]
        public ActionResult Edit(Question question)
        {
            db.Entry(question).State = System.Data.Entity.EntityState.Modified;
            db.Entry(question).Property(e => e.CreateBy).IsModified = false;
            db.Entry(question).Property(e => e.CreateDate).IsModified = false;

            question.ModifyBy = "BySystem";
            question.ModifyDate = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return HttpNotFound();
            }
            var question = db.Question.Find(Id);
            db.Question.Remove(question);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}