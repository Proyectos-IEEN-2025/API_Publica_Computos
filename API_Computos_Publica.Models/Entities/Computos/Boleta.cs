using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_Computos_Publica.Models.Entities.Configuracion;
using API_Computos_Publica.Models.Enums;
using API_Computos_Publica.Models.System;

namespace API_Computos_Publica.Models.Entities.Computos
{
    public class Boleta : Auditoria
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Numero { get; set; }
        [Required]
        public int Tipo_Eleccion_Id { get; set; }
        [Required]
        public int Paquete_Id { get; set; }
        [Required]
        public int Votos_Nulos { get; set; }
        [Required]
        public int Campos_No_Utilizados { get; set; }
        public string? Candidata_1 { get; set; }
        public bool? Is_Nulo_1 { get; set; }
        public string? Candidata_2 { get; set; }
        public bool? Is_Nulo_2 { get; set; }
        public string? Candidata_3 { get; set; }
        public bool? Is_Nulo_3 { get; set; }
        public string? Candidata_4 { get; set; }
        public bool? Is_Nulo_4 { get; set; }
        public string? Candidata_5 { get; set; }
        public bool? Is_Nulo_5 { get; set; }
        public string? Candidato_1 { get; set; }
        public bool? Is_Nulo_6 { get; set; }
        public string? Candidato_2 { get; set; }
        public bool? Is_Nulo_7 { get; set; }
        public string? Candidato_3 { get; set; }
        public bool? Is_Nulo_8 { get; set; }
        public string? Candidato_4 { get; set; }
        public bool? Is_Nulo_9 { get; set; }
        public int? NO_Capturas { get; set; } = 0;
        public bool? Finalizada { get; set; } = false;
        public bool? Adicional { get; set; } = false;
        public string? Captura1 { get; set; }
        public string? Captura2 { get; set; }
        public string? Captura3 { get; set; }
        public bool? Inconsistencia { get; set; } = false;
        public Inconsistencias? Tipo_Inconsistencia { get; set; }

        [ForeignKey(nameof(Tipo_Eleccion_Id))]
        public Tipo_Eleccion? Tipo_Eleccion { get; set; }
        [ForeignKey(nameof(Paquete_Id))]
        public Paquete? Paquete { get; set; }
    }
}
