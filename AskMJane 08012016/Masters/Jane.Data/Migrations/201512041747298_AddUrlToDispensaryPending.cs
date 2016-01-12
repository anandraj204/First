using System.Data.Entity.Migrations;

namespace Jane.Data.Migrations
{
    public partial class AddUrlToDispensaryPending : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PendingDispensaries", "Website", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PendingDispensaries", "Website");
        }
    }
}
