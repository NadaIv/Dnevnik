namespace Dnevnik.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OdeljenjaSP : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
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
                      
                      SELECT t0.[OdeljenjeID]
                      FROM [dbo].[Odeljenja] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[OdeljenjeID] = @OdeljenjeID"
            );
            
            CreateStoredProcedure(
                "dbo.Odeljenja_Update",
                p => new
                    {
                        OdeljenjeID = p.Int(),
                        Ime = p.String(maxLength: 50),
                        Budzet = p.Decimal(precision: 19, scale: 4, storeType: "money"),
                        StartDate = p.DateTime(),
                        ProfesoriID = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Odeljenja]
                      SET [Ime] = @Ime, [Budzet] = @Budzet, [StartDate] = @StartDate, [ProfesoriID] = @ProfesoriID
                      WHERE ([OdeljenjeID] = @OdeljenjeID)"
            );
            
            CreateStoredProcedure(
                "dbo.Odeljenja_Delete",
                p => new
                    {
                        OdeljenjeID = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Odeljenja]
                      WHERE ([OdeljenjeID] = @OdeljenjeID)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Odeljenja_Delete");
            DropStoredProcedure("dbo.Odeljenja_Update");
            DropStoredProcedure("dbo.Odeljenja_Insert");
        }
    }
}
