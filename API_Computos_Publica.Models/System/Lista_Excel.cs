using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Computos_Publica.Models.System
{
    public class Lista_Excel
    {
        public int ESTADO { get; set; }
        public string NOMBRE_ESTADO { get; set; }
        public string ID_MUNICIPIO { get; set; }
        public string MUNICIPIO { get; set; }
        public string SECCION { get; set; }
        public int ID_CASILLA { get; set; }
        public string TIPO_CASILLA { get; set; }
        public int EXT_CONTIGUA { get; set; } = 0;
        public string TIPO_ACTA { get; set; }
        public Dictionary<string, int> CANDIDATOS { get; set; } = [];
        public int NULOS { get; set; }
        public int TOTAL_VOTOS { get; set; }
        public int LISTA_NOMINAL { get; set; }
        public string ESTATUS_ACTA { get; set; }
    }
}
