using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_Computos_Publica.Models.System;

namespace API_Computos_Publica.Models.Entities.Configuracion
{
    public class Tipo_Eleccion : Auditoria
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }
        [Required]
        [MaxLength(10)]
        public string Siglas { get; set; }
        public bool Activo { get; set; }
        public int? No_Mujeres { get; set; }
        public int? No_Hombres { get; set; }
        //Correo
        public string? Correo { get; set; }
        public string? Contrasena { get; set; }
        public int? Puerto { get; set; }
        public string? Smtp { get; set; }
        //Correo rp
        public string? Correo_RP { get; set; }
        public string? Contrasena_RP { get; set; }
        public int? Puerto_RP { get; set; }
        public string? Smtp_RP { get; set; }
        public string? Color { get; set; }
        public int? Orden { get; set; }
    }
}
