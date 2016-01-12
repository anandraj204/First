using System.Data.Entity.Migrations;

namespace Jane.Data.Migrations
{
    public partial class AddIsHiddenToDispensary : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dispensaries", "IsHidden", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Dispensaries", "IsHidden");
        }
    }
}
