using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_Computos_Publica.Models.Entities.Configuracion;

namespace API_Computos_Publica.Models.DTO.Configuracion
{
    public class Seccion_DTO
    {
        public int Id { get; set; }
        public int Distrito_Id { get; set; }
        public string Distrito { get; set; }
        public int Municipio_Id { get; set; }
        public string Municipio { get; set; }
        public int Demarcacion_Id { get; set; }
        public string Demarcacion { get; set; }
        public string Nombre { get; set; }
        public string Cabecera_Localidad { get; set; }
        public int Padron_Electoral { get; set; }
        public int Listado_Nominal { get; set; }
        public string Tipo_Seccion { get; set; }
        public int? Region { get; set; }
    }
}
