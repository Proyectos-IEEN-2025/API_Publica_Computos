using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_Computos_Publica.Models.Entities.Configuracion;

namespace API_Computos_Publica.Models.DTO.Configuracion
{
    public class Candidato_DTO
    {

        public int Id { get; set; }
        public int Tipo_Eleccion_Id { get; set; }
        public string? Tipo_Eleccion { get; set; }
        public string? Poder_Postulante { get; set; }
        public string? No_Formula { get; set; }
        public string? Nombres { get; set; }
        public string? Apellido_Paterno { get; set; }
        public string? Apellido_Materno { get; set; }
        public string? Sexo { get; set; }
        public bool? Activo { get; set; }
        public int? Votos { get; set; }
        public decimal? Porcentaje { get; set; }
        public int? Orden { get; set; }
        public string? Fotografia_URL { get; set; }
    }
}
