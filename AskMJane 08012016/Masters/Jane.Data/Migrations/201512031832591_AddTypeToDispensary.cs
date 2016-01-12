using System.Data.Entity.Migrations;

namespace Jane.Data.Migrations
{
    public partial class AddTypeToDispensary : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dispensaries", "Type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "YouTubeVideoUrl");
        }
    }
}
