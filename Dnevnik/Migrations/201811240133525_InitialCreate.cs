namespace Dnevnik.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.KancelarijaDodela",
                c => new
                    {
                        ProfesoriID = c.Int(nullable: false),
                        Lokacija = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProfesoriID)
                .ForeignKey("dbo.Profesori", t => t.ProfesoriID)
                .Index(t => t.ProfesoriID);
            
            CreateTable(
                "dbo.Profesori",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Ime = c.String(nullable: false, maxLength: 50),
                        Prezime = c.String(nullable: false, maxLength: 50),
                        DatumZaposlenja = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Predmeti",
                c => new
                    {
                        PredmetiID = c.Int(nullable: false),
                        NazivPredmeta = c.String(maxLength: 50),
                        Cena = c.Int(nullable: false),
                        OdeljenjeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PredmetiID)
                .ForeignKey("dbo.Odeljenja", t => t.OdeljenjeID, cascadeDelete: true)
                .Index(t => t.OdeljenjeID);
            
            CreateTable(
                "dbo.Odeljenja",
                c => new
                    {
                        OdeljenjeID = c.Int(nullable: false, identity: true),
                        Ime = c.String(maxLength: 50),
                        Budzet = c.Decimal(nullable: false, storeType: "money"),
                        StartDate = c.DateTime(nullable: false),
                        ProfesoriID = c.Int(),
                    })
                .PrimaryKey(t => t.OdeljenjeID)
                .ForeignKey("dbo.Profesori", t => t.ProfesoriID)
                .Index(t => t.ProfesoriID);
            
            CreateTable(
                "dbo.Ucen_Predm",
                c => new
                    {
                        Ucen_PredmID = c.Int(nullable: false, identity: true),
                        PredmetiID = c.Int(nullable: false),
                        UceniciID = c.Int(nullable: false),
                        Ocene = c.Int(),
                    })
                .PrimaryKey(t => t.Ucen_PredmID)
                .ForeignKey("dbo.Predmeti", t => t.PredmetiID, cascadeDelete: true)
                .ForeignKey("dbo.Ucenici", t => t.UceniciID, cascadeDelete: true)
                .Index(t => t.PredmetiID)
                .Index(t => t.UceniciID);
            
            CreateTable(
                "dbo.Ucenici",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Ime = c.String(nullable: false, maxLength: 50),
                        Prezime = c.String(nullable: false, maxLength: 50),
                        DatumUpisa = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PredmetProfesor",
                c => new
                    {
                        PredmetiID = c.Int(nullable: false),
                        ProfesoriID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PredmetiID, t.ProfesoriID })
                .ForeignKey("dbo.Predmeti", t => t.PredmetiID, cascadeDelete: true)
                .ForeignKey("dbo.Profesori", t => t.ProfesoriID, cascadeDelete: true)
                .Index(t => t.PredmetiID)
                .Index(t => t.ProfesoriID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KancelarijaDodela", "ProfesoriID", "dbo.Profesori");
            DropForeignKey("dbo.Ucen_Predm", "UceniciID", "dbo.Ucenici");
            DropForeignKey("dbo.Ucen_Predm", "PredmetiID", "dbo.Predmeti");
            DropForeignKey("dbo.PredmetProfesor", "ProfesoriID", "dbo.Profesori");
            DropForeignKey("dbo.PredmetProfesor", "PredmetiID", "dbo.Predmeti");
            DropForeignKey("dbo.Predmeti", "OdeljenjeID", "dbo.Odeljenja");
            DropForeignKey("dbo.Odeljenja", "ProfesoriID", "dbo.Profesori");
            DropIndex("dbo.PredmetProfesor", new[] { "ProfesoriID" });
            DropIndex("dbo.PredmetProfesor", new[] { "PredmetiID" });
            DropIndex("dbo.Ucen_Predm", new[] { "UceniciID" });
            DropIndex("dbo.Ucen_Predm", new[] { "PredmetiID" });
            DropIndex("dbo.Odeljenja", new[] { "ProfesoriID" });
            DropIndex("dbo.Predmeti", new[] { "OdeljenjeID" });
            DropIndex("dbo.KancelarijaDodela", new[] { "ProfesoriID" });
            DropTable("dbo.PredmetProfesor");
            DropTable("dbo.Ucenici");
            DropTable("dbo.Ucen_Predm");
            DropTable("dbo.Odeljenja");
            DropTable("dbo.Predmeti");
            DropTable("dbo.Profesori");
            DropTable("dbo.KancelarijaDodela");
        }
    }
}
