using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dnevnik.DAL;
using Dnevnik.Models;
using PagedList;

namespace Dnevnik.Controllers
{
	public class UceniciController : Controller
	{
		private SchoolContext db = new SchoolContext();

		// GET: Ucenici
		public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
			
		{
			ViewBag.CurrentSort = sortOrder;
			ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "ime_desc" : "";
			ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
			ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
			if (searchString != null)
			{
				page = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			ViewBag.CurrentFilter = searchString;

			var ucenicis = from s in db.Ucenicis
						   select s;
			if (!String.IsNullOrEmpty(searchString))
			{
				ucenicis = ucenicis.Where(s => s.Ime.Contains(searchString)
									   || s.Prezime.Contains(searchString));
			}
			switch (sortOrder)
			{
				case "ime_desc":
					ucenicis = ucenicis.OrderByDescending(s => s.Ime);
					break;
				case "name_desc":
					ucenicis = ucenicis.OrderByDescending(s => s.Prezime);
					break;
				case "Date":
					ucenicis = ucenicis.OrderBy(s => s.DatumUpisa);
					break;
				case "date_desc":
					ucenicis = ucenicis.OrderByDescending(s => s.DatumUpisa);
					break;
				default:
					ucenicis = ucenicis.OrderBy(s => s.Prezime);
					break;
			}
			int pageSize = 5;
			int pageNumber = (page ?? 1);
			return View(ucenicis.ToPagedList(pageNumber, pageSize));
		}

		// GET: Ucenici/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Ucenici ucenici = db.Ucenicis.Find(id);
			if (ucenici == null)
			{
				return HttpNotFound();
			}
			return View(ucenici);
		}

		// GET: Ucenici/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Ucenici/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Ime,Prezime,DatumUpisa")] Ucenici ucenici)
		{
			try
			{
				if (ModelState.IsValid)
				{
					db.Ucenicis.Add(ucenici);
					db.SaveChanges();
					return RedirectToAction("Index");
				}
			}
			catch (DataException /* dex */)
			{
				//Prijavi gresku

				ModelState.AddModelError("", "Ne mogu da sacuvam promene. Pokusajte ponovo,a ako se problem i dalje javlja, obratite se administratoru.");
			}

			return View(ucenici);
		}

		// GET: Ucenici/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Ucenici ucenici = db.Ucenicis.Find(id);
			if (ucenici == null)
			{
				return HttpNotFound();
			}
			return View(ucenici);
		}

		// POST: Ucenici/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost, ActionName("Edit")]
		[ValidateAntiForgeryToken]
		public ActionResult EditPost(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var ucenikToUpdate = db.Ucenicis.Find(id);
			if (TryUpdateModel(ucenikToUpdate, "",
			   new string[] { "Ime", "Prezime", "DatumUpisa" }))
			{
				try
				{
					db.SaveChanges();

					return RedirectToAction("Index");
				}
				catch (DataException /* dex */)
				{
					//Log the error (uncomment dex variable name and add a line here to write a log.
					ModelState.AddModelError("", "Ne mogu da sacuvam promene. Pokusajte ponovo,a ako se problem i dalje javlja, obratite se administratoru.");
				}
			}
			return View(ucenikToUpdate);
		}

		// GET: Ucenici/Delete/5
		public ActionResult Delete(int? id, bool? saveChangesError = false)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			if (saveChangesError.GetValueOrDefault())
			{
				ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
			}
			Ucenici ucenici = db.Ucenicis.Find(id);
			if (ucenici == null)
			{
				return HttpNotFound();
			}
			return View(ucenici);
		}

		// POST: Ucenici/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			try
			{
				Ucenici ucenici = db.Ucenicis.Find(id);
				db.Ucenicis.Remove(ucenici);
				db.SaveChanges();
			}
			catch (DataException/* dex */)
			{
				//Log the error (uncomment dex variable name and add a line here to write a log.
				return RedirectToAction("Delete", new { id = id, saveChangesError = true });
			}
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
