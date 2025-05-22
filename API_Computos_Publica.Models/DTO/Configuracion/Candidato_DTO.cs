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
        public string? CURP { get; set; }
        public string? Especialidad { get; set; }
        public string? RFC { get; set; }
        public string? Clave_Elector { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Pagina_Web { get; set; }
        public string? Calle { get; set; }
        public string? No_Exterior { get; set; }
        public string? No_Interior { get; set; }
        public string? Colonia { get; set; }
        public string? Codigo_Postal { get; set; }
        public string? Municipio { get; set; }
        public string? Estado { get; set; }
        public string? Fotografia_URL { get; set; }
        public string? Facebook { get; set; }
        public string? Twitter { get; set; }
        public string? Instagram { get; set; }
        public string? Youtube { get; set; }
        public string? TikTok { get; set; }
        public string? Otro { get; set; }
        public string? Correo_Publico { get; set; }
        public string? Telefono_Publico { get; set; }
        public string? Razon_Ocupacion { get; set; }
        public string? Historia_Profesional { get; set; }
        public string? Ultimo_Grado_Estudios { get; set; }
        public string? Trayectoria { get; set; }
        public string? Vision_Funcion_Jurisdiccional { get; set; }
        public string? Vision_Imparticion_Justicia { get; set; }
        public string? Propuestas_Mejora_Funcion_Jurisdiccional { get; set; }
        public string? Expedientes_URL { get; set; }
        public bool? Usuario { get; set; }
        public bool? Autorizacion { get; set; }
        public DateTime? Fecha_Nacimiento { get; set; }
        public int Edad { get; set; }
        public string? Estatus_Grado_Estudios { get; set; }
        public bool? Activo { get; set; } = true;
    }
}
