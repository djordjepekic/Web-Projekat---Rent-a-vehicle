namespace RentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletedOfficeName : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Offices", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Offices", "Name", c => c.String());
        }
    }
}
