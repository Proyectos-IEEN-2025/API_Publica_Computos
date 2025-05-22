using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Computos_Publica.Models.DTO.Computos.Dashboard
{
    public class Junta_Actas_Computadas
    {
        public int No_Oficina { get; set; }
        public string Junta { get; set; }
        public string Municipio { get; set; }
        public int Paquetes_Esperados { get; set; }
        public int Paquetes_Computados { get; set; }
        public int Paquetes_Cotejados { get; set; }
        public int Paquetes_Publicados { get; set; }
    }
}
