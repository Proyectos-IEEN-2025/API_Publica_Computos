using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using API_Computos_Publica.Data.DbContexts;
using API_Computos_Publica.Data.Repository.Interfacez.Computos;
using API_Computos_Publica.Models.Entities.Computos;

namespace API_Computos_Publica.Data.Repository.Clases.Computos
{
    public class Boleta_Repository : Repository<Boleta>, IBoleta_Repository
    {
        private readonly ApplicationDbContext _db;
        public Boleta_Repository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
