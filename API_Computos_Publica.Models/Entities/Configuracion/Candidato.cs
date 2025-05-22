using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API_Computos_Publica.Models.System;

namespace API_Computos_Publica.Models.Entities.Configuracion
{
    public class Candidato : Auditoria
    {
        [Key]
        public int Id { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
        [Required]
        public int Tipo_Eleccion_Id { get; set; }
        public string? Poder_Postulante { get; set; }
        public string? No_Formula { get; set; }
        [StringLength(50)]
        [Required]
        public string? Nombres { get; set; }
        [StringLength(50)]
        public string? Apellido_Paterno { get; set; }
        [StringLength(50)]
        public string? Apellido_Materno { get; set; }
        [StringLength(50)]
        [Required]
        public string? Sexo { get; set; }
        [StringLength(18)]
        public string? CURP { get; set; }
        [StringLength(13)]
        public string? RFC { get; set; }
        [StringLength(500)]
        public string? Especialidad { get; set; }
        [StringLength(18)]
        public string? Clave_Elector { get; set; }
        [StringLength(10)]
        public string? Telefono { get; set; }
        [EmailAddress]
        [Required]
        public string? Correo { get; set; }
        [Url]
        public string? Pagina_Web { get; set; }
        [StringLength(50)]
        public string? Calle { get; set; }
        [StringLength(10)]
        public string? No_Exterior { get; set; }
        [StringLength(50)]
        public string? No_Interior { get; set; }
        public string? Colonia { get; set; }
        public string? Codigo_Postal { get; set; }
        public string?  Municipio { get; set; }
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
        public bool? Autorizacion { get; set; } = false;
        public string? Estatus_Grado_Estudios { get; set; }
        public bool? Activo { get; set; } = true;
        public int? Orden { get; set; }
        public DateTime? Fecha_Nacimiento { get; set; }
        [ForeignKey(nameof(Tipo_Eleccion_Id))]
        public Tipo_Eleccion? Tipo_Eleccion { get; set; }

    }
}
