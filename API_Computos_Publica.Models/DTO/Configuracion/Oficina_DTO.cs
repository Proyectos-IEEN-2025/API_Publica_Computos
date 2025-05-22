
namespace API_Computos_Publica.Models.DTO.Configuracion
{
    public class Oficina_DTO
    {
        public int Id { get; set; }
        public string? Municipio_Id { get; set; }
        public string? Municipio { get; set; }
        public string? Localidad { get; set; }
        public string? Nombre { get; set; }
        public bool? OPLE { get; set; }
        public string? Direccion { get; set; }
        public bool? Tiene_Ventanas { get; set; }
        public string? Estatus_Bodega { get; set; }
        public int? No_Oficina { get; set; }
    }
}
