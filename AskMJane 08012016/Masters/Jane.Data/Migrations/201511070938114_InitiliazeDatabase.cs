using System.Data.Entity.Migrations;

namespace Jane.Data.Migrations
{
    public partial class InitiliazeDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address1 = c.String(nullable: false),
                        Address2 = c.String(),
                        City = c.String(nullable: false),
                        State = c.String(nullable: false),
                        Zip = c.String(nullable: false),
                        Country = c.String(nullable: false),
                        PhoneNumber = c.String(),
                        Latitude = c.Double(),
                        Longitude = c.Double(),
                        FormattedAddress = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Dispensaries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        EmailAddress = c.String(nullable: false),
                        HoursAndInfo = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        PhotoUrl = c.String(),
                        HasDelivery = c.Boolean(nullable: false),
                        HasPickup = c.Boolean(nullable: false),
                        HasScheduledDelivery = c.Boolean(nullable: false),
                        HasStorefront = c.Boolean(nullable: false),
                        IsCaregiver = c.Boolean(nullable: false),
                        IsPrivate = c.Boolean(nullable: false),
                        Slug = c.String(nullable: false),
                        LeaflySlug = c.String(),
                        Guid = c.String(),
                        Photos = c.String(),
                        HoursOfOperation = c.String(),
                        DeliveryZones = c.String(),
                        ApprovalZipCodes = c.String(),
                        DeliveryZipCodes = c.String(),
                        OnfleetMerchantId = c.String(),
                        AddressId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AddressId, cascadeDelete: true)
                .Index(t => t.AddressId);
            
            CreateTable(
                "dbo.DispensaryInvites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DispensaryInviteCode = c.String(nullable: false),
                        Name = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        UserId = c.Int(nullable: false),
                        DispensaryId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dispensaries", t => t.DispensaryId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.DispensaryId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Zipcode = c.String(),
                        Birthday = c.DateTime(),
                        LastLogin = c.DateTimeOffset(nullable: false, precision: 7),
                        OnfleetRecipientId = c.String(),
                        CurrentIp = c.String(),
                        LastIp = c.String(),
                        SignInCount = c.Int(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        Guid = c.String(nullable: false),
                        PatientInfoId = c.Int(),
                        AddressId = c.Int(),
                        DeliveryAddressId = c.Int(),
                        BillingAddressId = c.Int(),
                        WalletId = c.Int(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AddressId)
                .ForeignKey("dbo.Addresses", t => t.BillingAddressId)
                .ForeignKey("dbo.Addresses", t => t.DeliveryAddressId)
                .ForeignKey("dbo.PatientInfos", t => t.PatientInfoId)
                .ForeignKey("dbo.Wallets", t => t.WalletId)
                .Index(t => t.PatientInfoId)
                .Index(t => t.AddressId)
                .Index(t => t.DeliveryAddressId)
                .Index(t => t.BillingAddressId)
                .Index(t => t.WalletId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.DispensaryStaff",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JobRole = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        UserId = c.Int(nullable: false),
                        DispensaryId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dispensaries", t => t.DispensaryId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.DispensaryId);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PaymentType = c.Int(nullable: false),
                        DeliveryType = c.Int(nullable: false),
                        IsCheckedOut = c.Boolean(nullable: false),
                        IsConfirmed = c.Boolean(nullable: false),
                        IsReceived = c.Boolean(nullable: false),
                        OrderReferenceId = c.String(nullable: false),
                        OnfleetDestinationId = c.String(),
                        OnfleetTaskId = c.String(),
                        OnfleetTrackingURL = c.String(),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CheckedOutAt = c.DateTimeOffset(precision: 7),
                        DeliveryCharge = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DispensaryId = c.Int(),
                        BillingAddressId = c.Int(),
                        DeliveryAddressId = c.Int(),
                        UserId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.BillingAddressId)
                .ForeignKey("dbo.Addresses", t => t.DeliveryAddressId)
                .ForeignKey("dbo.Dispensaries", t => t.DispensaryId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.DispensaryId)
                .Index(t => t.BillingAddressId)
                .Index(t => t.DeliveryAddressId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.DispensaryProductVariantOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsPricedByWeight = c.Boolean(nullable: false),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DispensaryProductVariantId = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DispensaryProductVariants", t => t.DispensaryProductVariantId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.DispensaryProductVariantId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.DispensaryProductVariants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsPricedByWeight = c.Boolean(nullable: false),
                        VariantQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Slug = c.String(),
                        LeaflySlug = c.String(),
                        Guid = c.String(),
                        DisplayOrder = c.Int(nullable: false),
                        Photos = c.String(),
                        VariantAttributes = c.String(),
                        VariantPricing = c.String(nullable: false),
                        DispensaryProductId = c.Int(nullable: false),
                        IsMasterVariant = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DispensaryProducts", t => t.DispensaryProductId, cascadeDelete: false)
                .Index(t => t.DispensaryProductId);
            
            CreateTable(
                "dbo.DispensaryProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        IsAvailable = c.Boolean(nullable: false),
                        IsDiscounted = c.Boolean(nullable: false),
                        IsPopular = c.Boolean(nullable: false),
                        Slug = c.String(nullable: false),
                        LeaflySlug = c.String(),
                        Photos = c.String(),
                        ProductAttributes = c.String(),
                        ProductId = c.Int(nullable: false),
                        DispensaryId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dispensaries", t => t.DispensaryId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.DispensaryId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Slug = c.String(nullable: false),
                        LeaflySlug = c.String(),
                        Photos = c.String(),
                        Attributes = c.String(),
                        ProductCategoryId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductCategories", t => t.ProductCategoryId, cascadeDelete: true)
                .Index(t => t.ProductCategoryId);
            
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Color = c.String(nullable: false),
                        PhotoUrl = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PatientInfos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MedicalCardNumber = c.String(),
                        MedicalCardExpirationDate = c.DateTime(),
                        IsValidMedicalCard = c.Boolean(nullable: false),
                        MedicalCardValidationDate = c.DateTime(),
                        RecommendationImageUrl = c.String(),
                        DriversLicenseNumber = c.String(),
                        DriversLicenseImageUrl = c.String(),
                        IsApproved = c.Boolean(nullable: false),
                        PreferredWeight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ApprovalStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.UserSessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        SessionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sessions", t => t.SessionId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.SessionId);
            
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Token = c.String(nullable: false),
                        LastSeen = c.DateTimeOffset(nullable: false, precision: 7),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Wallets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Credit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionType = c.Int(nullable: false),
                        TransactionAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransactionCharge = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransactedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        WalletId = c.Int(nullable: false),
                        OrderId = c.Int(),
                        TransactionStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .ForeignKey("dbo.Wallets", t => t.WalletId, cascadeDelete: true)
                .Index(t => t.WalletId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.Effects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductReviewEffects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EffectId = c.Int(nullable: false),
                        ProductReviewId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Effects", t => t.EffectId, cascadeDelete: true)
                .ForeignKey("dbo.ProductReviews", t => t.ProductReviewId, cascadeDelete: true)
                .Index(t => t.EffectId)
                .Index(t => t.ProductReviewId);
            
            CreateTable(
                "dbo.ProductReviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Review = c.String(nullable: false),
                        Rating = c.Int(nullable: false),
                        ReviewedType = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        DispensaryProductId = c.Int(nullable: false),
                        PurchasedFromDispensaryId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dispensaries", t => t.PurchasedFromDispensaryId, cascadeDelete: true)
                .ForeignKey("dbo.DispensaryProducts", t => t.DispensaryProductId, cascadeDelete: false)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ProductId)
                .Index(t => t.DispensaryProductId)
                .Index(t => t.PurchasedFromDispensaryId);
            
            CreateTable(
                "dbo.Flavors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Invites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InviteCode = c.String(),
                        InviterCredit = c.Decimal(precision: 18, scale: 2),
                        InviteeCredit = c.Decimal(precision: 18, scale: 2),
                        InviterId = c.Int(nullable: false),
                        InviteeId = c.Int(),
                        InviteeName = c.String(),
                        InviteeEmail = c.String(),
                        InviteePhone = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.InviteeId)
                .ForeignKey("dbo.Users", t => t.InviterId, cascadeDelete: true)
                .Index(t => t.InviterId)
                .Index(t => t.InviteeId);
            
            CreateTable(
                "dbo.ProductReviewFlavors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductReviewId = c.Int(nullable: false),
                        FlavorId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Flavors", t => t.FlavorId, cascadeDelete: true)
                .ForeignKey("dbo.ProductReviews", t => t.ProductReviewId, cascadeDelete: true)
                .Index(t => t.ProductReviewId)
                .Index(t => t.FlavorId);
            
            CreateTable(
                "dbo.OAuthClients",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        ClientSecretHash = c.String(),
                        AllowedGrant = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductReviewFlavors", "ProductReviewId", "dbo.ProductReviews");
            DropForeignKey("dbo.ProductReviewFlavors", "FlavorId", "dbo.Flavors");
            DropForeignKey("dbo.Invites", "InviterId", "dbo.Users");
            DropForeignKey("dbo.Invites", "InviteeId", "dbo.Users");
            DropForeignKey("dbo.ProductReviewEffects", "ProductReviewId", "dbo.ProductReviews");
            DropForeignKey("dbo.ProductReviews", "UserId", "dbo.Users");
            DropForeignKey("dbo.ProductReviews", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductReviews", "DispensaryProductId", "dbo.DispensaryProducts");
            DropForeignKey("dbo.ProductReviews", "PurchasedFromDispensaryId", "dbo.Dispensaries");
            DropForeignKey("dbo.ProductReviewEffects", "EffectId", "dbo.Effects");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.DispensaryInvites", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "WalletId", "dbo.Wallets");
            DropForeignKey("dbo.Transactions", "WalletId", "dbo.Wallets");
            DropForeignKey("dbo.Transactions", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.UserSessions", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserSessions", "SessionId", "dbo.Sessions");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "PatientInfoId", "dbo.PatientInfos");
            DropForeignKey("dbo.Orders", "UserId", "dbo.Users");
            DropForeignKey("dbo.DispensaryProductVariantOrders", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.DispensaryProductVariantOrders", "DispensaryProductVariantId", "dbo.DispensaryProductVariants");
            DropForeignKey("dbo.Products", "ProductCategoryId", "dbo.ProductCategories");
            DropForeignKey("dbo.DispensaryProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.DispensaryProductVariants", "DispensaryProductId", "dbo.DispensaryProducts");
            DropForeignKey("dbo.DispensaryProducts", "DispensaryId", "dbo.Dispensaries");
            DropForeignKey("dbo.Orders", "DispensaryId", "dbo.Dispensaries");
            DropForeignKey("dbo.Orders", "DeliveryAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Orders", "BillingAddressId", "dbo.Addresses");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.DispensaryStaff", "UserId", "dbo.Users");
            DropForeignKey("dbo.DispensaryStaff", "DispensaryId", "dbo.Dispensaries");
            DropForeignKey("dbo.Users", "DeliveryAddressId", "dbo.Addresses");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "BillingAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Users", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.DispensaryInvites", "DispensaryId", "dbo.Dispensaries");
            DropForeignKey("dbo.Dispensaries", "AddressId", "dbo.Addresses");
            DropIndex("dbo.ProductReviewFlavors", new[] { "FlavorId" });
            DropIndex("dbo.ProductReviewFlavors", new[] { "ProductReviewId" });
            DropIndex("dbo.Invites", new[] { "InviteeId" });
            DropIndex("dbo.Invites", new[] { "InviterId" });
            DropIndex("dbo.ProductReviews", new[] { "PurchasedFromDispensaryId" });
            DropIndex("dbo.ProductReviews", new[] { "DispensaryProductId" });
            DropIndex("dbo.ProductReviews", new[] { "ProductId" });
            DropIndex("dbo.ProductReviews", new[] { "UserId" });
            DropIndex("dbo.ProductReviewEffects", new[] { "ProductReviewId" });
            DropIndex("dbo.ProductReviewEffects", new[] { "EffectId" });
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.Transactions", new[] { "OrderId" });
            DropIndex("dbo.Transactions", new[] { "WalletId" });
            DropIndex("dbo.UserSessions", new[] { "SessionId" });
            DropIndex("dbo.UserSessions", new[] { "UserId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.Products", new[] { "ProductCategoryId" });
            DropIndex("dbo.DispensaryProducts", new[] { "DispensaryId" });
            DropIndex("dbo.DispensaryProducts", new[] { "ProductId" });
            DropIndex("dbo.DispensaryProductVariants", new[] { "DispensaryProductId" });
            DropIndex("dbo.DispensaryProductVariantOrders", new[] { "OrderId" });
            DropIndex("dbo.DispensaryProductVariantOrders", new[] { "DispensaryProductVariantId" });
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropIndex("dbo.Orders", new[] { "DeliveryAddressId" });
            DropIndex("dbo.Orders", new[] { "BillingAddressId" });
            DropIndex("dbo.Orders", new[] { "DispensaryId" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.DispensaryStaff", new[] { "DispensaryId" });
            DropIndex("dbo.DispensaryStaff", new[] { "UserId" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.Users", new[] { "WalletId" });
            DropIndex("dbo.Users", new[] { "BillingAddressId" });
            DropIndex("dbo.Users", new[] { "DeliveryAddressId" });
            DropIndex("dbo.Users", new[] { "AddressId" });
            DropIndex("dbo.Users", new[] { "PatientInfoId" });
            DropIndex("dbo.DispensaryInvites", new[] { "DispensaryId" });
            DropIndex("dbo.DispensaryInvites", new[] { "UserId" });
            DropIndex("dbo.Dispensaries", new[] { "AddressId" });
            DropTable("dbo.OAuthClients");
            DropTable("dbo.ProductReviewFlavors");
            DropTable("dbo.Invites");
            DropTable("dbo.Flavors");
            DropTable("dbo.ProductReviews");
            DropTable("dbo.ProductReviewEffects");
            DropTable("dbo.Effects");
            DropTable("dbo.Roles");
            DropTable("dbo.Transactions");
            DropTable("dbo.Wallets");
            DropTable("dbo.Sessions");
            DropTable("dbo.UserSessions");
            DropTable("dbo.UserRoles");
            DropTable("dbo.PatientInfos");
            DropTable("dbo.ProductCategories");
            DropTable("dbo.Products");
            DropTable("dbo.DispensaryProducts");
            DropTable("dbo.DispensaryProductVariants");
            DropTable("dbo.DispensaryProductVariantOrders");
            DropTable("dbo.Orders");
            DropTable("dbo.UserLogins");
            DropTable("dbo.DispensaryStaff");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.DispensaryInvites");
            DropTable("dbo.Dispensaries");
            DropTable("dbo.Addresses");
        }
    }
}
