using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Computos_Publica.Models.DTO.Computos
{
    public class Resultados_Paquete_Tipo_Eleccion_DTO
    {
        public Paquete_Tipo_Eleccion_DTO Paquete_Tipo_Eleccion { get; set; }
        public string? Ruta_Acta { get; set; }
        public List<Candidato_Tipo_Eleccion_DTO> Candidatos { get; set; }
    }
}
