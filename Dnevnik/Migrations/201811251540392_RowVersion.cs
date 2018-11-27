namespace Dnevnik.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RowVersion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Odeljenja", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AlterStoredProcedure(
                "dbo.Odeljenja_Insert",
                p => new
                    {
                        Ime = p.String(maxLength: 50),
                        Budzet = p.Decimal(precision: 19, scale: 4, storeType: "money"),
                        StartDate = p.DateTime(),
                        ProfesoriID = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Odeljenja]([Ime], [Budzet], [StartDate], [ProfesoriID])
                      VALUES (@Ime, @Budzet, @StartDate, @ProfesoriID)
                      
                      DECLARE @OdeljenjeID int
                      SELECT @OdeljenjeID = [OdeljenjeID]
                      FROM [dbo].[Odeljenja]
                      WHERE @@ROWCOUNT > 0 AND [OdeljenjeID] = scope_identity()
                      
                      SELECT t0.[OdeljenjeID], t0.[RowVersion]
                      FROM [dbo].[Odeljenja] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[OdeljenjeID] = @OdeljenjeID"
            );
            
            AlterStoredProcedure(
                "dbo.Odeljenja_Update",
                p => new
                    {
                        OdeljenjeID = p.Int(),
                        Ime = p.String(maxLength: 50),
                        Budzet = p.Decimal(precision: 19, scale: 4, storeType: "money"),
                        StartDate = p.DateTime(),
                        ProfesoriID = p.Int(),
                        RowVersion_Original = p.Binary(maxLength: 8, fixedLength: true, storeType: "rowversion"),
                    },
                body:
                    @"UPDATE [dbo].[Odeljenja]
                      SET [Ime] = @Ime, [Budzet] = @Budzet, [StartDate] = @StartDate, [ProfesoriID] = @ProfesoriID
                      WHERE (([OdeljenjeID] = @OdeljenjeID) AND (([RowVersion] = @RowVersion_Original) OR ([RowVersion] IS NULL AND @RowVersion_Original IS NULL)))
                      
                      SELECT t0.[RowVersion]
                      FROM [dbo].[Odeljenja] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[OdeljenjeID] = @OdeljenjeID"
            );
            
            AlterStoredProcedure(
                "dbo.Odeljenja_Delete",
                p => new
                    {
                        OdeljenjeID = p.Int(),
                        RowVersion_Original = p.Binary(maxLength: 8, fixedLength: true, storeType: "rowversion"),
                    },
                body:
                    @"DELETE [dbo].[Odeljenja]
                      WHERE (([OdeljenjeID] = @OdeljenjeID) AND (([RowVersion] = @RowVersion_Original) OR ([RowVersion] IS NULL AND @RowVersion_Original IS NULL)))"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Odeljenja", "RowVersion");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
