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
    public class Solicitud_Recaptura_DTO
    {
        public int Id { get; set; }
        public int Boleta_Id { get; set; }
        public string Boleta { get; set; }
        public int Paquete_Id { get; set; }
        public int Tipo_Eleccion_Id { get; set; }
        public int? Oficina_Id { get; set; }
        public string? Oficina { get; set; }
        public string? Motivo { get; set; }
        public bool? Aprobado { get; set; }
        public string? Usuario_Registro { get; set; }
    }
}
