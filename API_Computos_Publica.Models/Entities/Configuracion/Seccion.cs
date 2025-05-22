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
    public class Seccion : Auditoria
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Distrito_Id { get; set; }
        [Required]
        public int Municipio_Id { get; set; }
        [Required]
        public int Demarcacion_Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Nombre { get; set; }
        [Required]
        [MaxLength(100)]
        public string Cabecera_Localidad { get; set; }
        public int Padron_Electoral { get; set; }
        public int Listado_Nominal { get; set; }
        [Required]
        [MaxLength(20)]
        public string Tipo_Seccion { get; set; }

        [ForeignKey("Municipio_Id")]
        public Municipio? Municipio { get; set; }

    }
}
