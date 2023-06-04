using Alea.Areas.Identity.Data;
using Alea.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alea.Data;

public class AleaContext : IdentityDbContext<AleaUser>
{
    public AleaContext(DbContextOptions<AleaContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.ApplyConfiguration(new AppUserEntityConfiguration());
    }

    public DbSet<Knjige> Knjige { get; set; }
    public DbSet<Ocjene> Ocjene { get; set; }
    //public DbSet<Korisnici> Korisnik { get; set; }
    public DbSet<Glumci> Glumci { get; set; }
}


internal class AppUserEntityConfiguration : IEntityTypeConfiguration<AleaUser>
{
    public void Configure(EntityTypeBuilder<AleaUser> builder)
    {
        builder.Property(u => u.Ime).HasMaxLength(128);
        builder.Property(u => u.Prezime).HasMaxLength(128);
    }

}
