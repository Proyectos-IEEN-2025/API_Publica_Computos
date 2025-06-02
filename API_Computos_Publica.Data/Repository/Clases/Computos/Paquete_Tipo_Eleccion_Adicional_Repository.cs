using API_Computos_Publica.Data.DbContexts;
using API_Computos_Publica.Data.Repository.Interfacez.Computos;
using API_Computos_Publica.Models.Entities.Computos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Computos_Publica.Data.Repository.Clases.Computos
{
    public class Paquete_Tipo_Eleccion_Adicional_Repository : Repository<Paquete_Tipo_Eleccion_Adicional>, IPaquete_Tipo_Eleccion_Adicional_Repository
    {
        private readonly ApplicationDbContext _db;
        public Paquete_Tipo_Eleccion_Adicional_Repository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
