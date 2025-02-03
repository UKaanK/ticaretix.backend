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
    public class CategoryEntity
    {
        [Key] // Birincil anahtarı belirt
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Otomatik artan ID
        public int KategoriID { get; set; }
        public string KategoriAdi { get; set; } = null!;
        public string Aciklama { get; set; } = null!;

        // Navigation Property
        [JsonIgnore]
        public ICollection<UrunlerEntity> Urunler { get; set; } = new List<UrunlerEntity>();


    }
}
