namespace Dnevnik.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inheritance : DbMigration
    {
        public override void Up()
        {
			//RenameTable(name: "dbo.Profesori", newName: "Osobe");
			//AddColumn("dbo.Osobe", "DatumUpisa", c => c.DateTime());
			//AddColumn("dbo.Osobe", "Discriminator", c => c.String(nullable: false, maxLength: 128));
			//AlterColumn("dbo.Osobe", "DatumZaposlenja", c => c.DateTime());
			//DropTable("dbo.Ucenici");

			// Drop foreign keys and indexes that point to tables we're going to drop.
			DropForeignKey("dbo.Ucen_Predm", "UceniciID", "dbo.Ucenici");
			DropIndex("dbo.Ucen_Predm", new[] { "UceniciID" });

			RenameTable(name: "dbo.Profesori", newName: "Osobe");
			AddColumn("dbo.Osobe", "DatumUpisa", c => c.DateTime());
			AddColumn("dbo.Osobe", "Discriminator", c => c.String(nullable: false, maxLength: 128, defaultValue: "Profesori"));
			AlterColumn("dbo.Osobe", "DatumZaposlenja", c => c.DateTime());
			AddColumn("dbo.Osobe", "OldId", c => c.Int(nullable: true));

			// Copy existing Student data into new Person table.
			Sql("INSERT INTO dbo.Osobe (Ime, Prezime, DatumZaposlenja, DatumUpisa, Discriminator, OldId) SELECT Ime, Prezime, null AS DatumZaposlenja, DatumUpisa, 'Ucenici' AS Discriminator, ID AS OldId FROM dbo.Ucenici");

			// Fix up existing relationships to match new PK's.
			Sql("UPDATE dbo.Ucen_Predm SET UceniciId = (SELECT ID FROM dbo.Osobe WHERE OldId = Ucen_Predm.UceniciId AND Discriminator = 'Ucenici')");

			// Remove temporary key
			DropColumn("dbo.Osobe", "OldId");

			DropTable("dbo.Ucenici");

			// Re-create foreign keys and indexes pointing to new table.
			AddForeignKey("dbo.Ucen_Predm", "UceniciID", "dbo.Osobe", "ID", cascadeDelete: true);
			CreateIndex("dbo.Ucen_Predm", "UceniciID");
		}
        
        public override void Down()
        {
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
            
            AlterColumn("dbo.Osobe", "DatumZaposlenja", c => c.DateTime(nullable: false));
            DropColumn("dbo.Osobe", "Discriminator");
            DropColumn("dbo.Osobe", "DatumUpisa");
            RenameTable(name: "dbo.Osobe", newName: "Profesori");
        }
    }
}
