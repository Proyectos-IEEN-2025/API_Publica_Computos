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
    public class Paquete : Auditoria
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Casilla_Id { get; set; }
        public int? Mesa_Trabajo_Id { get; set; }
        public int? Bina_Id { get; set; }
        public int? Total_Personas_Voto { get; set; } = 0;
        public int? Votos_Magistratura_Tribunal_Superior_Justicia { get; set; } = 0;
        public int? Votos_Magistratura_Tribunal_Diciplina { get; set; } = 0;
        public int? Votos_Juezas_Jueces { get; set; } = 0;
        public bool? Computada { get; set; } = false;
        public bool? Recibido { get; set; } = false;
        public bool? En_Bodega { get; set; } = false;

        [ForeignKey(nameof(Casilla_Id))]
        public Casilla? Casilla { get; set; }
        public IEnumerable<Paquete_Tipo_Eleccion> Paquetes_Tipos_Elecciones { get; set; }
    }
}
