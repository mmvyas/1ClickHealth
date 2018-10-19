using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OneClickHealth.Models;

namespace OneClickHealth.Controllers
{
    public class GraphController : Controller
    {
        private HealthModel1 db = new HealthModel1();
        public ActionResult GetData()
        {
            DateTime dat1 = new DateTime();
            DateTime dat2 = new DateTime();

            dat1 = (DateTime)Session["date2"];
            dat2 = (DateTime)Session["date3"];


            var data = db.ExerciseProgresses.Where(x => x.UserId == User.Identity.Name).ToList();
            DateTime [] entryDate = data.Where(x => x.EntryDate >= dat1 && x.EntryDate <= dat2).Select(x => x.EntryDate).Distinct().ToArray();
            int[] exercideId = data.Select(x => x.ExerciseId).Distinct().ToArray();
            String[] entries = new String[entryDate.Length];
            for (int i = 0; i < entryDate.Length; i++)
            {
                entries[i] = entryDate[i].ToLongDateString();          
            }
            int[] entry1 = new int[entryDate.Length];
            int[] entry2 = new int[entryDate.Length];
            int[] entry3 = new int[entryDate.Length];
            int[] cal = new int[exercideId.Length];
            int[] totalCalories = new int[entryDate.Length];
            for(int i =0;i<entryDate.Length;i++)
            {
                int totalC = data.Where(x => x.EntryDate == entryDate[i]).Sum(x => x.Exercise.CaloriesBurnt * x.HoursSpent);
                totalCalories[i] = totalC;
                for (int j = 0; j < exercideId.Length; j++)
                {
                    int excalories = data.Where(x => x.EntryDate == entryDate[i] && x.ExerciseId == exercideId[j]).Sum(x => x.Exercise.CaloriesBurnt * x.HoursSpent);
                    cal[j] = excalories;                    
                }
                entry1[i] = cal[0];
                entry2[i] = cal[1];
                entry3[i] = cal[2];
            }
            Chart g = new Chart();
            g.TotalCalories = totalCalories;
            g.EntryDate = entries;
            g.Entry1 = entry1;
            g.Entry2 = entry2;
            g.Entry3 = entry3;
        
            return Json(g, JsonRequestBehavior.AllowGet);
        }

        public class Chart
        {
            public int[] TotalCalories { get; set; }
            public String[] EntryDate { get; set; }
            public int[] Entry1 { get; set; }
            public int[] Entry2 { get; set; }
            public int[] Entry3 { get; set; }
            public int[] CaloriesBurnt { get; set; }
            public String[] ExerciseName { get; set; }
            public String InputDate { get; set; }

        }

        // GET: Graph
        public ActionResult Index()
        {
            //var exerciseProgresses = db.ExerciseProgresses.Include(e => e.Exercise).Include(e => e.ProgressDiary);
            //return View(exerciseProgresses.ToList());
            ViewBag.Date2 = new SelectList(db.ExerciseProgresses.Where(x => x.UserId == User.Identity.Name).Select(x => x.EntryDate).Distinct());
            ViewBag.Date1 = new SelectList(db.ExerciseProgresses.Where(x => x.UserId == User.Identity.Name).Select(x => x.EntryDate).Distinct());
            return View();
        }

        // GET: Graph/Details/5
        public ActionResult Details()
        {
            
            return View();
        }

        // GET: Graph/Create
        public ActionResult Create()
        {
            ViewBag.ExerciseId = new SelectList(db.Exercises, "ExerciseId", "ExerciseName");
            ViewBag.ProgressId = new SelectList(db.ProgressDiaries, "ProgressId", "EntryName");
            ViewBag.EntryDate = new SelectList(db.ExerciseProgresses.Where(x => x.UserId == User.Identity.Name).Select(x => x.EntryDate).Distinct());
            return View();
        }
        
        // POST: Graph/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HealthModel1 h1)
        {
            Session["date1"] = h1.EntryDate;
                return RedirectToAction("Details");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(HealthModel1 h1)
        {
            Session["date2"] = h1.Date1;
            Session["date3"] = h1.Date2;
            return RedirectToAction("Edit");
        }


        public ActionResult GetPieData()
        {
            DateTime dat = new DateTime();
            dat = (DateTime)Session["date1"];
            var data = db.ExerciseProgresses.Where(x => x.UserId == User.Identity.Name && x.EntryDate == dat).ToList();
            String[] exerciseName = data.Select(x => x.Exercise.ExerciseName).Distinct().ToArray();
            int[] caloriesBurnt = new int[exerciseName.Length];
            int[] exerciseID = data.Select(x => x.ExerciseId).Distinct().ToArray();
            for (int i = 0; i < exerciseID.Length; i++)
            {
                int sum = data.Where(x => x.ExerciseId == exerciseID[i]).Sum(x => x.Exercise.CaloriesBurnt * x.HoursSpent);
                caloriesBurnt[i] = sum;
            }

            Chart obj = new Chart();
            obj.InputDate = dat.ToString();
            obj.ExerciseName = exerciseName;
            obj.CaloriesBurnt = caloriesBurnt;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        // GET: Graph/Edit/5
        public ActionResult Edit()
        {
            return View();
        }

        // POST: Graph/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProgressId,ExerciseId,UserId,HoursSpent,EntryDate")] ExerciseProgress exerciseProgress)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exerciseProgress).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ExerciseId = new SelectList(db.Exercises, "ExerciseId", "ExerciseName", exerciseProgress.ExerciseId);
            ViewBag.ProgressId = new SelectList(db.ProgressDiaries, "ProgressId", "EntryName", exerciseProgress.ProgressId);
            return View(exerciseProgress);
        }

        // GET: Graph/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExerciseProgress exerciseProgress = db.ExerciseProgresses.Find(id);
            if (exerciseProgress == null)
            {
                return HttpNotFound();
            }
            return View(exerciseProgress);
        }

        // POST: Graph/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExerciseProgress exerciseProgress = db.ExerciseProgresses.Find(id);
            db.ExerciseProgresses.Remove(exerciseProgress);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
