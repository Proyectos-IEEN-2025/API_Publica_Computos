using API_Computos_Publica.Models.Entities.Computos;
using API_Computos_Publica.Models.Entities.Configuracion;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace API_Computos_Publica.Data.DbContexts
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        #region Configuracion

        public DbSet<Estado> Estados { get; set; }//Repository//Controller
        public DbSet<Municipio> Municipios { get; set; }//Repository//Controller
        public DbSet<Oficina> Oficinas { get; set; }//Repository//Controller
        public DbSet<Seccion> Secciones { get; set; }//Repository//Controller
        public DbSet<Tipo_Casilla> Tipos_Casilla { get; set; }//Repository//Controller
        public DbSet<Casilla> Casillas { get; set; }//Repository//Controller
        public DbSet<Tipo_Eleccion> Tipos_Eleccion { get; set; }//Repository//Controller
        public DbSet<Candidato> Candidatos { get; set; }//Repository//Controller
        #endregion

        #region Computos
        public DbSet<Boleta> Boletas { get; set; }//Repository//DTO//Controller
        public DbSet<Paquete> Paquetes { get; set; }//Repository//DTO//Controller
        public DbSet<Paquete_Tipo_Eleccion> Paquetes_Tipos_Elecciones { get; set; }//Repository//DTO

        public DbSet<Candidatos_Tipo_Eleccion> Candidatos_Tipos_Elecciones { get; set; }//Repository//DTO//Controller

        public DbSet<Actas_Parciales> Actas_Parciales { get; set; }//Repository//DTO//Controller
        public DbSet<Actas_Estatales> Actas_Estatales { get; set; }//Repository//DTO//Controller
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Paquete>().HasQueryFilter(x => x.Eliminado != true);
            modelBuilder.Entity<Boleta>().HasQueryFilter(x => x.Eliminado != true);
            modelBuilder.Entity<Paquete_Tipo_Eleccion>().HasQueryFilter(x => x.Eliminado != true);
            modelBuilder.Entity<Estado>().HasQueryFilter(x => x.Eliminado != true);
            modelBuilder.Entity<Municipio>().HasQueryFilter(x => x.Eliminado != true);
            modelBuilder.Entity<Oficina>().HasQueryFilter(x => x.Eliminado != true);
            modelBuilder.Entity<Seccion>().HasQueryFilter(x => x.Eliminado != true);
            modelBuilder.Entity<Tipo_Casilla>().HasQueryFilter(x => x.Eliminado != true);
            modelBuilder.Entity<Casilla>().HasQueryFilter(x => x.Eliminado != true);
            modelBuilder.Entity<Tipo_Eleccion>().HasQueryFilter(x => x.Eliminado != true);
            modelBuilder.Entity<Candidato>().HasQueryFilter(x => x.Eliminado != true);
            base.OnModelCreating(modelBuilder);

        }
    }
}
