using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Computos_Publica.Models.DTO.Configuracion
{
    public class Estado_DTO
    {
        public string Clave { get; set; }
        public required string? Nombre { get; set; }
    }
}
