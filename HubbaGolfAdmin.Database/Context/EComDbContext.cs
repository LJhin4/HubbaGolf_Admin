using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;
using HubbaGolfAdmin.Database.Models;

namespace HubbaGolfAdmin.Database
{
    public partial class EComDbContext : DbContext
    {
        public EComDbContext() : base()
        {

        }
        public EComDbContext(DbContextOptions<EComDbContext> options)
        : base(options)
        {

        }
        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Pricing> Pricings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Events>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Events__3214EC07B4EEFB49");

                entity.Property(e => e.Title).HasMaxLength(255);
                entity.Property(e => e.Description).HasMaxLength(5000);
                entity.Property(e => e.StartDate).HasColumnType("datetime");
                entity.Property(e => e.EndDate).HasColumnType("datetime");
                entity.Property(e => e.Location).HasMaxLength(255);
                entity.Property(e => e.IsAllDay).HasDefaultValue(false);
                entity.Property(e => e.StatusRecord).HasColumnType("int");
                entity.Property(e => e.Name).HasMaxLength(255);
                entity.Property(e => e.Phone).HasMaxLength(255);
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.PlayerNumber).HasColumnType("int");
                entity.Property(e => e.Country).HasMaxLength(255);
                entity.Property(e => e.Course).HasMaxLength(255);
                entity.Property(e => e.OrderNumber).HasMaxLength(10);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.CreatedBy).HasMaxLength(50);
                entity.Property(e => e.CreatedName).HasMaxLength(250);
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.DisplayName).HasMaxLength(500);
                entity.Property(e => e.Email).HasMaxLength(250);
                entity.Property(e => e.LastLoginOn).HasColumnType("datetime");
                entity.Property(e => e.ModifiedBy).HasMaxLength(50);
                entity.Property(e => e.ModifiedName).HasMaxLength(250);
                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
                entity.Property(e => e.Password).HasMaxLength(500);
                entity.Property(e => e.Phone).HasMaxLength(50);
                entity.Property(e => e.UserName).HasMaxLength(500);
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("Menu");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Action).HasMaxLength(500);
                entity.Property(e => e.Controller).HasMaxLength(500);
                entity.Property(e => e.CreatedBy).HasMaxLength(50);
                entity.Property(e => e.CreatedName).HasMaxLength(250);
                entity.Property(e => e.CreatedOn)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.GroupMenu).HasMaxLength(254);
                entity.Property(e => e.Icon).HasMaxLength(500);
                entity.Property(e => e.Link).HasMaxLength(250);
                entity.Property(e => e.Location).HasMaxLength(254);
                entity.Property(e => e.ModifiedBy).HasMaxLength(50);
                entity.Property(e => e.ModifiedName).HasMaxLength(250);
                entity.Property(e => e.ModifiedOn)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Name).HasMaxLength(500);
                entity.Property(e => e.Params).HasMaxLength(254);
                entity.Property(e => e.RecordStatus).HasDefaultValue(0);
                entity.Property(e => e.Section).HasMaxLength(254);
                entity.Property(e => e.Slug).HasDefaultValue(0);
                entity.Property(e => e.Sort).HasDefaultValue(1);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.CreatedBy).HasMaxLength(50);
                entity.Property(e => e.CreatedName).HasMaxLength(250);
                entity.Property(e => e.CreatedOn)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(250);
                entity.Property(e => e.MenuId).HasColumnName("MenuID");
                entity.Property(e => e.ModifiedBy).HasMaxLength(50);
                entity.Property(e => e.ModifiedName).HasMaxLength(250);
                entity.Property(e => e.ModifiedOn)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Name).HasMaxLength(500);
                entity.Property(e => e.Type).HasMaxLength(100);
            });

            modelBuilder.Entity<Article>(entity =>
            {
                entity.ToTable("Article");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Author).HasMaxLength(100);
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
                entity.Property(e => e.CreatedBy).HasMaxLength(50);
                entity.Property(e => e.CreatedName).HasMaxLength(250);
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");
                entity.Property(e => e.Date).HasColumnType("datetime");
                entity.Property(e => e.DateExp).HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Icon).HasMaxLength(250);
                entity.Property(e => e.MenuId).HasColumnName("MenuID");
                entity.Property(e => e.ModifiedBy).HasMaxLength(50);
                entity.Property(e => e.ModifiedName).HasMaxLength(250);
                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<Pricing>(entity =>
            {
                entity.ToTable("Pricing");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.ArticleId).HasColumnName("ArticleID");
                entity.Property(e => e.CreatedBy).HasMaxLength(50);
                entity.Property(e => e.CreatedName).HasMaxLength(250);
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedBy).HasMaxLength(50);
                entity.Property(e => e.ModifiedName).HasMaxLength(250);
                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
                entity.Property(e => e.Price).HasColumnType("decimal(19, 2)");
                entity.Property(e => e.PricingType).HasMaxLength(250);
                entity.Property(e => e.SpecificDate).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
