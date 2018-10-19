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
    public class ProgressDiariesController : Controller
    {
        private HealthModel1 db = new HealthModel1();

        // GET: ProgressDiaries
        public ActionResult Index()
        {
            return View(db.ExerciseProgresses.Where(x => x.UserId == User.Identity.Name || User.Identity.Name == "admin@oneclickhealth.com").ToList());
        }

        // GET: ProgressDiaries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgressDiary progressDiary = db.ProgressDiaries.Find(id);
            if (progressDiary == null)
            {
                return HttpNotFound();
            }
            return View(progressDiary);
        }

        // GET: ProgressDiaries/Create
        public ActionResult Create()
        {
            ViewBag.ExerciseId = new SelectList(db.Exercises, "ExerciseId", "ExerciseName");
            return View();
        }

        // POST: ProgressDiaries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HealthModel1 h1)
        {
            if (ModelState.IsValid)
            {
                if (h1.ExerciseId[0] != h1.ExerciseId[1] && h1.ExerciseId[0] != h1.ExerciseId[2] && h1.ExerciseId[1] != h1.ExerciseId[2])
                {
                    ProgressDiary p1 = new ProgressDiary();
                    p1.EntryName = h1.EntryName;
                    db.ProgressDiaries.Add(p1);
                    db.SaveChanges();
                    int pid = p1.ProgressId;

                    ExerciseProgress e1 = new ExerciseProgress();
                    e1.ProgressId = pid;
                    e1.ExerciseId = h1.ExerciseId[0];
                    e1.UserId = User.Identity.Name;
                    e1.HoursSpent = h1.HoursSpent1;
                    e1.EntryDate = DateTime.Today;
                    db.ExerciseProgresses.Add(e1);
                    db.SaveChanges();

                    ExerciseProgress e2 = new ExerciseProgress();
                    e2.ProgressId = pid;
                    e2.ExerciseId = h1.ExerciseId[1];
                    e2.UserId = User.Identity.Name;
                    e2.HoursSpent = h1.HoursSpent2;
                    e2.EntryDate = DateTime.Today;
                    db.ExerciseProgresses.Add(e2);
                    db.SaveChanges();

                    ExerciseProgress e3 = new ExerciseProgress();
                    e3.ProgressId = pid;
                    e3.ExerciseId = h1.ExerciseId[2];
                    e3.UserId = User.Identity.Name;
                    e3.HoursSpent = h1.HoursSpent3;
                    e3.EntryDate = DateTime.Today;
                    db.ExerciseProgresses.Add(e3);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Create");
                }
            }

            return View(h1);
        }

        // GET: ProgressDiaries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgressDiary progressDiary = db.ProgressDiaries.Find(id);
            if (progressDiary == null)
            {
                return HttpNotFound();
            }
            return View(progressDiary);
        }

        // POST: ProgressDiaries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProgressId,EntryName")] ProgressDiary progressDiary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(progressDiary).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(progressDiary);
        }

        // GET: ProgressDiaries/Delete/5
        public ActionResult Delete(int? id ,int? id2)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgressDiary progress = db.ProgressDiaries.Find(id);
            if (progress == null)
            {
                return HttpNotFound();
            }
            return View(progress);
        }

        // POST: ProgressDiaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            while (true)
            {
                var user = (from i in db.ExerciseProgresses where i.ProgressId == id select i).FirstOrDefault();
                if (user == null)
                { break; }
                int newId = user.ExerciseId;
                ExerciseProgress ep = db.ExerciseProgresses.Find(id, newId);
                db.ExerciseProgresses.Remove(ep);
                db.SaveChanges();

            }
            ProgressDiary progressDiary = db.ProgressDiaries.Find(id);
            db.ProgressDiaries.Remove(progressDiary);
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
