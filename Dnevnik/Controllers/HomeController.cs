using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dnevnik.DAL;
using Dnevnik.ViewModels;

namespace Dnevnik.Controllers
{
	public class HomeController : Controller
	{
		private SchoolContext db = new SchoolContext();
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			//LINQ verzija

			//IQueryable<Ucen_PredmGroup> data = from ucenici in db.Ucenicis
			//   group ucenici by ucenici.DatumUpisa into dateGroup
			//select new Ucen_PredmGroup()
			//{
			//DatumUpisa = dateGroup.Key,
			//UceniciCount = dateGroup.Count()
			//};

			// SQL version of the above LINQ code.

			string query = "SELECT DatumUpisa, COUNT(*) AS UceniciCount "
				+ "FROM Osobe "
				+ "WHERE Discriminator = 'Ucenici' "
				+ "GROUP BY DatumUpisa";
			IEnumerable<Ucen_PredmGroup> data = db.Database.SqlQuery<Ucen_PredmGroup>(query);
			return View(data.ToList());
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
		protected override void Dispose(bool disposing)
		{
			db.Dispose();
			base.Dispose(disposing);
		}
	}
}