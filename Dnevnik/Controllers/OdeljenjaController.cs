using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dnevnik.DAL;
using Dnevnik.Models;
using System.Data.Entity.Infrastructure;

namespace Dnevnik.Controllers
{
    public class OdeljenjaController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Odeljenja
        public async Task<ActionResult> Index()
        {
			var odeljenjas = db.Odeljenjas.Include(d => d.Administrator);

			return View(await odeljenjas.ToListAsync());
        }

        // GET: Odeljenja/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
			//Odeljenja odeljenja = await db.Odeljenjas.FindAsync(id);


			string query = "SELECT * FROM Odeljenja WHERE OdeljenjeID = @p0";
			Odeljenja odeljenja = await db.Odeljenjas.SqlQuery(query, id).SingleOrDefaultAsync();

			if (odeljenja == null)
            {
                return HttpNotFound();
            }
            return View(odeljenja);
        }

        // GET: Odeljenja/Create
        public ActionResult Create()
        {
			ViewBag.ProfesoriID = new SelectList(db.Profesoris, "ID", "ImeIPrezime");

			return View();
        }

        // POST: Odeljenja/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "OdeljenjeID,ImeIPrezime,Budzet,StartDate,ProfesoriID")] Odeljenja odeljenja)
        {
            if (ModelState.IsValid)
            {
                db.Odeljenjas.Add(odeljenja);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(odeljenja);
        }

        // GET: Odeljenja/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Odeljenja odeljenja = await db.Odeljenjas.FindAsync(id);
            if (odeljenja == null)
            {
                return HttpNotFound();
            }
			ViewBag.ProfesoriID = new SelectList(db.Profesoris, "ID", "ImeIPrezime", odeljenja.ProfesoriID);

			return View(odeljenja);
        }

		// POST: Odeljenja/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<ActionResult> Edit([Bind(Include = "OdeljenjeID,ImeIPrezime,Budzet,StartDate,ProfesoriID")] Odeljenja odeljenja)
		//{
		//    if (ModelState.IsValid)
		//    {
		//        db.Entry(odeljenja).State = EntityState.Modified;
		//        await db.SaveChangesAsync();
		//        return RedirectToAction("Index");
		//    }
		//    return View(odeljenja);
		//}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(int? id, byte[] rowVersion)
		{
			string[] fieldsToBind = new string[] { "Ime", "Budzet", "StartDate", "ProfesoriID", "RowVersion" };

			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var odeljenjeToUpdate = await db.Odeljenjas.FindAsync(id);
			if (odeljenjeToUpdate == null)
			{
				Odeljenja deletedOdeljenja = new Odeljenja();
				TryUpdateModel(deletedOdeljenja, fieldsToBind);
				ModelState.AddModelError(string.Empty,
					"Unable to save changes. The department was deleted by another user.");
				ViewBag.ProfesoriID = new SelectList(db.Profesoris, "ID", "ImeIPrezime", deletedOdeljenja.ProfesoriID);
				return View(deletedOdeljenja);
			}

			if (TryUpdateModel(odeljenjeToUpdate, fieldsToBind))
			{
				try
				{
					db.Entry(odeljenjeToUpdate).OriginalValues["RowVersion"] = rowVersion;
					await db.SaveChangesAsync();

					return RedirectToAction("Index");
				}
				catch (DbUpdateConcurrencyException ex)
				{
					var entry = ex.Entries.Single();
					var clientValues = (Odeljenja)entry.Entity;
					var databaseEntry = entry.GetDatabaseValues();
					if (databaseEntry == null)
					{
						ModelState.AddModelError(string.Empty,
							"Unable to save changes. The department was deleted by another user.");
					}
					else
					{
						var databaseValues = (Odeljenja)databaseEntry.ToObject();

						if (databaseValues.Ime != clientValues.Ime)
							ModelState.AddModelError("Ime", "Current value: "
								+ databaseValues.Ime);
						if (databaseValues.Budzet != clientValues.Budzet)
							ModelState.AddModelError("Budzet", "Current value: "
								+ String.Format("{0:c}", databaseValues.Budzet));
						if (databaseValues.StartDate != clientValues.StartDate)
							ModelState.AddModelError("StartDate", "Current value: "
								+ String.Format("{0:d}", databaseValues.StartDate));
						if (databaseValues.ProfesoriID != clientValues.ProfesoriID)
							ModelState.AddModelError("ProfesoriID", "Current value: "
								+ db.Profesoris.Find(databaseValues.ProfesoriID).ImeIPrezime);
						ModelState.AddModelError(string.Empty, "The record you attempted to edit "
							+ "was modified by another user after you got the original value. The "
							+ "edit operation was canceled and the current values in the database "
							+ "have been displayed. If you still want to edit this record, click "
							+ "the Save button again. Otherwise click the Back to List hyperlink.");
						odeljenjeToUpdate.RowVersion = databaseValues.RowVersion;
					}
				}
				catch (RetryLimitExceededException /* dex */)
				{
					//Log the error (uncomment dex variable name and add a line here to write a log.)
					ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
				}
			}
			ViewBag.ProfesoriID = new SelectList(db.Profesoris, "ID", "ImeIPrezime", odeljenjeToUpdate.ProfesoriID);
			return View(odeljenjeToUpdate);
		}


		// GET: Odeljenja/Delete/5
		//public async Task<ActionResult> Delete(int? id)
		//      {
		//          if (id == null)
		//          {
		//              return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//          }
		//          Odeljenja odeljenja = await db.Odeljenjas.FindAsync(id);
		//          if (odeljenja == null)
		//          {
		//              return HttpNotFound();
		//          }
		//          return View(odeljenja);
		//      }

		public async Task<ActionResult> Delete(int? id, bool? concurrencyError)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Odeljenja odeljenja = await db.Odeljenjas.FindAsync(id);
			if (odeljenja == null)
			{
				if (concurrencyError.GetValueOrDefault())
				{
					return RedirectToAction("Index");
				}
				return HttpNotFound();
			}

			if (concurrencyError.GetValueOrDefault())
			{
				ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete "
					+ "was modified by another user after you got the original values. "
					+ "The delete operation was canceled and the current values in the "
					+ "database have been displayed. If you still want to delete this "
					+ "record, click the Delete button again. Otherwise "
					+ "click the Back to List hyperlink.";
			}

			return View(odeljenja);
		}


		// POST: Odeljenja/Delete/5
		//[HttpPost, ActionName("Delete")]
		//      [ValidateAntiForgeryToken]
		//      public async Task<ActionResult> DeleteConfirmed(int id)
		//      {
		//          Odeljenja odeljenja = await db.Odeljenjas.FindAsync(id);
		//          db.Odeljenjas.Remove(odeljenja);
		//          await db.SaveChangesAsync();
		//          return RedirectToAction("Index");
		//      }


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Delete(Odeljenja odeljenja)
		{
			try
			{
				db.Entry(odeljenja).State = EntityState.Deleted;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			catch (DbUpdateConcurrencyException)
			{
				return RedirectToAction("Delete", new { concurrencyError = true, id = odeljenja.OdeljenjeID });
			}
			catch (DataException /* dex */)
			{
				//Log the error (uncomment dex variable name after DataException and add a line here to write a log.
				ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
				return View(odeljenja);
			}
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
