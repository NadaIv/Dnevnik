using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dnevnik.Models
{
	public class Ucenici : Osobe
	{   
		//public int ID { get; set; }
		//[Required]
		//[StringLength(50, ErrorMessage = "Ime ne moze biti duze od 50 karaktera.")]
		//[Display(Name = "Ime")]
		//public string Ime { get; set; }
		//[Required]
		//[StringLength(50, ErrorMessage = "Prezime ne moze biti duze od 50 karaktera.")]
		//[Display(Name = "Prezime")]
		//public string Prezime { get; set; }
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		[Display(Name = "Datum upisa")]
		public DateTime DatumUpisa { get; set; }

		//[Display(Name = "Ime i prezime")]
		//public string ImeIPrezime
		//{
		//	get
		//	{
		//		return Ime + ", " + Prezime;
		//	}
		//}

		public virtual ICollection<Ucen_Predm> Ucen_Predms { get; set; }

	}
}