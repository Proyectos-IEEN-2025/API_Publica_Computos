using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_Computos_Publica.Models.Entities.Configuracion;
using API_Computos_Publica.Models.System;

namespace API_Computos_Publica.Models.Entities.Computos
{
    public class Actas_Parciales : Auditoria
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Oficina_Id { get; set; }
        [Required]
        public int Tipo_Eleccion_Id { get; set; }
        public bool? Impreso { get; set; } = false;
        public string? Acta_Url { get; set; }
        public string? Acta_Url_Local { get; set; }
        public string? Content_Type { get; set; }
        public string? File_Name { get; set; }
        public DateTime? Fecha_Impresion { get; set; }

        [ForeignKey(nameof(Oficina_Id))]
        public Oficina? Oficina { get; set; }

        [ForeignKey(nameof(Tipo_Eleccion_Id))]
        public Tipo_Eleccion? Tipo_Eleccion { get; set; }
    }
}
