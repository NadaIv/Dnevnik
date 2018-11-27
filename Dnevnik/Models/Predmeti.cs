using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Dnevnik.Models
{
	public class Predmeti
	{
		
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Display(Name = "Broj")]
		public int PredmetiID { get; set; }

		[StringLength(50, MinimumLength = 3)]
		public string NazivPredmeta { get; set; }

		[Range(0, 5)]
		public int Cena { get; set; }

		public int OdeljenjeID { get; set; }

		public virtual Odeljenja Odeljenja { get; set; }
		public virtual ICollection<Ucen_Predm> Ucen_Predms { get; set; }
		public virtual ICollection<Profesori> Profesoris { get; set; }
	}
}
