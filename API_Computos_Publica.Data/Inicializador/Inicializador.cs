using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_Computos_Publica.Data.DbContexts;

namespace API_Computos_Publica.Data.Inicializador
{
    public class Inicializador : I_Inicializador
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<Inicializador> _logger;

        public Inicializador(ApplicationDbContext db, ILogger<Inicializador> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async void Inicializar()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.StackTrace + " " + ex.InnerException);
            }
        }
    }
}