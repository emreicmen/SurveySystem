using SurveySystem.Models;
using SurveySystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveySystem.Controllers
{
    public class AnswerController : BaseController
    {
        // GET: Answer
        public ActionResult Index()
        {
            var model = db.Answer.Where(m => m.UserCode == UsersCode).ToList();
            return View(model);
        }

        public ActionResult Create(string Code)
        {
            if (Code == null)
            {
                List<SelectListItem> personList = (from person in db.Person
                                                   where person.Code != UsersCode
                                                   select new SelectListItem
                                                   {
                                                       Text = person.NameSurname,
                                                       Value = person.Code.ToString()
                                                   }).ToList();
                ViewBag.Person = new SelectList(personList.OrderBy(m => m.Text), "Value", "Text");

                var questionModel = db.Question.ToList();
                return View(questionModel);
            }
            else
            {
                CalculateScore(Code);
                return RedirectToAction("Index");
            }
        }

        public void CalculateScore(string code)
        {
            double yes = 0, no = 0, result = 0;
            var answer = db.Answer.FirstOrDefault(m => m.PersonCode == code && m.UserCode == UsersCode);
            var answerLine = db.AnswerLine.Where(m => m.AnswerId == answer.Id).ToList();

            foreach (var item in answerLine)
            {
                if (item.Answer == Constants.AnswerType.Yes)
                {
                    yes++;
                }
                else
                {
                    no++;
                }
            }
            result = (yes / (yes + no)) * 100;
            if (result >79)
            {
                answer.IsCompleted = true;
            }
            else
            {
                answer.IsCompleted = false;
            }
            answer.Score = result.ToString();
            db.SaveChanges();
        }

        public String SendData(AnswerModel answerModel)
        {
            int? currentMonth = DateTime.Now.Month;
            var model = db.Answer.FirstOrDefault(m => m.PersonCode == answerModel.Code && m.UserCode == UsersCode && m.CreateDate.Value.Month == currentMonth);
            if (model != null)
            {
                SaveAnswerLine(answerModel.Question, answerModel.Answer, model.Id);

            }
            else
            {
                Answer answer = new Answer();
                answer.PersonCode = answerModel.Code;
                answer.PersonName = answerModel.NameSurname;
                answer.UserCode = UsersCode;
                answer.CreateDate = DateTime.Now;
                answer.CreateBy = NameSurname;
                db.Answer.Add(answer);
                db.SaveChanges();
                SaveAnswerLine(answerModel.Question, answerModel.Answer, answer.Id);
            }

            return "True";
        }

        public void SaveAnswerLine(string question, string answer, int answerId)
        {
            var model = db.AnswerLine.FirstOrDefault(m => m.AnswerId == answerId && m.Question == question);
            if (model != null)
            {
                model.Answer = answer;
                db.SaveChanges();
            }
            else
            {
                AnswerLine answerLine = new AnswerLine();
                answerLine.AnswerId = answerId;
                answerLine.Answer = answer;
                answerLine.Question = question;

                db.AnswerLine.Add(answerLine);
                db.SaveChanges();
            }

        }
    }
}