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
    public class Casilla_DTO
    {
        public int Id { get; set; }
        public int Municipio_Id { get; set; }
        public string Municipio { get; set; }
        public int Seccion_Id { get; set; }
        public string Seccion { get; set; }
        public int Tipo_Casilla_Id { get; set; }
        public string Tipo_Casilla { get; set; }
        public int No_Casilla { get; set; }
        public int Extension_Contigua { get; set; }
        public int Listado_Nominal { get; set; }
        public int Padron_Electoral { get; set; }
        public int Boletas_Entregadas { get; set; }
        public bool Activo { get; set; } = false;
        public string Nombre { get; set; }
        public string Tipo_Seccion { get; set; }
        public string Domicilio { get; set; }
        public string Referencia { get; set; }
        public int? Tipo_Lugar { get; set; }
        public string? Ubicacion { get; set; }
        public double? Latitud_Cartografica { get; set; }
        public double? Latitud_Google { get; set; }
        public double? Longitud_Cartografica { get; set; }
        public double? Longitud_Google { get; set; }
    }
}
