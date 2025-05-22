using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Computos_Publica.Models.DTO.Commons
{
    public class Respuesta_Transaccion
    {
        public bool Succes { get; set; }
        public dynamic? Data { get; set; }
        public dynamic? Data_2 { get; set; }
        public Exception? Exception { get; set; }
    }
}
