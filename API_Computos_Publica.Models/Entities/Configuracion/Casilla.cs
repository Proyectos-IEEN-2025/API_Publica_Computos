using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_Computos_Publica.Models.System;

namespace API_Computos_Publica.Models.Entities.Configuracion
{
    public class Casilla : Auditoria
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Municipio_Id { get; set; }
        [Required]
        public int Seccion_Id { get; set; }
        [Required]
        public int Tipo_Casilla_Id { get; set; }
        [Required]
        public int No_Casilla { get; set; }
        [Required]
        public int Extension_Contigua { get; set; }
        [Required]
        public int Listado_Nominal { get; set; }
        [Required]
        public int Padron_Electoral { get; set; }
        [Required]
        public int Boletas_Entregadas { get; set; }
        [Required]
        public bool Activo { get; set; } = false;
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }
        [Required]
        [MaxLength(50)]
        public string Tipo_Seccion { get; set; }
        [MaxLength(250)]
        public string Domicilio { get; set; }
        [MaxLength(250)]
        public string Referencia { get; set; }
        public int? Tipo_Lugar { get; set; }
        [MaxLength(250)]
        public string? Ubicacion { get; set; }
        public double? Latitud_Cartografica { get; set; }
        public double? Latitud_Google { get; set; }
        public double? Longitud_Cartografica { get; set; }
        public double? Longitud_Google { get; set; }

        [ForeignKey("Municipio_Id")]
        public Municipio? Municipio { get; set; }
        [ForeignKey("Seccion_Id")]
        public Seccion? Seccion { get; set; }
        [ForeignKey("Tipo_Casilla_Id")]
        public Tipo_Casilla? Tipos_Casilla { get; set; }
    }
}
