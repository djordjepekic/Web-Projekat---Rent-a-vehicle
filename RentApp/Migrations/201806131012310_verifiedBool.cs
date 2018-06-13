namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class verifiedBool : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Services", "Verified", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Services", "Verified", c => c.String());
        }
    }
}
