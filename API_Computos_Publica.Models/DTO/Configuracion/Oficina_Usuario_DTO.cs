using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Computos_Publica.Models.DTO.Configuracion
{
    public class Oficina_Usuario_DTO
    {
        public int? Oficina_Id { get; set; }
        public string? Nombre_Oficina { get; set; }
        public string? Usuario_Id { get; set; }
        public int? Empleado_Id { get; set; }
        public string? Nombre_Completo { get; set; }
        public string? Municipio { get; set; }
        public bool? Tiene_Ventanas { get; set; }
        public string? Estatus_Bodega { get; set; }
    }
}
