using Candidate.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Tracing;
using System.Reflection.Emit;

namespace Candidate.Data
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }
        public DbSet<User> User { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Partner> Partners { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<EventPartners>()
                .HasKey(ep => new { ep.EventId, ep.PartnerId });
            builder.Entity<EventPartners>()
                .HasOne(ep => ep.Event)
                .WithMany(e => e.EventPartners)
                .HasForeignKey(ep => ep.EventId);
            builder.Entity<EventPartners>()
                .HasOne(ep => ep.Partner)
                .WithMany(p => p.EventPartners)
                .HasForeignKey(ep => ep.PartnerId);

            builder.Entity<Model.EventChannels>()
                .HasKey(ec => new { ec.EventId, ec.ChannelId });

            builder.Entity<Model.EventChannels>()
                .HasOne(ec => ec.Event)
                .WithMany(e => e.EventChannels)
                .HasForeignKey(ec => ec.EventId);

            builder.Entity<Model.EventChannels>()
                .HasOne(ec => ec.Channel)
                .WithMany(c => c.EventChannels)
                .HasForeignKey(ec => ec.ChannelId);
            List<IdentityRole> roles = new List<IdentityRole> { 
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "Leader",
                    NormalizedName = "LEADER"
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);

        }
    }
}
