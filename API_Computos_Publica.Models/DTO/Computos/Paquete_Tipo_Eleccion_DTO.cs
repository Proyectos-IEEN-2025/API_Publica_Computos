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
    public class Paquete_Tipo_Eleccion_DTO
    {
        public int Id { get; set; }
        public int Paquete_Id { get; set; }
        public int Tipo_Eleccion_Id { get; set; }
        public string Tipo_Eleccion { get; set; }
        public int Boletas_Sobrantes { get; set; } = 0;
        public int Boletas_Capturadas { get; set; } = 0;
        public int Votos_Nulos { get; set; } = 0;
        public int Campos_No_Utilizados { get; set; } = 0;
        public int Total_Votos { get; set; } = 0;
        public string? Acta_Url { get; set; }
        public bool? Cotejo { get; set; }
        public bool? Publicado { get; set; }
    }
}
