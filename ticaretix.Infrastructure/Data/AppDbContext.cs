using Microsoft.EntityFrameworkCore;
using ticaretix.Core.Entities;

namespace ticaretix.Infrastructure.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CategoryEntity> Kategoriler { get; set; }
        public DbSet<KullaniciEntity> Kullanicilar { get; set; }
        public DbSet<UrunlerEntity> Urunler { get; set; }
        public DbSet<SepetEntity> Sepetler { get; set; }
        public DbSet<SepetDetaylariEntity> SepetDetaylari { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Kategori - Ürün (1-N)
            modelBuilder.Entity<UrunlerEntity>()
                .HasOne(u => u.Kategori)
                .WithMany(k => k.Urunler)
                .HasForeignKey(u => u.KategoriID);

            // Kullanici - Sepet (1-N)
            modelBuilder.Entity<SepetEntity>()
                .HasOne(s => s.Kullanici)
                .WithMany(k => k.Sepetler)
                .HasForeignKey(s => s.KullaniciID);

            // Sepet - SepetDetay (1-N)
            modelBuilder.Entity<SepetDetaylariEntity>()
                .HasOne(sd => sd.Sepet)
                .WithMany(s => s.SepetDetaylari)
                .HasForeignKey(sd => sd.SepetID);

            // Urun - SepetDetay (1-N)
            modelBuilder.Entity<SepetDetaylariEntity>()
                .HasOne(sd => sd.Urun)
                .WithMany(u => u.SepetDetaylari)
                .HasForeignKey(sd => sd.UrunID);
        }

    }

   
}
