using API_Computos_Publica.Models.Entities.Configuracion;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Computos_Publica.Models.Entities.Computos
{
    public class Candidatos_Tipo_Eleccion_Adicional
    {
        [Key]
        public int Id { get; set; }
        public int Paquete_Tipo_Eleccion_Id { get; set; }
        public int Candidato_Id { get; set; }
        public int Votos { get; set; }
        public bool? Adicional { get; set; }
        public bool? Adicional_JAO { get; set; }

        [ForeignKey(nameof(Paquete_Tipo_Eleccion_Id))]
        public Paquete_Tipo_Eleccion_Adicional? Paquete_Tipo_Eleccion { get; set; }
        [ForeignKey(nameof(Candidato_Id))]
        public Candidato? Candidato { get; set; }
    }
}
