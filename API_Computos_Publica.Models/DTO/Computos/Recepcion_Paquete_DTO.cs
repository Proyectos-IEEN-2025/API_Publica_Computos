using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_Computos_Publica.Models.Entities.Computos;

namespace API_Computos_Publica.Models.DTO.Computos
{
    public class Recepcion_Paquete_DTO
    {
        public int Id { get; set; }
        public string? Fecha_Recepcion { get; set; }
        public string? Fecha_Registro { get; set; }
        public string? Ciudadano_Entrega { get; set; }
        public string? Puesto { get; set; }
        public int Total_Paquetes { get; set; }
        public string? Recibe { get; set; }
        public string? Cargo_Recibe { get; set; }
        public string? Lugar_Recepcion { get; set; }
        public string? Medio_Recepcion { get; set; }
        public int? Oficina_Id { get; set; }
        public string? Oficina { get; set; }
    }
}
