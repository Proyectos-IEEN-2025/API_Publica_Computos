using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_Computos_Publica.Models.System;

namespace API_Computos_Publica.Models.Entities.Configuracion
{
    public class Oficina : Auditoria
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int? Municipio_Id { get; set; }
        public string? Localidad { get; set; }
        [Required]
        public string? Nombre { get; set; }
        public bool? OPLE { get; set; } = false;
        public string? Direccion { get; set; }
        public string? Estatus_Bodega { get; set; }
        public bool? Tiene_Ventanas { get; set; }
        public bool? Regional { get; set; } = false;
        public int? No_Oficina { get; set; }
     

        [ForeignKey("Municipio_Id")]
        public Municipio? Municipio { get; set; }
    }
}
