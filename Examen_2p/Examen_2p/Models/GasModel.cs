using SQLite;

namespace Examen_2p.Models
{
    [Table("Gasolinera")]
    public class GasModel
    {
        public int id { get; set; }
        public string Marca { get; set; }
        public string Sucursal { get; set; }
        public string Foto { get; set; }
        public decimal GasVerde { get; set; }
        public decimal GasRojo { get; set; }
        public decimal Diesel { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
    }
}
