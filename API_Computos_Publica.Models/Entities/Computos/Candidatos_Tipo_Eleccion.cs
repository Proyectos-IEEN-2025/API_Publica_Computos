using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_Computos_Publica.Models.Entities.Configuracion;

namespace API_Computos_Publica.Models.Entities.Computos
{
    public class Candidatos_Tipo_Eleccion
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Paquete_Tipo_Eleccion_Id { get; set; }
        [Required]
        public int Candidato_Id { get; set; }
        [Required]
        public int Votos { get; set; }

        [ForeignKey(nameof(Paquete_Tipo_Eleccion_Id))]
        public Paquete_Tipo_Eleccion? Paquete_Tipo_Eleccion { get; set; }
        [ForeignKey(nameof(Candidato_Id))]
        public Candidato? Candidato { get; set; }
    }
}
