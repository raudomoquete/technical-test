using DGII.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DGII.Infrastructure.Data;

public class DgiiDbContext : DbContext
{
    public DgiiDbContext()
    {
    }

    public DgiiDbContext(DbContextOptions<DgiiDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ComprobanteFiscal> ComprobantesFiscales { get; set; }

    public virtual DbSet<Contribuyente> Contribuyentes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Fallback connection string for development
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Dgii_test;Trusted_Connection=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ComprobanteFiscal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comproba__3214EC0727DE92D3");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.NCF)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired();
            entity.Property(e => e.itbis18)
                .HasColumnType("decimal(18, 2)")
                .IsRequired();
            entity.Property(e => e.monto)
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.HasOne(d => d.Contribuyente)
                .WithMany(p => p.ComprobantesFiscales)
                .HasForeignKey(d => d.ContribuyenteId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ComprobantesFiscales_Contribuyentes");

            // Índices para mejorar performance
            entity.HasIndex(e => e.NCF).IsUnique();
            entity.HasIndex(e => e.ContribuyenteId);
        });

        modelBuilder.Entity<Contribuyente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Contribu__3214EC070D5622E1");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.estatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsRequired();
            entity.Property(e => e.nombre)
                .HasMaxLength(100)
                .IsRequired();
            entity.Property(e => e.rncCedula)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired();
            entity.Property(e => e.tipo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired();

            // Índices para mejorar performance
            entity.HasIndex(e => e.rncCedula).IsUnique();
            entity.HasIndex(e => e.estatus);
        });
    }
}
