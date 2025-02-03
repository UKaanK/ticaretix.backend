using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace ticaretix.Core.Entities
{
    public class UrunlerEntity
    {
        [Key] // Birincil anahtarı belirt
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Otomatik artan ID
        public int UrunID { get; set; }
        public string UrunAdi { get; set; } = null!;
        public string Aciklama { get; set; } = null!;
        public decimal Fiyat { get; set; }
        public int StokMiktari { get; set; }
        public int KategoriID { get; set; }
        public DateTime EklenmeTarihi { get; set; }
        public string Image {  get; set; } = null!;


        // Foreign Key
        [JsonIgnore]
        public CategoryEntity? Kategori { get; set; }

        // Navigation Property
        [JsonIgnore]
        public ICollection<SepetDetaylariEntity>? SepetDetaylari { get; set; } = new List<SepetDetaylariEntity>();
    }
}
