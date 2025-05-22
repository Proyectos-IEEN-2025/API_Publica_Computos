using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_Computos_Publica.Models.Entities.Configuracion;

namespace API_Computos_Publica.Models.DTO.Computos
{
    public class Acta_Parcial_DTO
    {
        public int Id { get; set; }
        public int Oficina_Id { get; set; }
        public string? Oficina { get; set; }
        public int Tipo_Eleccion_Id { get; set; }
        public string? Tipo_Eleccion { get; set; }
        public bool? Impreso { get; set; }
        public string? Acta_Url { get; set; }
        public string? Acta_Url_Local { get; set; }
    }
}
