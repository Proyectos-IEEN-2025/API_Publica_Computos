using Microsoft.EntityFrameworkCore;
using API_Computos_Publica.Data.DbContexts;
using API_Computos_Publica.Data.Repository.Interfacez.Configuracion;
using API_Computos_Publica.Models.Entities.Configuracion;

namespace API_Computos_Publica.Data.Repository.Clases.Configuracion
{
    public class Estado_Repository : Repository<Estado>, IEstado_Repository
    {
        private readonly ApplicationDbContext _db;
        public Estado_Repository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
