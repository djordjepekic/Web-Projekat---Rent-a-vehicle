namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vehicleModelUpdate2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vehicles", "UserId", "dbo.AppUsers");
            DropIndex("dbo.Vehicles", new[] { "UserId" });
            AddColumn("dbo.PriceListItems", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.PriceListItems", "UserId");
            AddForeignKey("dbo.PriceListItems", "UserId", "dbo.AppUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.Vehicles", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "UserId", c => c.Int(nullable: false));
            DropForeignKey("dbo.PriceListItems", "UserId", "dbo.AppUsers");
            DropIndex("dbo.PriceListItems", new[] { "UserId" });
            DropColumn("dbo.PriceListItems", "UserId");
            CreateIndex("dbo.Vehicles", "UserId");
            AddForeignKey("dbo.Vehicles", "UserId", "dbo.AppUsers", "Id", cascadeDelete: true);
        }
    }
}
