using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dnevnik.Models
{
	public class Odeljenja
	{	[Key]
		public int OdeljenjeID { get; set; }

		[StringLength(50, MinimumLength = 3)]
		public string Ime { get; set; }


		[DataType(DataType.Currency)]
		[Column(TypeName = "money")]
		public decimal Budzet { get; set; }


		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		[Display(Name = "Start Date")]
		public DateTime StartDate { get; set; }

		[Display(Name = "Administrator")]
		public int? ProfesoriID { get; set; }

		[Timestamp]
		public byte[] RowVersion { get; set; }

		public virtual Profesori Administrator { get; set; }
		public virtual ICollection<Predmeti> Predmetis { get; set; }

	}
}