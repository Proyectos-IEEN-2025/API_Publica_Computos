using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Computos_Publica.Models.DTO.Configuracion
{
    public class Tipo_Casilla_DTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Siglas { get; set; }
    }
}
