using System.Data.Entity;

namespace Harvestgeek.Data.EntityFramework.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<backer_infos> backer_infos { get; set; }
        public virtual DbSet<betaregistration> betaregistrations { get; set; }
        public virtual DbSet<chart_configs> chart_configs { get; set; }
        public virtual DbSet<claim> claims { get; set; }
        public virtual DbSet<claims_products> claims_products { get; set; }
        public virtual DbSet<dashboard> dashboards { get; set; }
        public virtual DbSet<kickstarter_reward_levels> kickstarter_reward_levels { get; set; }
        public virtual DbSet<rewards_products> rewards_products { get; set; }
        public virtual DbSet<role> roles { get; set; }
        public virtual DbSet<sensor> sensors { get; set; }
        public virtual DbSet<series> series { get; set; }
        public virtual DbSet<series_data> series_data { get; set; }
        public virtual DbSet<series_tags> series_tags { get; set; }
        public virtual DbSet<session> sessions { get; set; }
        public virtual DbSet<spree_addresses> spree_addresses { get; set; }
        public virtual DbSet<spree_countries> spree_countries { get; set; }
        public virtual DbSet<spree_products> spree_products { get; set; }
        public virtual DbSet<spree_states> spree_states { get; set; }
        public virtual DbSet<station_addresses> station_addresses { get; set; }
        public virtual DbSet<station_groups> station_groups { get; set; }
        public virtual DbSet<station_station_groups> station_station_groups { get; set; }
        public virtual DbSet<station_type_sensors> station_type_sensors { get; set; }
        public virtual DbSet<station_types> station_types { get; set; }
        public virtual DbSet<station> stations { get; set; }
        public virtual DbSet<stations_tags> stations_tags { get; set; }
        public virtual DbSet<tag> tags { get; set; }
        public virtual DbSet<user> users { get; set; }
        public virtual DbSet<users_roles> users_roles { get; set; }
        public virtual DbSet<widget> widgets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<backer_infos>()
                .Property(e => e.amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<dashboard>()
                .HasMany(e => e.widgets)
                .WithRequired(e => e.dashboard)
                .HasForeignKey(e => e.dashboard_id);

            modelBuilder.Entity<kickstarter_reward_levels>()
                .Property(e => e.price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<role>()
                .HasMany(e => e.users_roles)
                .WithRequired(e => e.role)
                .HasForeignKey(e => e.role_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sensor>()
                .HasMany(e => e.series)
                .WithRequired(e => e.sensor)
                .HasForeignKey(e => e.sensor_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sensor>()
                .HasMany(e => e.station_type_sensors)
                .WithRequired(e => e.sensor)
                .HasForeignKey(e => e.sensor_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<series>()
                .HasMany(e => e.series_tags)
                .WithRequired(e => e.series)
                .HasForeignKey(e => e.series_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<series_data>()
                .Property(e => e.last)
                
                .HasPrecision(10, 2);

            modelBuilder.Entity<series_data>()
                .Property(e => e.min)
                .HasPrecision(10, 2);

            modelBuilder.Entity<series_data>()
                .Property(e => e.max)
                .HasPrecision(10, 2);

            modelBuilder.Entity<series_data>()
                .Property(e => e.sum)
                .HasPrecision(10, 2);

            modelBuilder.Entity<station_groups>()
                .HasMany(e => e.station_station_groups)
                .WithRequired(e => e.station_groups)
                .HasForeignKey(e => e.station_group_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<station_types>()
                .HasMany(e => e.station_type_sensors)
                .WithRequired(e => e.station_types)
                .HasForeignKey(e => e.station_type_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<station_types>()
                .HasMany(e => e.stations)
                .WithRequired(e => e.station_types)
                .HasForeignKey(e => e.station_type_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<station>()
                .HasMany(e => e.series)
                .WithRequired(e => e.station)
                .HasForeignKey(e => e.station_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<station>()
                .HasMany(e => e.station_addresses)
                .WithRequired(e => e.station)
                .HasForeignKey(e => e.station_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<station>()
                .HasMany(e => e.station_station_groups)
                .WithRequired(e => e.station)
                .HasForeignKey(e => e.station_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<station>()
                .HasMany(e => e.stations_tags)
                .WithRequired(e => e.station)
                .HasForeignKey(e => e.station_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tag>()
                .HasMany(e => e.stations_tags)
                .WithRequired(e => e.tag)
                .HasForeignKey(e => e.tag_id);

            modelBuilder.Entity<user>()
                .HasMany(e => e.dashboards)
                .WithRequired(e => e.user)
                .HasForeignKey(e => e.user_id);

            modelBuilder.Entity<user>()
                .HasMany(e => e.stations)
                .WithOptional(e => e.user)
                .HasForeignKey(e => e.user_id);

            modelBuilder.Entity<user>()
                .HasMany(e => e.users_roles)
                .WithRequired(e => e.user)
                .HasForeignKey(e => e.user_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<user>()
                .HasMany(e => e.widgets)
                .WithRequired(e => e.user)
                .HasForeignKey(e => e.user_id);

            modelBuilder.Entity<widget>()
                .HasMany(e => e.chart_configs)
                .WithRequired(e => e.widget)
                .HasForeignKey(e => e.widget_id);
        }
    }
}
