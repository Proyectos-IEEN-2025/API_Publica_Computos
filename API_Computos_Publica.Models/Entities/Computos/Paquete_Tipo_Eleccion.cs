using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_Computos_Publica.Models.Entities.Configuracion;
using API_Computos_Publica.Models.System;

namespace API_Computos_Publica.Models.Entities.Computos
{
    public class Paquete_Tipo_Eleccion : Auditoria
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Paquete_Id { get; set; }
        [Required]
        public int Tipo_Eleccion_Id { get; set; }
        public int Boletas_Sobrantes { get; set; } = 0;
        public int Boletas_Capturadas { get; set; } = 0;
        public int Votos_Nulos { get; set; } = 0;
        public int Campos_No_Utilizados { get; set; } = 0;
        public int Total_Votos { get; set; } = 0;
        public bool? Impreso { get; set; } = false;
        public bool? Cotejo { get; set; } = false;
        public bool? Publicado { get; set; } = false;
        public string? Acta_Url { get; set; }
        public string? Acta_Url_Local { get; set; }
        public string? Content_Type { get; set; }
        public string? File_Name { get; set; }

        public DateTime? Fecha_Impresion { get; set; }
        public DateTime? Fecha_Cotejo { get; set; }
        public DateTime? Fecha_Publicacion { get; set; }
        [ForeignKey(nameof(Paquete_Id))]
        public Paquete? Paquete { get; set; }
        [ForeignKey(nameof(Tipo_Eleccion_Id))]
        public Tipo_Eleccion? Tipo_Eleccion { get; set; }
        public IEnumerable<Candidatos_Tipo_Eleccion>? Candidatos_Votos { get; set; }
    }
}
