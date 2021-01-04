using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using static DAW_project.Models.RegisterViewModel;

namespace DAW_project.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public PrivacyEnum Privacy { get; set; }
        public virtual ICollection<Comment> UserComments { get; set; }
        public virtual ICollection<Post> UserPosts { get; set; }
        public virtual ICollection<Group> UserGroups { get; set; }
        [ForeignKey("Admin_Id")]
        public virtual ICollection<Notification> AdminNotifications { get; set; }
        [ForeignKey("User_Id")]
        public virtual ICollection<Notification> UserNotifications { get; set; }

        //[ForeignKey("User1_Id")]
        public virtual ICollection<Friendship> SentRequests { get; set; }

        //[ForeignKey("User2_Id")]
        public virtual ICollection<Friendship> ReceivedRequests { get; set; }

        //public virtual ICollection<ApplicationUser> MyFriends { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext,DAW_project.Migrations.Configuration>("DefaultConnection"));
        }

        //public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Group> Groups { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}