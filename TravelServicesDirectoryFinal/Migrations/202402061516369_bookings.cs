namespace TravelServicesDirectoryFinal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bookings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingId = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        BookingDate = c.DateTime(nullable: false),
                        GrandTotal = c.Single(nullable: false),
                        PkgId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookingId)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Packages", t => t.PkgId, cascadeDelete: true)
                .Index(t => t.PkgId)
                .Index(t => t.CustomerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "PkgId", "dbo.Packages");
            DropForeignKey("dbo.Bookings", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Bookings", new[] { "CustomerId" });
            DropIndex("dbo.Bookings", new[] { "PkgId" });
            DropTable("dbo.Bookings");
        }
    }
}
