using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace fletnix.Models
{
    public partial class FLETNIXContext : DbContext
    {
        public virtual DbSet<Award> Award { get; set; }
        public virtual DbSet<AwardType> AwardType { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<Genre> Genre { get; set; }
        public virtual DbSet<Movie> Movie { get; set; }
        public virtual DbSet<MovieAward> MovieAward { get; set; }
        public virtual DbSet<MovieCast> MovieCast { get; set; }
        public virtual DbSet<MovieDirector> MovieDirector { get; set; }
        public virtual DbSet<MovieGenre> MovieGenre { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Watchhistory> Watchhistory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(@"Server=tcp:fletnix.database.windows.net,1433;Initial Catalog=FLETNIX;Persist Security Info=False;User ID=nromeijn;Password=qQopoXAVQJyqzULeafG3P;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Award>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("pk_Award");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<AwardType>(entity =>
            {
                entity.HasKey(e => new { e.Name, e.Type })
                    .HasName("pk_AwardType");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.CountryName)
                    .HasName("PK_Country");

                entity.Property(e => e.CountryName)
                    .HasColumnName("country_name")
                    .HasColumnType("varchar(50)");
            });

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

                entity.HasOne(d => d.CountryNameNavigation)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.CountryName)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_country");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => new { e.Name, e.Year })
                    .HasName("pk_Event");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Year).HasColumnName("year");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasColumnName("location")
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.LocationNavigation)
                    .WithMany(p => p.Event)
                    .HasForeignKey(d => d.Location)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_Event_Country");

                entity.HasOne(d => d.NameNavigation)
                    .WithMany(p => p.Event)
                    .HasForeignKey(d => d.Name)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_Event_Award");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(e => e.GenreName)
                    .HasName("PK_Genre_1");

                entity.Property(e => e.GenreName)
                    .HasColumnName("genre_name")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.Property(e => e.MovieId)
                    .HasColumnName("movie_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CoverImage)
                    .HasColumnName("cover_image")
                    .HasColumnType("image");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.PreviousPart).HasColumnName("previous_part");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("numeric");

                entity.Property(e => e.PublicationYear).HasColumnName("publication_year");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Url)
                    .HasColumnName("URL")
                    .HasColumnType("varchar(255)");

                entity.HasOne(d => d.PreviousPartNavigation)
                    .WithMany(p => p.InversePreviousPartNavigation)
                    .HasForeignKey(d => d.PreviousPart)
                    .HasConstraintName("FK_previous_part");
            });

            modelBuilder.Entity<MovieAward>(entity =>
            {
                entity.HasKey(e => new { e.Name, e.Year, e.Type, e.MovieId, e.PersonId })
                    .HasName("pk_MovieAward");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Year).HasColumnName("year");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.MovieId).HasColumnName("movie_id");

                entity.Property(e => e.PersonId).HasColumnName("person_id");

                entity.Property(e => e.Result)
                    .IsRequired()
                    .HasColumnName("result")
                    .HasColumnType("varchar(10)");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.MovieAward)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_Event_Movie");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.MovieAward)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_Event_Person");

                entity.HasOne(d => d.AwardType)
                    .WithMany(p => p.MovieAward)
                    .HasForeignKey(d => new { d.Name, d.Type })
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_Event_AwardType");
            });

            modelBuilder.Entity<MovieCast>(entity =>
            {
                entity.HasKey(e => new { e.MovieId, e.PersonId, e.Role })
                    .HasName("PK_Movie_Cast");

                entity.ToTable("Movie_Cast");

                entity.Property(e => e.MovieId).HasColumnName("movie_id");

                entity.Property(e => e.PersonId).HasColumnName("person_id");

                entity.Property(e => e.Role)
                    .HasColumnName("role")
                    .HasColumnType("varchar(255)");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.MovieCast)
                    .HasForeignKey(d => d.MovieId)
                    .HasConstraintName("FK2_movie_id");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.MovieCast)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK2_person_id");
            });

            modelBuilder.Entity<MovieDirector>(entity =>
            {
                entity.HasKey(e => new { e.MovieId, e.PersonId })
                    .HasName("PK_Movie_Directors");

                entity.ToTable("Movie_Director");

                entity.Property(e => e.MovieId).HasColumnName("movie_id");

                entity.Property(e => e.PersonId).HasColumnName("person_id");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.MovieDirector)
                    .HasForeignKey(d => d.MovieId)
                    .HasConstraintName("FK_movie_id");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.MovieDirector)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK_person_id");
            });

            modelBuilder.Entity<MovieGenre>(entity =>
            {
                entity.HasKey(e => new { e.MovieId, e.GenreName })
                    .HasName("PK_Movie_Genre");

                entity.ToTable("Movie_Genre");

                entity.Property(e => e.MovieId).HasColumnName("movie_id");

                entity.Property(e => e.GenreName)
                    .HasColumnName("genre_name")
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.GenreNameNavigation)
                    .WithMany(p => p.MovieGenre)
                    .HasForeignKey(d => d.GenreName)
                    .HasConstraintName("FK_genre");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.MovieGenre)
                    .HasForeignKey(d => d.MovieId)
                    .HasConstraintName("FK3_movie_id");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.PersonId)
                    .HasColumnName("person_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasColumnName("firstname")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasColumnType("char(1)");

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasColumnName("lastname")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Watchhistory>(entity =>
            {
                entity.HasKey(e => new { e.MovieId, e.CustomerMailAddress, e.WatchDate })
                    .HasName("PK_Movie_Watchhistory_1");

                entity.Property(e => e.MovieId).HasColumnName("movie_id");

                entity.Property(e => e.CustomerMailAddress)
                    .HasColumnName("customer_mail_address")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.WatchDate)
                    .HasColumnName("watch_date")
                    .HasColumnType("date");

                entity.Property(e => e.Invoiced).HasColumnName("invoiced");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("numeric");

                entity.HasOne(d => d.CustomerMailAddressNavigation)
                    .WithMany(p => p.Watchhistory)
                    .HasForeignKey(d => d.CustomerMailAddress)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_customer");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.Watchhistory)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK4_movie_id");
            });
        }
    }
}