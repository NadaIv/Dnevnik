using Dnevnik.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Dnevnik.DAL
{
	public class SchoolInitializer :DropCreateDatabaseIfModelChanges<SchoolContext>
	{
		protected override void Seed(SchoolContext context)
		{
			var ucenicis = new List<Ucenici>
			{
			new Ucenici{Ime="Carson",Prezime="Alexander",DatumUpisa=DateTime.Parse("2005-09-01")},
			new Ucenici{Ime="Meredith",Prezime="Alonso",DatumUpisa=DateTime.Parse("2002-09-01")},
			new Ucenici{Ime="Arturo",Prezime="Anand",DatumUpisa=DateTime.Parse("2003-09-01")},
			new Ucenici{Ime="Gytis",Prezime="Barzdukas",DatumUpisa=DateTime.Parse("2002-09-01")},
			new Ucenici{Ime="Yan",Prezime="Li",DatumUpisa=DateTime.Parse("2002-09-01")},
			new Ucenici{Ime="Peggy",Prezime="Justice",DatumUpisa=DateTime.Parse("2001-09-01")},
			new Ucenici{Ime="Laura",Prezime="Norman",DatumUpisa=DateTime.Parse("2003-09-01")},
			new Ucenici{Ime="Nino",Prezime="Olivetto",DatumUpisa=DateTime.Parse("2005-09-01")}
			};
			
			ucenicis.ForEach(s => context.Ucenicis.Add(s));
			context.SaveChanges();

			var predmetis = new List<Predmeti>
			{
			new Predmeti{PredmetiID=1050,NazivPredmeta="Chemistry",Cena=3,},
			new Predmeti{PredmetiID=4022,NazivPredmeta="Microeconomics",Cena=3,},
			new Predmeti{PredmetiID=4041,NazivPredmeta="Macroeconomics",Cena=3,},
			new Predmeti{PredmetiID=1045,NazivPredmeta="Calculus",Cena=4,},
			new Predmeti{PredmetiID=3141,NazivPredmeta="Trigonometry",Cena=4,},
			new Predmeti{PredmetiID=2021,NazivPredmeta="Composition",Cena=3,},
			new Predmeti{PredmetiID=2042,NazivPredmeta="Literature",Cena=4,}
			};

			predmetis.ForEach(s => context.Predmetis.Add(s));
			context.SaveChanges();

			var ucen_predms = new List<Ucen_Predm>
			{
			new Ucen_Predm{UceniciID=1,PredmetiID=1050,Ocene=Ocene.Nedovoljan},
			new Ucen_Predm{UceniciID=1,PredmetiID=4022,Ocene=Ocene.Dobar},
			new Ucen_Predm{UceniciID=1,PredmetiID=4041,Ocene=Ocene.Dovoljan},
			new Ucen_Predm{UceniciID=2,PredmetiID=1045,Ocene=Ocene.Dovoljan},
			new Ucen_Predm{UceniciID=2,PredmetiID=3141,Ocene=Ocene.Odlican},
			new Ucen_Predm{UceniciID=2,PredmetiID=2021,Ocene=Ocene.Odlican},
			new Ucen_Predm{UceniciID=3,PredmetiID=1050},
			new Ucen_Predm{UceniciID=4,PredmetiID=1050,},
			new Ucen_Predm{UceniciID=4,PredmetiID=4022,Ocene=Ocene.Odlican},
			
			};
			ucen_predms.ForEach(s => context.Ucen_Predms.Add(s));
			context.SaveChanges();
		}
	}
}
