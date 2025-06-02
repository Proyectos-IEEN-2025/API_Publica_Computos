using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Computos_Publica.Models.DTO.Computos
{
    public class Avance_DTO
    {
        public int? Participacion { get; set; }
        public decimal? Porcentaje_Participacion { get; set; }
        public int? Lista_Nominal { get; set; }
        public int Votos_Candidatura { get; set; }
        public int Votos_Nulos { get; set; }
        public int Total { get; set; }
        public decimal Avance { get; set; }
        public int Casillas_Computadas { get; set; }
        public int Casillas_Esperadas { get; set; }
    }
}