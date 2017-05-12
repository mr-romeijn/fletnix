using System;
using fletnix.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace fletnix.Data.Migrations.Identity
{
    [DbContext(typeof(FLETNIXContext))]
    partial class FLETNIXContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("fletnix.Models.Auth.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("fletnix.Models.Award", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("name")
                        .HasColumnType("varchar(50)");

                    b.HasKey("Name")
                        .HasName("pk_Award");

                    b.ToTable("Award");
                });

            modelBuilder.Entity("fletnix.Models.AwardType", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Type")
                        .HasColumnName("type")
                        .HasColumnType("varchar(50)");

                    b.HasKey("Name", "Type")
                        .HasName("pk_AwardType");

                    b.ToTable("AwardType");
                });

            modelBuilder.Entity("fletnix.Models.Country", b =>
                {
                    b.Property<string>("CountryName")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("country_name")
                        .HasColumnType("varchar(50)");

                    b.HasKey("CountryName")
                        .HasName("PK_Country");

                    b.ToTable("Country");
                });

            modelBuilder.Entity("fletnix.Models.Customer", b =>
                {
                    b.Property<string>("CustomerMailAddress")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("customer_mail_address")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CountryName")
                        .IsRequired()
                        .HasColumnName("country_name")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnName("password")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("PaypalAccount")
                        .IsRequired()
                        .HasColumnName("paypal_account")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("SubscriptionEnd")
                        .HasColumnName("subscription_end")
                        .HasColumnType("date");

                    b.Property<DateTime>("SubscriptionStart")
                        .HasColumnName("subscription_start")
                        .HasColumnType("date");

                    b.HasKey("CustomerMailAddress")
                        .HasName("PK_Customer");

                    b.HasIndex("CountryName");

                    b.HasIndex("PaypalAccount")
                        .IsUnique()
                        .HasName("AK_Customer_Paypal");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("fletnix.Models.Event", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("varchar(50)");

                    b.Property<int>("Year")
                        .HasColumnName("year");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnName("location")
                        .HasColumnType("varchar(50)");

                    b.HasKey("Name", "Year")
                        .HasName("pk_Event");

                    b.HasIndex("Location");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("fletnix.Models.Genre", b =>
                {
                    b.Property<string>("GenreName")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("genre_name")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Description")
                        .HasColumnName("description")
                        .HasColumnType("varchar(255)");

                    b.HasKey("GenreName")
                        .HasName("PK_Genre_1");

                    b.ToTable("Genre");
                });

            modelBuilder.Entity("fletnix.Models.Movie", b =>
                {
                    b.Property<int>("MovieId")
                        .HasColumnName("movie_id");

                    b.Property<byte[]>("CoverImage")
                        .HasColumnName("cover_image")
                        .HasColumnType("image");

                    b.Property<string>("Description")
                        .HasColumnName("description")
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("Duration")
                        .HasColumnName("duration");

                    b.Property<int?>("PreviousPart")
                        .HasColumnName("previous_part");

                    b.Property<decimal>("Price")
                        .HasColumnName("price")
                        .HasColumnType("numeric");

                    b.Property<int?>("PublicationYear")
                        .HasColumnName("publication_year");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("title")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Url")
                        .HasColumnName("URL")
                        .HasColumnType("varchar(255)");

                    b.HasKey("MovieId");

                    b.HasIndex("PreviousPart");

                    b.ToTable("Movie");
                });

            modelBuilder.Entity("fletnix.Models.MovieAward", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("varchar(50)");

                    b.Property<int>("Year")
                        .HasColumnName("year");

                    b.Property<string>("Type")
                        .HasColumnName("type")
                        .HasColumnType("varchar(50)");

                    b.Property<int>("MovieId")
                        .HasColumnName("movie_id");

                    b.Property<int>("PersonId")
                        .HasColumnName("person_id");

                    b.Property<string>("Result")
                        .IsRequired()
                        .HasColumnName("result")
                        .HasColumnType("varchar(10)");

                    b.HasKey("Name", "Year", "Type", "MovieId", "PersonId")
                        .HasName("pk_MovieAward");

                    b.HasIndex("MovieId");

                    b.HasIndex("PersonId");

                    b.HasIndex("Name", "Type");

                    b.ToTable("MovieAward");
                });

            modelBuilder.Entity("fletnix.Models.MovieCast", b =>
                {
                    b.Property<int>("MovieId")
                        .HasColumnName("movie_id");

                    b.Property<int>("PersonId")
                        .HasColumnName("person_id");

                    b.Property<string>("Role")
                        .HasColumnName("role")
                        .HasColumnType("varchar(255)");

                    b.HasKey("MovieId", "PersonId", "Role")
                        .HasName("PK_Movie_Cast");

                    b.HasIndex("PersonId");

                    b.ToTable("Movie_Cast");
                });

            modelBuilder.Entity("fletnix.Models.MovieDirector", b =>
                {
                    b.Property<int>("MovieId")
                        .HasColumnName("movie_id");

                    b.Property<int>("PersonId")
                        .HasColumnName("person_id");

                    b.HasKey("MovieId", "PersonId")
                        .HasName("PK_Movie_Directors");

                    b.HasIndex("PersonId");

                    b.ToTable("Movie_Director");
                });

            modelBuilder.Entity("fletnix.Models.MovieGenre", b =>
                {
                    b.Property<int>("MovieId")
                        .HasColumnName("movie_id");

                    b.Property<string>("GenreName")
                        .HasColumnName("genre_name")
                        .HasColumnType("varchar(50)");

                    b.HasKey("MovieId", "GenreName")
                        .HasName("PK_Movie_Genre");

                    b.HasIndex("GenreName");

                    b.ToTable("Movie_Genre");
                });

            modelBuilder.Entity("fletnix.Models.Person", b =>
                {
                    b.Property<int>("PersonId")
                        .HasColumnName("person_id");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnName("firstname")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Gender")
                        .HasColumnName("gender")
                        .HasColumnType("char(1)");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnName("lastname")
                        .HasColumnType("varchar(50)");

                    b.HasKey("PersonId");

                    b.ToTable("Person");
                });

            modelBuilder.Entity("fletnix.Models.Watchhistory", b =>
                {
                    b.Property<int>("MovieId")
                        .HasColumnName("movie_id");

                    b.Property<string>("CustomerMailAddress")
                        .HasColumnName("customer_mail_address")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("WatchDate")
                        .HasColumnName("watch_date")
                        .HasColumnType("date");

                    b.Property<bool>("Invoiced")
                        .HasColumnName("invoiced");

                    b.Property<decimal>("Price")
                        .HasColumnName("price")
                        .HasColumnType("numeric");

                    b.HasKey("MovieId", "CustomerMailAddress", "WatchDate")
                        .HasName("PK_Movie_Watchhistory_1");

                    b.HasIndex("CustomerMailAddress");

                    b.ToTable("Watchhistory");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("fletnix.Models.Customer", b =>
                {
                    b.HasOne("fletnix.Models.Country", "CountryNameNavigation")
                        .WithMany("Customer")
                        .HasForeignKey("CountryName")
                        .HasConstraintName("FK_country");
                });

            modelBuilder.Entity("fletnix.Models.Event", b =>
                {
                    b.HasOne("fletnix.Models.Country", "LocationNavigation")
                        .WithMany("Event")
                        .HasForeignKey("Location")
                        .HasConstraintName("fk_Event_Country");

                    b.HasOne("fletnix.Models.Award", "NameNavigation")
                        .WithMany("Event")
                        .HasForeignKey("Name")
                        .HasConstraintName("fk_Event_Award");
                });

            modelBuilder.Entity("fletnix.Models.Movie", b =>
                {
                    b.HasOne("fletnix.Models.Movie", "PreviousPartNavigation")
                        .WithMany("InversePreviousPartNavigation")
                        .HasForeignKey("PreviousPart")
                        .HasConstraintName("FK_previous_part");
                });

            modelBuilder.Entity("fletnix.Models.MovieAward", b =>
                {
                    b.HasOne("fletnix.Models.Movie", "Movie")
                        .WithMany("MovieAward")
                        .HasForeignKey("MovieId")
                        .HasConstraintName("fk_Event_Movie");

                    b.HasOne("fletnix.Models.Person", "Person")
                        .WithMany("MovieAward")
                        .HasForeignKey("PersonId")
                        .HasConstraintName("fk_Event_Person");

                    b.HasOne("fletnix.Models.AwardType", "AwardType")
                        .WithMany("MovieAward")
                        .HasForeignKey("Name", "Type")
                        .HasConstraintName("fk_Event_AwardType");
                });

            modelBuilder.Entity("fletnix.Models.MovieCast", b =>
                {
                    b.HasOne("fletnix.Models.Movie", "Movie")
                        .WithMany("MovieCast")
                        .HasForeignKey("MovieId")
                        .HasConstraintName("FK2_movie_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("fletnix.Models.Person", "Person")
                        .WithMany("MovieCast")
                        .HasForeignKey("PersonId")
                        .HasConstraintName("FK2_person_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("fletnix.Models.MovieDirector", b =>
                {
                    b.HasOne("fletnix.Models.Movie", "Movie")
                        .WithMany("MovieDirector")
                        .HasForeignKey("MovieId")
                        .HasConstraintName("FK_movie_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("fletnix.Models.Person", "Person")
                        .WithMany("MovieDirector")
                        .HasForeignKey("PersonId")
                        .HasConstraintName("FK_person_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("fletnix.Models.MovieGenre", b =>
                {
                    b.HasOne("fletnix.Models.Genre", "GenreNameNavigation")
                        .WithMany("MovieGenre")
                        .HasForeignKey("GenreName")
                        .HasConstraintName("FK_genre")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("fletnix.Models.Movie", "Movie")
                        .WithMany("MovieGenre")
                        .HasForeignKey("MovieId")
                        .HasConstraintName("FK3_movie_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("fletnix.Models.Watchhistory", b =>
                {
                    b.HasOne("fletnix.Models.Customer", "CustomerMailAddressNavigation")
                        .WithMany("Watchhistory")
                        .HasForeignKey("CustomerMailAddress")
                        .HasConstraintName("FK_customer");

                    b.HasOne("fletnix.Models.Movie", "Movie")
                        .WithMany("Watchhistory")
                        .HasForeignKey("MovieId")
                        .HasConstraintName("FK4_movie_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("fletnix.Models.Auth.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("fletnix.Models.Auth.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("fletnix.Models.Auth.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
