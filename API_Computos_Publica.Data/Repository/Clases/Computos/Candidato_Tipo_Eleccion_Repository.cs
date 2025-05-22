using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_Computos_Publica.Data.DbContexts;
using API_Computos_Publica.Data.Repository.Interfacez.Computos;
using API_Computos_Publica.Models.Entities.Computos;

namespace API_Computos_Publica.Data.Repository.Clases.Computos
{
    public class Candidato_Tipo_Eleccion_Repository : Repository<Candidatos_Tipo_Eleccion>, ICandidato_Tipo_Eleccion_Repository
    {
        private readonly ApplicationDbContext _db;
        public Candidato_Tipo_Eleccion_Repository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
