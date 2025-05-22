using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Computos_Publica.Models.DTO.Commons
{
    public class Importacion_Excel
    {
        [Required]
        public IFormFile Excel { get; set; }

    }
}
