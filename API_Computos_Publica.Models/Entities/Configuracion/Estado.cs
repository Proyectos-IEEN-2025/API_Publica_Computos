using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_Computos_Publica.Models.System;

namespace API_Computos_Publica.Models.Entities.Configuracion
{
    public class Estado : Auditoria
    {
        [Key]
        public string Clave { get; set; }
        [Required]
        public required string? Nombre { get; set; }
    }
}
