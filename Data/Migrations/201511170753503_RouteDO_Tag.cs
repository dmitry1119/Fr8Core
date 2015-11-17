namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RouteDO_Tag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Routes", "Tag", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Routes", "Tag");
        }
    }
}
