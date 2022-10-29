using Microsoft.EntityFrameworkCore;

namespace Currency.Exchange.Data.DbContext
{
    public partial class exchangeContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public exchangeContext()
        {
        }

        public exchangeContext(DbContextOptions<exchangeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<Exchange> Exchanges { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost:5432;Database=exchange;Username=postgres;Password=MyPassword!23");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("client");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .HasColumnName("name");

                entity.Property(e => e.Surname)
                    .HasMaxLength(150)
                    .HasColumnName("surname");
            });

            modelBuilder.Entity<Exchange>(entity =>
            {
                entity.ToTable("exchange");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Clientid).HasColumnName("clientid");

                entity.Property(e => e.Exchangerate).HasColumnName("exchangerate");

                entity.Property(e => e.Fromcurrency)
                    .HasMaxLength(3)
                    .HasColumnName("fromcurrency");

                entity.Property(e => e.Tocurrency)
                    .HasMaxLength(3)
                    .HasColumnName("tocurrency");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Exchanges)
                    .HasForeignKey(d => d.Clientid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_exchange_client");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
