namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TakeAndReturnOffice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PriceLists", "TakeOfficeId", c => c.Int());
            AddColumn("dbo.PriceLists", "ReturnOfficeId", c => c.Int());
            CreateIndex("dbo.PriceLists", "TakeOfficeId");
            CreateIndex("dbo.PriceLists", "ReturnOfficeId");
            AddForeignKey("dbo.PriceLists", "ReturnOfficeId", "dbo.Offices", "Id");
            AddForeignKey("dbo.PriceLists", "TakeOfficeId", "dbo.Offices", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PriceLists", "TakeOfficeId", "dbo.Offices");
            DropForeignKey("dbo.PriceLists", "ReturnOfficeId", "dbo.Offices");
            DropIndex("dbo.PriceLists", new[] { "ReturnOfficeId" });
            DropIndex("dbo.PriceLists", new[] { "TakeOfficeId" });
            DropColumn("dbo.PriceLists", "ReturnOfficeId");
            DropColumn("dbo.PriceLists", "TakeOfficeId");
        }
    }
}
