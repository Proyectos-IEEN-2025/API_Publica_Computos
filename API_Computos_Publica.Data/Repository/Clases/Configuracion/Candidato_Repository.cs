using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using API_Computos_Publica.Data.DbContexts;
using API_Computos_Publica.Data.Repository.Interfacez;
using API_Computos_Publica.Data.Repository.Interfacez.Configuracion;
using API_Computos_Publica.Models.Entities.Configuracion;

namespace API_Computos_Publica.Data.Repository.Clases.Configuracion
{
    public class Candidato_Repository : Repository<Candidato>, ICandidato_Repository
    {
        private readonly ApplicationDbContext _db;
        public Candidato_Repository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
