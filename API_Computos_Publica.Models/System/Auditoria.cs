using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Computos_Publica.Models.System
{
    public class Auditoria
    {
        public DateTime Fecha_Registro { get; set; } = DateTime.Now;
        public DateTime? Fecha_Modificacion { get; set; }
        public DateTime? Fecha_Eliminacion { get; set; }
        public string? Usuario_Registro { get; set; }
        public string? Usuario_Modificacion { get; set; }
        public string? Usuario_Eliminacion { get; set; }
        public bool? Eliminado { get; set; } = false;
    }
}
