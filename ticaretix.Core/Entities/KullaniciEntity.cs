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
    public class KullaniciEntity
    {
        [Key] // Birincil anahtarı belirt
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Otomatik artan ID
        public int KullaniciID { get; set; }
        public string KullaniciAdi { get; set; } = null!;
        public string Sifre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime KayitTarihi { get; set; } = DateTime.Now;
        public string Role { get; set; } = null!;

        // Navigation Property
        [JsonIgnore]
        public ICollection<SepetEntity> Sepetler { get; set; } = new List<SepetEntity>();
    }
}
