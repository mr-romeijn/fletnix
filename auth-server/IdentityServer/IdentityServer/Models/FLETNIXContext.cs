using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace fletnix.Models
{
    public partial class FLETNIXContext : DbContext
    {
        private IConfigurationRoot _config;

        public virtual DbSet<Customer> Customer { get; set; }


        public FLETNIXContext(IConfigurationRoot config, DbContextOptions<FLETNIXContext> options) : base(options)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(_config["Database:Fletnix"]);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerMailAddress)
                    .HasName("PK_Customer");

                entity.HasIndex(e => e.PaypalAccount)
                    .HasName("AK_Customer_Paypal")
                    .IsUnique();

                entity.Property(e => e.CustomerMailAddress)
                    .HasColumnName("customer_mail_address")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasColumnName("country_name")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PaypalAccount)
                    .IsRequired()
                    .HasColumnName("paypal_account")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.SubscriptionEnd)
                    .HasColumnName("subscription_end")
                    .HasColumnType("date");

                entity.Property(e => e.SubscriptionStart)
                    .HasColumnName("subscription_start")
                    .HasColumnType("date");

            });


        }
    }
}