using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Dnevnik.Models
{
	public class KancelarijaDodela
	{
		[Key]
		[ForeignKey("Profesori")]
		public int ProfesoriID { get; set; }
		[StringLength(50)]
		[Display(Name = "Lokacija kancelarije")]
		public string Lokacija { get; set; }

		public virtual Profesori Profesori { get; set; }
	}
}