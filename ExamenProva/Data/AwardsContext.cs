using ExamenProva.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamenProva.Data;

public class AwardsContext : DbContext
{
    public AwardsContext() { }
    
    public DbSet<Candidatura> Candidaturas { get; set; }
    public DbSet<Representant> Representants { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured) optionsBuilder.UseSqlite("Data Source=goty.db").UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // recursiva
        modelBuilder.Entity<Candidatura>()
            .HasOne(c => c.CandidaturaPrincipal)
            .WithMany(a => a.CandidaturesRelacionades)
            .HasForeignKey("PrincipalID")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        
        // 1-1
        modelBuilder.Entity<Candidatura>()
            .HasOne(c => c.Representant)
            .WithOne(r => r.Candidatura)
            .HasForeignKey<Candidatura>("RepresentantID")
            .IsRequired();
        
        // enum amb string
        modelBuilder.Entity<Candidatura>()
            .Property(c => c.Categoria)
            .HasConversion<string>();

    }
}