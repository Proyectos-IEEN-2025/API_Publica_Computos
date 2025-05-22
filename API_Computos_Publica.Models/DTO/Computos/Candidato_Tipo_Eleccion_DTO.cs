using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_Computos_Publica.Models.Entities.Computos;
using API_Computos_Publica.Models.Entities.Configuracion;

namespace API_Computos_Publica.Models.DTO.Computos
{
    public class Candidato_Tipo_Eleccion_DTO
    {
        public int Id { get; set; }
        public int Paquete_Tipo_Eleccion_Id { get; set; }
        public int Candidato_Id { get; set; }
        public string Candidato { get; set; }
        public string? No_Formula { get; set; }
        public int Votos { get; set; }
    }
}
