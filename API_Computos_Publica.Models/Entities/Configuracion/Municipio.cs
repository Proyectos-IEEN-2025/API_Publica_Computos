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
    public class Municipio : Auditoria
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string? Clave { get; set; }
        [Required]
        public string? Estado_Id { get; set; }
        [Required]
        public string? Nombre { get; set; }
        public string? Cabecera { get; set; }
        [Required]
        public int? Region { get; set; }

        [ForeignKey("Estado_Id")]
        public Estado? Estado { get; set; }
    }
}
