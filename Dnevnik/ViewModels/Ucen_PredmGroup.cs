using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dnevnik.ViewModels
{
	public class Ucen_PredmGroup
	{
		[DataType(DataType.Date)]
		public DateTime? DatumUpisa { get; set; }

		public int UceniciCount { get; set; }
	}
}