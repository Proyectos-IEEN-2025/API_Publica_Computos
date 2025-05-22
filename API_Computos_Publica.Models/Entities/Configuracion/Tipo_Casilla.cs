using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_Computos_Publica.Models.System;

namespace API_Computos_Publica.Models.Entities.Configuracion
{
    public class Tipo_Casilla : Auditoria
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }
        [Required]
        [MaxLength(10)]
        public string Siglas { get; set; }
    }
}
