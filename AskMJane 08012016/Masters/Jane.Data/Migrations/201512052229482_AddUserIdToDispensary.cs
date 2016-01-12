using System.Data.Entity.Migrations;

namespace Jane.Data.Migrations
{
    public partial class AddUserIdToDispensary : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dispensaries", "UserId", c => c.Int());
            CreateIndex("dbo.Dispensaries", "UserId");
            AddForeignKey("dbo.Dispensaries", "UserId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Dispensaries", "UserId", "dbo.Users");
            DropIndex("dbo.Dispensaries", new[] { "UserId" });
            DropColumn("dbo.Dispensaries", "UserId");
        }
    }
}
