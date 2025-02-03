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
    public class SepetDetaylariEntity
    {
        [Key] // Birincil anahtarı belirt
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Otomatik artan ID
        public int SepetDetayID { get; set; }
        public int SepetID { get; set; }
        public int UrunID { get; set; } 
        public int Miktar {  get; set; }

        public decimal BirimFiyat { get; set; }

        // Foreign Keys
        [JsonIgnore]

        public SepetEntity? Sepet { get; set; }
        [JsonIgnore]

        public UrunlerEntity? Urun { get; set; }
    }
}
