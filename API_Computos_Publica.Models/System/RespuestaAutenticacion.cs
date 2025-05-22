using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Computos_Publica.Models.System
{
    public class RespuestaAutenticacion
    {
        public string Token { get; set; }
        public DateTime Expiracion { get; set; }
        public string? UserName { get; set; }
        public string? Usuario { get; set; }
        public string? Tipo_Usuario { get; set; }
        public bool? Is_Empleado { get; set; }
        public string? Usuario_Id { get; set; }
        public int? Empleado_Id { get; set; }
        public int? Candidato_Id { get; set; }
        public int? OficinaId { get; set; }
    }
}
