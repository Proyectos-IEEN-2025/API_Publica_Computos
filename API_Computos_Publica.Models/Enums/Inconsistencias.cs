using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Computos_Publica.Models.Enums
{
    public enum Inconsistencias
    {
        Boleta_En_Blanco = 1,
        Marcas_Toda_Boleta = 2,
        Excede_Totalidad_Votos_Validos = 3,
        Diferentes_Formas_Votacion_Nula = 4
    }
}
