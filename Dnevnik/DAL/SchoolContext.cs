using Dnevnik.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Dnevnik.DAL
{
	public class SchoolContext : DbContext
	{
		//public SchoolContext() : base("SchoolContext")
		//{
		//}

		//public DbSet<Ucenici> Ucenicis { get; set; }
		//public DbSet<Ucen_Predm> Ucen_Predms { get; set; }
		//public DbSet<Predmeti> Predmetis { get; set; }

		//protected override void OnModelCreating(DbModelBuilder modelBuilder)
		//{
		//	modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		//}
		public DbSet<Predmeti> Predmetis { get; set; }
		public DbSet<Odeljenja> Odeljenjas { get; set; }
		public DbSet<Ucen_Predm> Ucen_Predms { get; set; }
		public DbSet<Profesori> Profesoris { get; set; }
		public DbSet<Ucenici> Ucenicis { get; set; }
		public DbSet<KancelarijaDodela> KancelarijaDodelas { get; set; }
		public DbSet<Osobe> Osobe { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
        modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			modelBuilder.Entity<Predmeti>()
				.HasMany(c => c.Profesoris).WithMany(i => i.Predmetis)
				.Map(t => t.MapLeftKey("PredmetiID")
					.MapRightKey("ProfesoriID")
					.ToTable("PredmetProfesor"));

			modelBuilder.Entity<Odeljenja>().MapToStoredProcedures();


		}

	}
}