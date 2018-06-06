namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rating = c.Int(nullable: false),
                        CommentText = c.String(),
                        ClientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUsers", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.PriceListItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Single(nullable: false),
                        VehicleId = c.Int(nullable: false),
                        PriceListId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PriceLists", t => t.PriceListId, cascadeDelete: true)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId, cascadeDelete: true)
                .Index(t => t.VehicleId)
                .Index(t => t.PriceListId);
            
            CreateTable(
                "dbo.PriceLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimeOfReservation = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        TimeToReturn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PriceListItems", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.PriceListItems", "PriceListId", "dbo.PriceLists");
            DropForeignKey("dbo.Comments", "ClientId", "dbo.AppUsers");
            DropIndex("dbo.PriceListItems", new[] { "PriceListId" });
            DropIndex("dbo.PriceListItems", new[] { "VehicleId" });
            DropIndex("dbo.Comments", new[] { "ClientId" });
            DropTable("dbo.PriceLists");
            DropTable("dbo.PriceListItems");
            DropTable("dbo.Comments");
        }
    }
}
