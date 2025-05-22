

using Microsoft.EntityFrameworkCore;
using API_Computos_Publica.Data.DbContexts;
using API_Computos_Publica.Data.Repository.Interfacez.Configuracion;
using API_Computos_Publica.Models.Entities.Configuracion;

namespace API_Computos_Publica.Data.Repository.Clases.Configuracion
{
    public class Casilla_Repository : Repository<Casilla>, ICasilla_Repository
    {
        private readonly ApplicationDbContext _db;
        public Casilla_Repository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
