using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using artfriks.Data;

namespace artfriks.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170506102059_ooooooo.55")]
    partial class ooooooo55
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("artfriks.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<DateTime>("Adddate");

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Country");

                    b.Property<string>("CountryCode");

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("FormattedAddress");

                    b.Property<string>("FullName");

                    b.Property<string>("Gender");

                    b.Property<string>("LastName");

                    b.Property<string>("Latitude");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Longitude");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("OTP");

                    b.Property<int>("OTPfrom");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("Phone");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("PinCode");

                    b.Property<string>("Profession");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("State");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("artfriks.Models.ArtArticles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<string>("Article");

                    b.Property<string>("Author");

                    b.Property<bool>("IsPublished");

                    b.Property<DateTime>("PublishTime");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("ArtArticles");
                });

            modelBuilder.Entity("artfriks.Models.ArtCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ArtId");

                    b.Property<int>("Category");

                    b.HasKey("Id");

                    b.ToTable("ArtCategories");
                });

            modelBuilder.Entity("artfriks.Models.ArtFavourite", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ArtId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("ArtFavourites");
                });

            modelBuilder.Entity("artfriks.Models.ARtKeywords", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ArtId");

                    b.Property<string>("Keyword");

                    b.HasKey("Id");

                    b.ToTable("ArtKeywords");
                });

            modelBuilder.Entity("artfriks.Models.ArtReview", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Adddate");

                    b.Property<string>("ApplicationUser");

                    b.Property<string>("ArtId");

                    b.Property<string>("Review");

                    b.Property<int>("StarRating");

                    b.Property<int>("active");

                    b.HasKey("Id");

                    b.ToTable("ArtReview");
                });

            modelBuilder.Entity("artfriks.Models.ArtTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Tag");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("ArtTags");
                });

            modelBuilder.Entity("artfriks.Models.ArtType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("PictureUrl");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("ArtTypes");
                });

            modelBuilder.Entity("artfriks.Models.ArtWithTags", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ArtId");

                    b.Property<int>("TagId");

                    b.HasKey("Id");

                    b.ToTable("ArtWithTags");
                });

            modelBuilder.Entity("artfriks.Models.ArtWithTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ArtId");

                    b.Property<int>("ArtTypes");

                    b.HasKey("Id");

                    b.ToTable("ArtWithTypes");
                });

            modelBuilder.Entity("artfriks.Models.ArtWork", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<DateTime>("ArtCreationDate");

                    b.Property<string>("Category");

                    b.Property<string>("Description");

                    b.Property<string>("DimensionUnit");

                    b.Property<decimal>("Height");

                    b.Property<string>("MediumString");

                    b.Property<string>("Orientation");

                    b.Property<string>("PictureUrl");

                    b.Property<decimal>("Price");

                    b.Property<int>("Status");

                    b.Property<bool>("TermAccepted");

                    b.Property<string>("Title");

                    b.Property<string>("UserId");

                    b.Property<int>("Views");

                    b.Property<decimal>("Width");

                    b.HasKey("Id");

                    b.ToTable("ArtWorks");
                });

            modelBuilder.Entity("artfriks.Models.CartId", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("cartId");

                    b.Property<string>("productType");

                    b.Property<int>("productUnit");

                    b.HasKey("id");

                    b.ToTable("CartIds");
                });

            modelBuilder.Entity("artfriks.Models.CartItem", b =>
                {
                    b.Property<int>("CartItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CartId")
                        .IsRequired();

                    b.Property<int>("Count");

                    b.Property<DateTime>("DateCreated");

                    b.Property<int>("ProductId");

                    b.Property<decimal>("ProductPrice");

                    b.Property<string>("ProductType");

                    b.Property<int>("ProductUnit");

                    b.Property<string>("UserId");

                    b.Property<decimal>("totalPrice");

                    b.HasKey("CartItemId");

                    b.HasIndex("ProductId");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("artfriks.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ParentId");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("artfriks.Models.Catgoryhomesection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CatId");

                    b.Property<string>("Image");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.ToTable("Catgoryhomesection");
                });

            modelBuilder.Entity("artfriks.Models.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CountryName");

                    b.Property<string>("Iso");

                    b.Property<string>("Iso3");

                    b.Property<string>("NiceName");

                    b.Property<short>("Numcode");

                    b.Property<int>("Phonecode");

                    b.HasKey("Id");

                    b.ToTable("Country");
                });

            modelBuilder.Entity("artfriks.Models.DailyDiscount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ArtId");

                    b.Property<int>("CategoryId");

                    b.Property<int>("DiscountAmount");

                    b.Property<int>("DiscountPercent");

                    b.Property<int>("DiscountType");

                    b.HasKey("Id");

                    b.ToTable("DailyDiscounts");
                });

            modelBuilder.Entity("artfriks.Models.DeliveryBoys", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Active");

                    b.Property<DateTime>("Adddate");

                    b.Property<string>("Address");

                    b.Property<string>("FullName");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("DeliveryBoys");
                });

            modelBuilder.Entity("artfriks.Models.DeliveryDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Active");

                    b.Property<string>("DeliveryAssignedBy");

                    b.Property<DateTime>("DeliveryAssignedTime");

                    b.Property<int>("DeliveryAssignedTo");

                    b.Property<int>("DeliveryStatus");

                    b.Property<int>("OrderId");

                    b.HasKey("Id");

                    b.ToTable("DeliveryDetails");
                });

            modelBuilder.Entity("artfriks.Models.Featured", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Image");

                    b.Property<decimal>("Price");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.ToTable("Featured");
                });

            modelBuilder.Entity("artfriks.Models.Feedback", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Message");

                    b.Property<string>("Subject");

                    b.Property<string>("UserId");

                    b.HasKey("id");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("artfriks.Models.homesection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Image");

                    b.Property<string>("Sectiontype");

                    b.Property<string>("TextonButton");

                    b.Property<string>("Title");

                    b.Property<string>("Title2");

                    b.HasKey("Id");

                    b.ToTable("homesection");
                });

            modelBuilder.Entity("artfriks.Models.Invite", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.HasKey("Id");

                    b.ToTable("Invites");
                });

            modelBuilder.Entity("artfriks.Models.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Active");

                    b.Property<string>("AppliedCoupen");

                    b.Property<float>("DiscountAmount");

                    b.Property<int>("DiscountPercentage");

                    b.Property<float>("GrandTotal");

                    b.Property<DateTime>("InvoiceDate");

                    b.Property<string>("InvoiceNote");

                    b.Property<string>("InvoiceNumber");

                    b.Property<int>("OrderId");

                    b.Property<float>("ShippingAmount");

                    b.Property<string>("TaxAmount");

                    b.Property<string>("TaxRate");

                    b.Property<float>("TotalPayableAmount");

                    b.HasKey("Id");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("artfriks.Models.Medium", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Mediums");

                    b.HasKey("Id");

                    b.ToTable("Mediums");
                });

            modelBuilder.Entity("artfriks.Models.MessageReplies", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<string>("Message");

                    b.Property<int>("MessageId");

                    b.Property<string>("Status");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("MessageReplies");
                });

            modelBuilder.Entity("artfriks.Models.Messages", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<int>("ArtId");

                    b.Property<string>("FromUserId");

                    b.Property<string>("Message");

                    b.Property<string>("Status");

                    b.Property<string>("Subject");

                    b.Property<string>("ToUserId");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("artfriks.Models.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Adddate");

                    b.Property<string>("ApplicationUserId");

                    b.Property<string>("Icon");

                    b.Property<string>("NoteMessage");

                    b.Property<string>("NoteUrl");

                    b.Property<bool>("Read");

                    b.HasKey("Id");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("artfriks.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Active");

                    b.Property<string>("CartId");

                    b.Property<int>("DeliveryAddressId");

                    b.Property<decimal>("DeliveryCharges");

                    b.Property<int>("DiscountPercentage");

                    b.Property<decimal>("DiscountedAmount");

                    b.Property<bool>("IsCoupenApplied");

                    b.Property<bool>("IsTokenApplied");

                    b.Property<DateTime>("OrderDate");

                    b.Property<string>("OrderMessage");

                    b.Property<decimal>("OriginalAmount");

                    b.Property<int>("Total");

                    b.Property<string>("UserId");

                    b.HasKey("OrderId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("artfriks.Models.OrderDetail", b =>
                {
                    b.Property<int>("OrderDetailId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<int>("OrderId");

                    b.Property<int>("ProductId");

                    b.Property<string>("ProductType");

                    b.Property<int>("ProductUnit");

                    b.Property<int>("Quantity");

                    b.Property<decimal>("UnitPrice");

                    b.Property<decimal>("totalPrice");

                    b.HasKey("OrderDetailId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("artfriks.Models.OrderTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Adddate");

                    b.Property<string>("CartId");

                    b.Property<string>("IsRefund");

                    b.Property<string>("ModeOfPayment");

                    b.Property<int>("OrderId");

                    b.Property<float>("TransactionAmount");

                    b.Property<string>("TransactionReferenceNumber");

                    b.Property<string>("TransactionRequestString");

                    b.Property<string>("TransactionResponseString");

                    b.Property<string>("TransactionSignature");

                    b.Property<int>("TransactionStatus");

                    b.HasKey("Id");

                    b.ToTable("OrderTransactions");
                });

            modelBuilder.Entity("artfriks.Models.Profession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ProfessionString");

                    b.HasKey("Id");

                    b.ToTable("Professions");
                });

            modelBuilder.Entity("artfriks.Models.Styles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Image");

                    b.Property<int>("TagId");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.ToTable("Styles");
                });

            modelBuilder.Entity("artfriks.Models.Unit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Units");

                    b.HasKey("Id");

                    b.ToTable("Units");
                });

            modelBuilder.Entity("artfriks.Models.UserModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BioData");

                    b.Property<string>("PictureUrl");

                    b.Property<DateTime>("UpdateDate");

                    b.Property<string>("UserBrief");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("UserModel");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
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

                    b.HasIndex("UserId");

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

            modelBuilder.Entity("OpenIddict.Models.OpenIddictApplication", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ClientId");

                    b.Property<string>("ClientSecret");

                    b.Property<string>("DisplayName");

                    b.Property<string>("LogoutRedirectUri");

                    b.Property<string>("RedirectUri");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ClientId")
                        .IsUnique();

                    b.ToTable("OpenIddictApplications");
                });

            modelBuilder.Entity("OpenIddict.Models.OpenIddictAuthorization", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ApplicationId");

                    b.Property<string>("Scope");

                    b.Property<string>("Subject");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.ToTable("OpenIddictAuthorizations");
                });

            modelBuilder.Entity("OpenIddict.Models.OpenIddictScope", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.ToTable("OpenIddictScopes");
                });

            modelBuilder.Entity("OpenIddict.Models.OpenIddictToken", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ApplicationId");

                    b.Property<string>("AuthorizationId");

                    b.Property<string>("Subject");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("AuthorizationId");

                    b.ToTable("OpenIddictTokens");
                });

            modelBuilder.Entity("artfriks.Models.CartItem", b =>
                {
                    b.HasOne("artfriks.Models.ArtWork", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("artfriks.Models.OrderDetail", b =>
                {
                    b.HasOne("artfriks.Models.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("artfriks.Models.ArtWork", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
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
                    b.HasOne("artfriks.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("artfriks.Models.ApplicationUser")
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

                    b.HasOne("artfriks.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OpenIddict.Models.OpenIddictAuthorization", b =>
                {
                    b.HasOne("OpenIddict.Models.OpenIddictApplication", "Application")
                        .WithMany("Authorizations")
                        .HasForeignKey("ApplicationId");
                });

            modelBuilder.Entity("OpenIddict.Models.OpenIddictToken", b =>
                {
                    b.HasOne("OpenIddict.Models.OpenIddictApplication", "Application")
                        .WithMany("Tokens")
                        .HasForeignKey("ApplicationId");

                    b.HasOne("OpenIddict.Models.OpenIddictAuthorization", "Authorization")
                        .WithMany("Tokens")
                        .HasForeignKey("AuthorizationId");
                });
        }
    }
}
