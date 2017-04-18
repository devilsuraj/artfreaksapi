using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using artfriks.Models;

namespace artfriks.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<ArtCategory> ArtCategories { get; set; }
        public DbSet<ArtFavourite> ArtFavourites { get; set; }
        public DbSet<ArtTag> ArtTags { get; set; }
        public DbSet<ArtType> ArtTypes { get; set; }
        public DbSet<ArtWork> ArtWorks { get; set; }
        public DbSet<ArtArticles> ArtArticles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ArtWithTags> ArtWithTags { get; set; }
        public DbSet<ArtWithTypes> ArtWithTypes { get; set; }
        public DbSet<Medium> Mediums { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<DeliveryDetails> DeliveryDetails { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<CartId> CartIds { get; set; }
        public DbSet<DailyDiscount> DailyDiscounts { get; set; }
        public DbSet<OrderTransaction> OrderTransactions { get; set; }
        public DbSet<ArtReview> ArtReview { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Invite> Invites { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<DeliveryBoys> DeliveryBoys { get; set; }
        public DbSet<MessageReplies> MessageReplies { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<UserModel> UserModel { get; set; }
        public DbSet<homesection> homesection { get; set; }
        public DbSet<Styles> Styles { get; set; }
        public DbSet<Catgoryhomesection> Catgoryhomesection { get; set; }
        public DbSet<Featured> Featured { get; set; }
        public DbSet<ARtKeywords> ArtKeywords { get; set; }
    }
}
