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
    public class SepetEntity
    {
        [Key] // Birincil anahtarı belirt
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Otomatik artan ID
        public int SepetID { get; set; }

        public int KullaniciID { get; set; }
        public DateTime OlusturmaTarihi { get; set; }= DateTime.Now;

        // Foreign Key
        [JsonIgnore]
        public KullaniciEntity? Kullanici { get; set; }

        // Navigation Property
        [JsonIgnore]
        public ICollection<SepetDetaylariEntity>? SepetDetaylari { get; set; } = new List<SepetDetaylariEntity>();
    }
}
