using APBDProject.Server.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace APBDProject.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Ohlc> Ohlcs { get; set; }
        
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions) { }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Company>(c =>
            {
                c.HasKey(e => e.Symbol);
            });

            builder.Entity<Subscription>(s =>
            {
                s.HasKey(e => e.IdSubscription);
                s.HasOne(e => e.User).WithMany(e => e.Subscriptions).HasForeignKey(e => e.IdUser);
                s.HasOne(e => e.Company).WithMany(e => e.Users).HasForeignKey(e => e.CompanyName);
            });
            
            builder.Entity<Ohlc>(o =>
            {
                o.HasKey(e => e.IdOhlc);

                o.HasOne(e => e.Company).WithMany(e => e.Ohlcs).HasForeignKey(e => e.Symbol); 
            });
        }
    }
}
