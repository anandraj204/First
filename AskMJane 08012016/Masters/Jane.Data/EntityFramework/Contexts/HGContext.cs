using System.Data.Entity;
using Jane.Data.EntityFramework.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Jane.Data.EntityFramework.Contexts
{
    public class HGContext : IdentityDbContext<User, Role, int, 
        UserLogin, UserRole, UserClaim>
    {
        public HGContext()
            : base("DefaultConnection")
        {
            
        }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Dispensary> Dispensaries { get; set; }
        public DbSet<Symptom> Symptoms { get; set; }
        public DbSet<DispensaryProduct> DispensaryProducts { get; set; }
        public DbSet<DispensaryProductVariant> DispensaryProductVariants { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Effect> Effects { get; set; }
        public DbSet<ZipCode> ZipCodes { get; set; }
        public DbSet<File> Files { get; set; }


        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<DispensaryProductVariantOrder> DispensaryProductVariantOrders { get; set; }
        public DbSet<PatientInfo> PatientInfos { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<PendingDispensary> PendingDispensaries { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Address>();
            modelBuilder.Entity<Dispensary>();
            modelBuilder.Entity<DispensaryInvite>();;
            modelBuilder.Entity<DispensaryProduct>();
            modelBuilder.Entity<DispensaryProduct>().HasMany(p => p.Photos).WithMany();
            modelBuilder.Entity<DispensaryProduct>().HasMany(p => p.Effects).WithMany();
            modelBuilder.Entity<DispensaryProduct>().HasMany(p => p.Symptoms).WithMany();
            modelBuilder.Entity<DispensaryProductVariant>().HasMany(p => p.Photos).WithMany();
            modelBuilder.Entity<DispensaryProductVariantOrder>();
            modelBuilder.Entity<DispensaryStaff>().ToTable("DispensaryStaff");
            modelBuilder.Entity<File>().ToTable("Files");
            modelBuilder.Entity<Effect>();
            modelBuilder.Entity<Flavor>();
            modelBuilder.Entity<Invite>();
            modelBuilder.Entity<Order>();
            modelBuilder.Entity<PatientInfo>().ToTable("PatientInfos");
            modelBuilder.Entity<Product>().HasMany(p => p.Effects).WithMany();
            modelBuilder.Entity<Product>().HasMany(p => p.Symptoms).WithMany();
            modelBuilder.Entity<Product>().HasMany(p => p.Photos).WithMany();
            modelBuilder.Entity<ProductCategory>();
            modelBuilder.Entity<ProductReview>();
            modelBuilder.Entity<ProductReviewEffect>();
            modelBuilder.Entity<ProductReviewFlavor>();
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<UserRole>().ToTable("UserRoles");
            modelBuilder.Entity<UserSession>();
            modelBuilder.Entity<Session>();
            modelBuilder.Entity<PendingDispensary>();
            modelBuilder.Entity<Dispensary>().HasMany(p => p.ApprovalZipCodes).WithMany().Map(t =>
            {
                t.ToTable("DispensaryApprovalZipCodes");
                t.MapLeftKey("Dispensary_Id");
                t.MapRightKey("ApprovalZip_Id");
            });
            modelBuilder.Entity<Dispensary>().HasMany(p => p.DeliveryZipCodes).WithMany().Map(t =>
            {
                t.ToTable("DispensaryDeliveryZipCodes");
                t.MapLeftKey("Dispensary_Id");
                t.MapRightKey("DeliveryZip_Id");
            });




            modelBuilder.Entity<OAuthClient>();



        }
        public static HGContext Create()
        {
            return new HGContext();
        }
    }
}
