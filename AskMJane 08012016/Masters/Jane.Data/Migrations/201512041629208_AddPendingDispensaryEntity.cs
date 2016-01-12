using System.Data.Entity.Migrations;

namespace Jane.Data.Migrations
{
    public partial class AddPendingDispensaryEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PendingDispensaries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        AddressId = c.Int(nullable: false),
                        Type = c.String(nullable: false),
                        PendingDispensaryStatus = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AddressId, cascadeDelete: true)
                .Index(t => t.AddressId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PendingDispensaries", "AddressId", "dbo.Addresses");
            DropIndex("dbo.PendingDispensaries", new[] { "AddressId" });
            DropTable("dbo.PendingDispensaries");
        }
    }
}
