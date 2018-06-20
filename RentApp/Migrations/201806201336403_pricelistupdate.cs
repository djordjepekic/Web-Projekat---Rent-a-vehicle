namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pricelistupdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PriceListItems", "UserId", "dbo.AppUsers");
            DropForeignKey("dbo.PriceListItems", "PriceListId", "dbo.PriceLists");
            DropIndex("dbo.PriceListItems", new[] { "UserId" });
            DropIndex("dbo.PriceListItems", new[] { "PriceListId" });
            AddColumn("dbo.PriceLists", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.PriceListItems", "PriceListId", c => c.Int());
            CreateIndex("dbo.PriceListItems", "PriceListId");
            CreateIndex("dbo.PriceLists", "UserId");
            AddForeignKey("dbo.PriceLists", "UserId", "dbo.AppUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PriceListItems", "PriceListId", "dbo.PriceLists", "Id");
            DropColumn("dbo.PriceListItems", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PriceListItems", "UserId", c => c.Int(nullable: false));
            DropForeignKey("dbo.PriceListItems", "PriceListId", "dbo.PriceLists");
            DropForeignKey("dbo.PriceLists", "UserId", "dbo.AppUsers");
            DropIndex("dbo.PriceLists", new[] { "UserId" });
            DropIndex("dbo.PriceListItems", new[] { "PriceListId" });
            AlterColumn("dbo.PriceListItems", "PriceListId", c => c.Int(nullable: false));
            DropColumn("dbo.PriceLists", "UserId");
            CreateIndex("dbo.PriceListItems", "PriceListId");
            CreateIndex("dbo.PriceListItems", "UserId");
            AddForeignKey("dbo.PriceListItems", "PriceListId", "dbo.PriceLists", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PriceListItems", "UserId", "dbo.AppUsers", "Id", cascadeDelete: true);
        }
    }
}
