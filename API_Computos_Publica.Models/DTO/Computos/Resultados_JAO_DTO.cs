using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Computos_Publica.Models.DTO.Computos
{
    public class Resultados_JAO_DTO
    {
        public int Tipo_Eleccion_Id { get; set; }
        public string Tipo_Eleccion { get; set; }
        public int Boletas_Sobrantes { get; set; } = 0;
        public int Boletas_Capturadas { get; set; } = 0;
        public int Votos_Nulos { get; set; } = 0;
        public int Campos_No_Utilizados { get; set; } = 0;
        public int Total_Votos { get; set; } = 0;
        public int Id { get; set; }
        public int Paquete_Tipo_Eleccion_Id { get; set; }
        public int Candidato_Id { get; set; }
        public string Candidato { get; set; }
        public int Votos { get; set; }
    }
}
