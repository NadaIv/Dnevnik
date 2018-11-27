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
using Dnevnik.ViewModels;

namespace Dnevnik.Controllers
{
    public class ProfesoriController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Profesori
        public ActionResult Index(int? id, int? predmetiID)
        {
			var viewModel = new ProfesoriIndexData();
			viewModel.Profesoris = db.Profesoris
				.Include(i => i.KancelarijaDodela)
				.Include(i => i.Predmetis.Select(c => c.Odeljenja))
				.OrderBy(i => i.Ime);

			if (id != null)
			{
				ViewBag.ProfesoriID = id.Value;
				viewModel.Predmetis = viewModel.Profesoris.Where(
					i => i.ID == id.Value).Single().Predmetis;
			}

			if (predmetiID != null)
			{
				ViewBag.PredmetiID = predmetiID.Value;

				// Lazy loading
				//viewModel.Ucen_Predms = viewModel.Predmetis.Where(
				//	x => x.PredmetiID == predmetiID).Single().Ucen_Predms;

				// Explicit loading
				var selectedPredmeti = viewModel.Predmetis.Where(x => x.PredmetiID == predmetiID).Single();
				db.Entry(selectedPredmeti).Collection(x => x.Ucen_Predms).Load();
				foreach (Ucen_Predm ucen_Predm in selectedPredmeti.Ucen_Predms)
				{
					db.Entry(ucen_Predm).Reference(x => x.Ucenici).Load();
				}

				viewModel.Ucen_Predms = selectedPredmeti.Ucen_Predms;
			}

				return View(viewModel);
		}

        // GET: Profesori/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profesori profesori = db.Profesoris.Find(id);
            if (profesori == null)
            {
                return HttpNotFound();
            }
            return View(profesori);
        }

		public ActionResult Create()
		{
			var profesori = new Profesori();
			profesori.Predmetis = new List<Predmeti>();
			UbacitiDodeljenePredmeteData(profesori);
			return View();
		}


		// GET: Profesori/Create
		//public ActionResult Create()
		//{
		//    ViewBag.ID = new SelectList(db.KancelarijaDodelas, "ProfesorID", "Lokacija");
		//    return View();
		//}

		// POST: Profesori/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Ime,Prezime,DatumZaposlenja,KancelarijaDodela")]Profesori profesori, string[] selectedPredmeti)
		{
			if (selectedPredmeti != null)
			{
				profesori.Predmetis = new List<Predmeti>();
				foreach (var predmeti in selectedPredmeti)
				{
					var predmetiToAdd = db.Predmetis.Find(int.Parse(predmeti));
					profesori.Predmetis.Add(predmetiToAdd);
				}
			}
			if (ModelState.IsValid)
			{
				db.Profesoris.Add(profesori);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			UbacitiDodeljenePredmeteData(profesori);
			return View(profesori);
		}

		//[HttpPost]
		//      [ValidateAntiForgeryToken]
		//      public ActionResult Create([Bind(Include = "ID,Ime,Prezime,DatumZaposlenja")] Profesori profesori)
		//      {
		//          if (ModelState.IsValid)
		//          {
		//              db.Profesoris.Add(profesori);
		//              db.SaveChanges();
		//              return RedirectToAction("Index");
		//          }

		//          ViewBag.ID = new SelectList(db.KancelarijaDodelas, "ProfesorID", "Lokacija", profesori.ID);
		//          return View(profesori);
		//      }

		// GET: Profesori/Edit/5
		public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
			//Profesori profesori = db.Profesoris.Find(id);
			Profesori profesori = db.Profesoris
	   .Include(i => i.KancelarijaDodela)
	   .Include(i => i.Predmetis)
	   .Where(i => i.ID == id)
	   .Single();
			UbacitiDodeljenePredmeteData(profesori);
			if (profesori == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.KancelarijaDodelas, "ProfesoriID", "Lokacija", profesori.ID);
            return View(profesori);

        }
		private void UbacitiDodeljenePredmeteData(Profesori profesori)
		{
			var allPredmeti = db.Predmetis;
			var profesoriPredmeti = new HashSet<int>(profesori.Predmetis.Select(c => c.PredmetiID));
			var viewModel = new List<DodelaPredmetaData>();
			foreach (var predmeti in allPredmeti)
			{
				viewModel.Add(new DodelaPredmetaData
				{
					PredmetiID = predmeti.PredmetiID,
					NazivPredmeta = predmeti.NazivPredmeta,
					Dodela = profesoriPredmeti.Contains(predmeti.PredmetiID)
				});
			}
			ViewBag.Predmetis = viewModel;
		}

		// POST: Profesori/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.


		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public ActionResult Edit([Bind(Include = "ID,Ime,Prezime,DatumZaposlenja")] Profesori profesori)
		//{
		//    if (ModelState.IsValid)
		//    {
		//        db.Entry(profesori).State = EntityState.Modified;
		//        db.SaveChanges();
		//        return RedirectToAction("Index");
		//    }
		//    ViewBag.ID = new SelectList(db.KancelarijaDodelas, "ProfesorID", "Lokacija", profesori.ID);
		//    return View(profesori);
		//}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int? id, string[] selectedPredmeti)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var profesoriToUpdate = db.Profesoris
			   .Include(i => i.KancelarijaDodela)
			   .Include(i => i.Predmetis)
			   .Where(i => i.ID == id)
			   .Single();

			if (TryUpdateModel(profesoriToUpdate, "",
			   new string[] { "Ime", "Prezime", "DatumZaposlenja", "KancelarijaDodela" }))
			{
				try
				{
					if (String.IsNullOrWhiteSpace(profesoriToUpdate.KancelarijaDodela.Lokacija))
					{
						profesoriToUpdate.KancelarijaDodela = null;
					}
					UpdateProfesoriPredmeti(selectedPredmeti, profesoriToUpdate);


					db.SaveChanges();

					return RedirectToAction("Index");
				}
				catch (RetryLimitExceededException /* dex */)
				{
					//Log the error (uncomment dex variable name and add a line here to write a log.
					ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
				}
			}
			UbacitiDodeljenePredmeteData(profesoriToUpdate);
			return View(profesoriToUpdate);
		}

		private void UpdateProfesoriPredmeti(string[] selectedPredmeti, Profesori profesoriToUpdate)
		{
			if (selectedPredmeti == null)
			{
				profesoriToUpdate.Predmetis = new List<Predmeti>();
				return;
			}

			var selectedPredmetiHS = new HashSet<string>(selectedPredmeti);
			var profesoriPredmeti = new HashSet<int>
				(profesoriToUpdate.Predmetis.Select(c => c.PredmetiID));
			foreach (var predmeti in db.Predmetis)
			{
				if (selectedPredmetiHS.Contains(predmeti.PredmetiID.ToString()))
				{
					if (!profesoriPredmeti.Contains(predmeti.PredmetiID))
					{
						profesoriToUpdate.Predmetis.Add(predmeti);
					}
				}
				else
				{
					if (profesoriPredmeti.Contains(predmeti.PredmetiID))
					{
						profesoriToUpdate.Predmetis.Remove(predmeti);
					}
				}
			}
		}

		// GET: Profesori/Delete/5
		public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profesori profesori = db.Profesoris.Find(id);
            if (profesori == null)
            {
                return HttpNotFound();
            }
            return View(profesori);
        }

        // POST: Profesori/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
			//Profesori profesori = db.Profesoris.Find(id);

			Profesori profesori = db.Profesoris
	.Include(i => i.KancelarijaDodela)
	.Where(i => i.ID == id)
	.Single();

			db.Profesoris.Remove(profesori);

			var odeljenja = db.Odeljenjas
	   .Where(d => d.OdeljenjeID == id)
	   .SingleOrDefault();
			if (odeljenja != null)
			{
				odeljenja.ProfesoriID = null;
			}

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
