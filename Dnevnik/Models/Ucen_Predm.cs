using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dnevnik.Models
{
	public enum Ocene
	{
		Nedovoljan, 
		Dovoljan,
		Dobar, 
		Vrlodobar,
		Odlican
	}

	public class Ucen_Predm
	{  
		public int Ucen_PredmID { get; set; }
		public int PredmetiID { get; set; }
		public int UceniciID { get; set; }
		[DisplayFormat(NullDisplayText = "Bez ocene")]
		public Ocene? Ocene { get; set; }

		public virtual Predmeti Predmeti { get; set; }
		public virtual Ucenici Ucenici { get; set; }
		
	}
}