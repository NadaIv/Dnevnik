using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dnevnik.DAL;
using Dnevnik.Models;

namespace Dnevnik.Controllers
{
    public class PredmetiController : Controller
    {
        private SchoolContext db = new SchoolContext();

		// GET: Predmeti
		//public ActionResult Index()
		//{
		//    var predmetis = db.Predmetis.Include(p => p.Odeljenja);
		//    return View(predmetis.ToList());
		//}

		//public ActionResult Index()
		//{
		//	var predmetis = db.Predmetis;
		//	var sql = predmetis.ToString();
		//	return View(predmetis.ToList());
		//}

		public ActionResult Index(int? SelectedOdeljenja)
		{
			var odeljenjas = db.Odeljenjas.OrderBy(q => q.Ime).ToList();
			ViewBag.SelectedOdeljenja = new SelectList(odeljenjas, "OdeljenjeID", "Ime", SelectedOdeljenja);
			int odeljenjeID = SelectedOdeljenja.GetValueOrDefault();

			IQueryable<Predmeti> predmetis = db.Predmetis
				.Where(c => !SelectedOdeljenja.HasValue || c.OdeljenjeID == odeljenjeID)
				.OrderBy(d => d.PredmetiID)
				.Include(d => d.Odeljenja);
			//var sql = predmetis.ToString();
			return View(predmetis.ToList());
		}

		public ActionResult UpdatePredmetiCenas()
		{
			return View();
		}

		[HttpPost]
		public ActionResult UpdatePredmetiCenas(int? multiplier)
		{
			if (multiplier != null)
			{
				ViewBag.RowsAffected = db.Database.ExecuteSqlCommand("UPDATE Predmeti SET Cena = Cena * {0}", multiplier);
			}
			return View();
		}

		// GET: Predmeti/Details/5
		public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Predmeti predmeti = db.Predmetis.Find(id);
            if (predmeti == null)
            {
                return HttpNotFound();
            }
            return View(predmeti);
        }


		public ActionResult Create()
		{
			PopulateDepartmentsDropDownList();
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "PredmetiID,NazivPredmeta,Cena,OdeljenjeID")]Predmeti predmeti)
		{
			try
			{
				if (ModelState.IsValid)
				{
					db.Predmetis.Add(predmeti);
					db.SaveChanges();
					return RedirectToAction("Index");
				}
			}
			catch (RetryLimitExceededException /* dex */)
			{
				//Log the error (uncomment dex variable name and add a line here to write a log.)
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
			}
			PopulateDepartmentsDropDownList(predmeti.PredmetiID);
			return View(predmeti);
		}

		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Predmeti predmeti = db.Predmetis.Find(id);
			if (predmeti == null)
			{
				return HttpNotFound();
			}
			PopulateDepartmentsDropDownList(predmeti.PredmetiID);
			return View(predmeti);
		}

		[HttpPost, ActionName("Edit")]
		[ValidateAntiForgeryToken]
		public ActionResult EditPost(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var predmetiToUpdate = db.Predmetis.Find(id);
			if (TryUpdateModel(predmetiToUpdate, "",
			   new string[] { "NazivPredmeta", "Cena", "OdeljenjeID" }))
			{
				try
				{
					db.SaveChanges();

					return RedirectToAction("Index");
				}
				catch (RetryLimitExceededException /* dex */)
				{
					//Log the error (uncomment dex variable name and add a line here to write a log.
					ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
				}
			}
			PopulateDepartmentsDropDownList(predmetiToUpdate.OdeljenjeID);
			return View(predmetiToUpdate);
		}

		private void PopulateDepartmentsDropDownList(object selectedOdeljenje = null)
		{
			var odeljenjaQuery = from d in db.Odeljenjas
								   orderby d.Ime
								   select d;
			ViewBag.OdeljenjeID = new SelectList(odeljenjaQuery, "OdeljenjeID", "Ime", selectedOdeljenje);
		}


		// GET: Predmeti/Create
		//public ActionResult Create()
		//{
		//    ViewBag.OdeljenjeID = new SelectList(db.Odeljenjas, "OdeljenjeID", "Ime");
		//    return View();
		//}

		// POST: Predmeti/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public ActionResult Create([Bind(Include = "PredmetiID,NazivPredmeta,Cena,OdeljenjeID")] Predmeti predmeti)
		//{
		//    if (ModelState.IsValid)
		//    {
		//        db.Predmetis.Add(predmeti);
		//        db.SaveChanges();
		//        return RedirectToAction("Index");
		//    }

		//    ViewBag.OdeljenjeID = new SelectList(db.Odeljenjas, "OdeljenjeID", "Ime", predmeti.OdeljenjeID);
		//    return View(predmeti);
		//}

		// GET: Predmeti/Edit/5
		//public ActionResult Edit(int? id)
		//{
		//    if (id == null)
		//    {
		//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//    }
		//    Predmeti predmeti = db.Predmetis.Find(id);
		//    if (predmeti == null)
		//    {
		//        return HttpNotFound();
		//    }
		//    ViewBag.OdeljenjeID = new SelectList(db.Odeljenjas, "OdeljenjeID", "Ime", predmeti.OdeljenjeID);
		//    return View(predmeti);
		//}

		// POST: Predmeti/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public ActionResult Edit([Bind(Include = "PredmetiID,NazivPredmeta,Cena,OdeljenjeID")] Predmeti predmeti)
		//{
		//    if (ModelState.IsValid)
		//    {
		//        db.Entry(predmeti).State = EntityState.Modified;
		//        db.SaveChanges();
		//        return RedirectToAction("Index");
		//    }
		//    ViewBag.OdeljenjeID = new SelectList(db.Odeljenjas, "OdeljenjeID", "Ime", predmeti.OdeljenjeID);
		//    return View(predmeti);
		//}

		// GET: Predmeti/Delete/5
		public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Predmeti predmeti = db.Predmetis.Find(id);
            if (predmeti == null)
            {
                return HttpNotFound();
            }
            return View(predmeti);
        }

        // POST: Predmeti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Predmeti predmeti = db.Predmetis.Find(id);
            db.Predmetis.Remove(predmeti);
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
