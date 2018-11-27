using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dnevnik.Models
{
	public class Osobe
	{
		public int ID { get; set; }

		[Required]
		[StringLength(50)]
		[Display(Name = "Ime")]
		public string Ime { get; set; }
		[Required]
		[StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
		
		[Display(Name = "Prezime")]
		public string Prezime { get; set; }

		[Display(Name = "Ime i prezime")]
		public string ImeIPrezime
		{
			get
			{
				return Ime + ", " + Prezime;
			}
		}
	}
}