using SQLite;

namespace Examen_2p.Models
{
    [Table("Gasolinera")]
    class GasModel
    {
        public int id { get; set; }
        public string Marca { get; set; }
        public string Sucursal { get; set; }
        public string Foto { get; set; }
        public decimal GasVerde { get; set; }
        public decimal GasRojo { get; set; }
        public decimal Diesel { get; set; }
        public float Latitud { get; set; }
        public float Longitud { get; set; }
    }
}
