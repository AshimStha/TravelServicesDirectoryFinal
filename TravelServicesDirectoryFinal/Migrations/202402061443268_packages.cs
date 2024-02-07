namespace TravelServicesDirectoryFinal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class packages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Packages",
                c => new
                    {
                        PkgId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.String(),
                        AccommodationType = c.String(),
                        Destination = c.String(),
                        Departure = c.DateTime(nullable: false),
                        Arrival = c.DateTime(nullable: false),
                        Cost = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.PkgId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Packages");
        }
    }
}
