using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dnevnik.Models;

namespace Dnevnik.ViewModels
{
	public class ProfesoriIndexData
	{
		public IEnumerable<Profesori> Profesoris { get; set; }
		public IEnumerable<Predmeti> Predmetis { get; set; }
		public IEnumerable<Ucen_Predm> Ucen_Predms { get; set; }

	}
}