using DGII.Domain.Entities;

namespace DGII.Infrastructure;

public partial class dbContextDGII : DbContext
{
    public dbContextDGII()
    {
    }

    public dbContextDGII(DbContextOptions<dbContextDGII> options)
        : base(options)
    {
    }

    public virtual DbSet<ComprobanteFiscal> ComprobantesFiscales { get; set; }

    public virtual DbSet<Contribuyente> Contribuyentes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Dgii_test;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ComprobanteFiscal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comproba__3214EC0727DE92D3");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.NCF)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.itbis18).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.monto).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Contribuyente).WithMany(p => p.ComprobantesFiscales)
                .HasForeignKey(d => d.ContribuyenteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComprobantesFiscales_Contribuyentes");
        });

        modelBuilder.Entity<Contribuyente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Contribu__3214EC070D5622E1");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.estatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.nombre).HasMaxLength(100);
            entity.Property(e => e.rncCedula)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.tipo)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
