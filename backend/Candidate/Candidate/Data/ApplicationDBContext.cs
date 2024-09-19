using Candidate.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Candidate.Data
{
    public class ApplicationDBContext : IdentityDbContext<User, Role, string, 
        IdentityUserClaim<string>, UserRoles, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles {  get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<CandidateInfo> CandidateInfos { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<CandidatePositions> CandidatesPositions { get; set; }
        public DbSet<EventChannels> EventChannels { get; set; }
        public DbSet<EventPartners> EventPartners { get; set; }
        public DbSet<EventPositions> EventPositions { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>(b =>
            {
                b.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            });
            builder.Entity<Role>(b =>
            {
                b.HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
            });
            List<Role> roles = new List<Role> {
                new Role
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
                new Role
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new Role
                {
                    Name = "Leader",
                    NormalizedName = "LEADER"
                }
            };
            builder.Entity<Role>().HasData(roles);
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

            builder.Entity<EventPositions>()
                .HasKey(ep => new { ep.EventId, ep.PositionId });
            builder.Entity<EventPositions>()
                .HasOne(ep => ep.Event)
                .WithMany(e => e.EventPositions)
                .HasForeignKey(ep => ep.EventId);
            builder.Entity<EventPositions>()
                .HasOne(ep => ep.Position)
                .WithMany(p => p.EventPositions)
                .HasForeignKey(ep => ep.PositionId);
            builder.Entity<Application>()
                    .HasKey(a => new { a.CandidateId, a.EventId });
            builder.Entity<Application>()
                .HasOne(a => a.CandidateInfo)
                .WithMany(c => c.Applications)
                .HasForeignKey(a => a.CandidateId);
            builder.Entity<CandidatePositions>()
                    .HasKey(ca => new { ca.CandidateInfoId, ca.EventId, ca.PositionId });
            builder.Entity<CandidatePositions>()
                .HasOne(cj => cj.Application)
                .WithMany(c => c.CandidatePositions)
                .HasForeignKey(cj => new { cj.CandidateInfoId, cj.EventId })
                .OnDelete(DeleteBehavior.NoAction); ;

            builder.Entity<CandidatePositions>()
                .HasOne(cj => cj.Position)
                .WithMany(j => j.CandidatePositions)
                .HasForeignKey(cj => cj.PositionId)
                .OnDelete(DeleteBehavior.NoAction); ;
            builder.Entity<CandidateInfo>()
                .HasOne(c => c.Partner)
                .WithMany(u => u.CandidateInfos)
                .HasForeignKey(c => c.UniversityId);
            builder.Entity<CandidateInfo>()
                .HasOne(c => c.User)
                .WithMany(u => u.CandidateInfos)
                .HasForeignKey(c => c.UserId);
            builder.Entity<Application>()
                .HasOne(a => a.CandidateInfo)
                .WithMany(c => c.Applications)
                .HasForeignKey(a => a.CandidateId);
            builder.Entity<Application>()
                .HasOne(a => a.Event)
                .WithMany(e => e.Applications)
                .HasForeignKey(a => a.EventId);
            builder.Entity<Application>()
                .HasOne(a => a.Channel)
                .WithMany(c => c.Applications)
                .HasForeignKey(a => a.ChannelId);
            
        }
    }
}
