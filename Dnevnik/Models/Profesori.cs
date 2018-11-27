using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dnevnik.Models
{
	public class Profesori : Osobe
	{
		//public int ID { get; set; }

		//[Required]
		//[Display(Name = "Ime")]
		//[StringLength(50)]
		//public string Ime { get; set; }

		//[Required]
		//[Display(Name = "Prezime")]
		//[StringLength(50)]
		//public string Prezime { get; set; }

		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		[Display(Name = "Datum zaposlenja")]
		public DateTime DatumZaposlenja { get; set; }

		//[Display(Name = "Ime i prezime")]
		//public string ImeIPrezime
		//{
		//	get { return Ime + ", " + Prezime; }
		//}

		public virtual ICollection<Predmeti> Predmetis { get; set; }
		public virtual KancelarijaDodela KancelarijaDodela { get; set; }
	}
}
