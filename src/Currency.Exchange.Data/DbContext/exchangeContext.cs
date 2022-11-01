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

        public virtual DbSet<Data.DbContext.Exchange> Exchanges { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Name=ConnectionStrings:Default");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Data.DbContext.Exchange>(entity =>
            {
                entity.ToTable("exchange");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Clientid).HasColumnName("clientid");

                entity.Property(e => e.Datecreated)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("datecreated")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Exchangerate).HasColumnName("exchangerate");

                entity.Property(e => e.Fromcurrency)
                    .HasMaxLength(3)
                    .HasColumnName("fromcurrency");

                entity.Property(e => e.Tocurrency)
                    .HasMaxLength(3)
                    .HasColumnName("tocurrency");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
