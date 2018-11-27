namespace Dnevnik.Migrations
{
	using Dnevnik.Models;
	using Dnevnik.DAL;
	using System;
	using System.Collections.Generic;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<Dnevnik.DAL.SchoolContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Dnevnik.DAL.SchoolContext context)
        {
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data.
			var ucenicis = new List<Ucenici>
			{
				new Ucenici { Ime = "Carson",   Prezime = "Alexander",
					DatumUpisa = DateTime.Parse("2010-09-01") },
				new Ucenici { Ime = "Meredith", Prezime = "Alonso",
					DatumUpisa = DateTime.Parse("2012-09-01") },
				new Ucenici { Ime = "Arturo",   Prezime = "Anand",
					DatumUpisa = DateTime.Parse("2013-09-01") },
				new Ucenici { Ime = "Gytis",    Prezime = "Barzdukas",
					DatumUpisa = DateTime.Parse("2012-09-01") },
				new Ucenici { Ime = "Yan",      Prezime = "Li",
					DatumUpisa = DateTime.Parse("2012-09-01") },
				new Ucenici { Ime = "Peggy",    Prezime = "Justice",
					DatumUpisa = DateTime.Parse("2011-09-01") },
				new Ucenici { Ime = "Laura",    Prezime = "Norman",
					DatumUpisa = DateTime.Parse("2013-09-01") },
				new Ucenici { Ime = "Nino",     Prezime = "Olivetto",
					DatumUpisa = DateTime.Parse("2005-09-01") }
			};

			ucenicis.ForEach(s => context.Ucenicis.AddOrUpdate(p => p.Prezime, s));
			context.SaveChanges();

			var profesoris = new List<Profesori>
			{
				new Profesori { Ime = "Kim",     Prezime = "Abercrombie",
					DatumZaposlenja = DateTime.Parse("1995-03-11") },
				new Profesori { Ime = "Fadi",    Prezime = "Fakhouri",
					DatumZaposlenja = DateTime.Parse("2002-07-06") },
				new Profesori { Ime = "Roger",   Prezime = "Harui",
					DatumZaposlenja = DateTime.Parse("1998-07-01") },
				new Profesori { Ime = "Candace", Prezime = "Kapoor",
					DatumZaposlenja = DateTime.Parse("2001-01-15") },
				new Profesori { Ime = "Roger",   Prezime = "Zheng",
					DatumZaposlenja = DateTime.Parse("2004-02-12") }
			};
			profesoris.ForEach(s => context.Profesoris.AddOrUpdate(p => p.Prezime, s));
			context.SaveChanges();

			var odeljenjas = new List<Odeljenja>
			{
				new Odeljenja { Ime = "English",     Budzet = 350000,
					StartDate = DateTime.Parse("2007-09-01"),
					ProfesoriID  = profesoris.Single( i => i.Prezime == "Abercrombie").ID },
				new Odeljenja { Ime = "Mathematics", Budzet = 100000,
					StartDate = DateTime.Parse("2007-09-01"),
					ProfesoriID  = profesoris.Single( i => i.Prezime == "Fakhouri").ID },
				new Odeljenja { Ime = "Engineering", Budzet = 350000,
					StartDate = DateTime.Parse("2007-09-01"),
					ProfesoriID  = profesoris.Single( i => i.Prezime == "Harui").ID },
				new Odeljenja { Ime = "Economics",   Budzet = 100000,
					StartDate = DateTime.Parse("2007-09-01"),
					ProfesoriID  = profesoris.Single( i => i.Prezime == "Kapoor").ID }
			};
			odeljenjas.ForEach(s => context.Odeljenjas.AddOrUpdate(p => p.Ime, s));
			context.SaveChanges();

			var predmetis = new List<Predmeti>
			{
				new Predmeti {PredmetiID = 1050, NazivPredmeta = "Chemistry",      Cena = 3,
				  OdeljenjeID = odeljenjas.Single( s => s.Ime == "Engineering").OdeljenjeID,
				  Profesoris = new List<Profesori>()
				},
				new Predmeti {PredmetiID = 4022, NazivPredmeta = "Microeconomics", Cena = 3,
				  OdeljenjeID = odeljenjas.Single( s => s.Ime == "Economics").OdeljenjeID,
				  Profesoris = new List<Profesori>()
				},
				new Predmeti {PredmetiID = 4041, NazivPredmeta = "Macroeconomics", Cena = 3,
				  OdeljenjeID = odeljenjas.Single( s => s.Ime == "Economics").OdeljenjeID,
				  Profesoris = new List<Profesori>()
				},
				new Predmeti {PredmetiID = 1045, NazivPredmeta = "Calculus",       Cena = 4,
				  OdeljenjeID = odeljenjas.Single( s => s.Ime == "Mathematics").OdeljenjeID,
				  Profesoris = new List<Profesori>()
				},
				new Predmeti {PredmetiID = 3141, NazivPredmeta = "Trigonometry",   Cena = 4,
				  OdeljenjeID = odeljenjas.Single( s => s.Ime == "Mathematics").OdeljenjeID,
				  Profesoris = new List<Profesori>()
				},
				new Predmeti {PredmetiID = 2021, NazivPredmeta = "Composition",    Cena = 3,
				  OdeljenjeID = odeljenjas.Single( s => s.Ime == "English").OdeljenjeID,
				  Profesoris = new List<Profesori>()
				},
				new Predmeti {PredmetiID = 2042, NazivPredmeta = "Literature",     Cena = 4,
				  OdeljenjeID = odeljenjas.Single( s => s.Ime == "English").OdeljenjeID,
				  Profesoris = new List<Profesori>()
				},
			};
			predmetis.ForEach(s => context.Predmetis.AddOrUpdate(p => p.PredmetiID, s));
			context.SaveChanges();

			var kancelarijaDodela = new List<KancelarijaDodela>
			{
				new KancelarijaDodela {
					ProfesoriID = profesoris.Single( i => i.Prezime == "Fakhouri").ID,
					Lokacija = "Smith 17" },
				new KancelarijaDodela {
					ProfesoriID = profesoris.Single( i => i.Prezime == "Harui").ID,
					Lokacija = "Gowan 27" },
				new KancelarijaDodela {
					ProfesoriID = profesoris.Single( i => i.Prezime == "Kapoor").ID,
					Lokacija = "Thompson 304" },
			};
			kancelarijaDodela.ForEach(s => context.KancelarijaDodelas.AddOrUpdate(p => p.ProfesoriID, s));
			context.SaveChanges();

			AddOrUpdateProfesor(context, "Chemistry", "Kapoor");
			AddOrUpdateProfesor(context, "Chemistry", "Harui");
			AddOrUpdateProfesor(context, "Microeconomics", "Zheng");
			AddOrUpdateProfesor(context, "Macroeconomics", "Zheng");

			AddOrUpdateProfesor(context, "Calculus", "Fakhouri");
			AddOrUpdateProfesor(context, "Trigonometry", "Harui");
			AddOrUpdateProfesor(context, "Composition", "Abercrombie");
			AddOrUpdateProfesor(context, "Literature", "Abercrombie");

			context.SaveChanges();

			var ucen_Predms = new List<Ucen_Predm>
			{
				new Ucen_Predm {
					PredmetiID = predmetis.Single(c => c.NazivPredmeta == "Chemistry" ).PredmetiID,
					UceniciID = ucenicis.Single(s => s.Prezime == "Alexander").ID,
					
					Ocene = Ocene.Nedovoljan
				},
				 new Ucen_Predm {
					UceniciID = ucenicis.Single(s => s.Prezime == "Alexander").ID,
					PredmetiID = predmetis.Single(c => c.NazivPredmeta == "Microeconomics" ).PredmetiID,
					Ocene = Ocene.Dobar
				 },
				 new Ucen_Predm {
					UceniciID = ucenicis.Single(s => s.Prezime == "Alexander").ID,
					PredmetiID = predmetis.Single(c => c.NazivPredmeta == "Macroeconomics" ).PredmetiID,
					Ocene = Ocene.Dobar
				 },
				 new Ucen_Predm {
					 UceniciID = ucenicis.Single(s => s.Prezime == "Alonso").ID,
					PredmetiID = predmetis.Single(c => c.NazivPredmeta == "Calculus" ).PredmetiID,
					Ocene = Ocene.Dobar
				 },
				 new Ucen_Predm {
					 UceniciID = ucenicis.Single(s => s.Prezime == "Alonso").ID,
					PredmetiID = predmetis.Single(c => c.NazivPredmeta == "Trigonometry" ).PredmetiID,
					Ocene = Ocene.Dovoljan
				 },
				 new Ucen_Predm {
					UceniciID = ucenicis.Single(s => s.Prezime == "Alonso").ID,
					PredmetiID = predmetis.Single(c => c.NazivPredmeta == "Composition" ).PredmetiID,
					Ocene = Ocene.Dovoljan
				 },
				 new Ucen_Predm {
					UceniciID = ucenicis.Single(s => s.Prezime == "Anand").ID,
					PredmetiID = predmetis.Single(c => c.NazivPredmeta == "Chemistry" ).PredmetiID,
					Ocene = Ocene.Dovoljan
				 },
				 new Ucen_Predm {
					UceniciID = ucenicis.Single(s => s.Prezime == "Anand").ID,
					PredmetiID = predmetis.Single(c => c.NazivPredmeta == "Microeconomics").PredmetiID,
					Ocene = Ocene.Dovoljan
				 },
				new Ucen_Predm {
					UceniciID = ucenicis.Single(s => s.Prezime == "Barzdukas").ID,
					PredmetiID = predmetis.Single(c => c.NazivPredmeta == "Chemistry").PredmetiID,
					Ocene = Ocene.Odlican
				 },
				 new Ucen_Predm {
					UceniciID = ucenicis.Single(s => s.Prezime == "Li").ID,
					PredmetiID = predmetis.Single(c => c.NazivPredmeta == "Composition").PredmetiID,
					Ocene = Ocene.Odlican
				 },
				 new Ucen_Predm {
					UceniciID = ucenicis.Single(s => s.Prezime == "Justice").ID,
					PredmetiID = predmetis.Single(c => c.NazivPredmeta == "Literature").PredmetiID,
					Ocene = Ocene.Vrlodobar
				 }
			};

			foreach (Ucen_Predm e in ucen_Predms)
			{
				var ucen_PredmInDataBase = context.Ucen_Predms.Where(
					s =>
						 s.Ucenici.ID == e.UceniciID &&
						 s.Predmeti.PredmetiID == e.PredmetiID).SingleOrDefault();
				if (ucen_PredmInDataBase == null)
				{
					context.Ucen_Predms.Add(e);
				}
			}
			context.SaveChanges();
		}

		void AddOrUpdateProfesor(SchoolContext context, string predmetiNaziv, string profesorPrezime)
		{
			var crs = context.Predmetis.SingleOrDefault(c => c.NazivPredmeta == predmetiNaziv);
			var inst = crs.Profesoris.SingleOrDefault(i => i.Prezime == profesorPrezime);
			if (inst == null)
				crs.Profesoris.Add(context.Profesoris.SingleOrDefault(i => i.Prezime == profesorPrezime));
		}
	}
    }

