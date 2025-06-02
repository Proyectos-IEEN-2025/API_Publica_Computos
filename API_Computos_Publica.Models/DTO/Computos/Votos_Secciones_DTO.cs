using API_Computos_Publica.Models.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace API_Computos_Publica.Models.DTO.Computos
{
    public class Votos_Secciones_DTO
    {
        public int? Seccion_Id { get; set; }
        public string? Seccion { get; set; }
        public int? Casilla_Id { get; set; }
        public string? Casilla { get; set; }
        public int? Tipo_Eleccion_Id { get; set; }
        public string? Tipo_Eleccion { get; set; }
        public string? Acta_URL { get; set; }
        public int? Votos_Candidaturas { get; set; }
        public int? Votos_Nulos { get; set; }
        public int? Total_Votos { get; set; }
        public IEnumerable<Candidato_DTO> Candidaturas { get; set; }
    }
}
