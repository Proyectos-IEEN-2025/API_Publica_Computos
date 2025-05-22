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
    public class Acta_Parcial_Repository : Repository<Actas_Parciales>, IActa_Parcial_Repository
    {

        private readonly ApplicationDbContext _db;
        public Acta_Parcial_Repository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
