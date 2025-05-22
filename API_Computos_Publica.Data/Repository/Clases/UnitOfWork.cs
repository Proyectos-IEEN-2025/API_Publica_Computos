using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_Computos_Publica.Data.DbContexts;
using API_Computos_Publica.Data.Repository.Clases.Computos;
using API_Computos_Publica.Data.Repository.Clases.Configuracion;
using API_Computos_Publica.Data.Repository.Interfacez;
using API_Computos_Publica.Data.Repository.Interfacez.Computos;
using API_Computos_Publica.Data.Repository.Interfacez.Configuracion;

namespace API_Computos_Publica.Data.Repository.Clases
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;

            #region Catalogos

            Estado = new Estado_Repository(_db);
            Municipio = new Municipio_Repository(_db);
            Oficina = new Oficina_Repository(_db);
            Seccion = new Seccion_Repository(_db);
            Tipo_Casilla = new Tipo_Casilla_Repository(_db);
            Casilla = new Casilla_Repository(_db);
            Tipo_Eleccion = new Tipo_Eleccion_Repository(_db);
            Candidato = new Candidato_Repository(_db);
            #endregion

            #region Computos
            Boleta = new Boleta_Repository(_db);
            Paquete = new Paquete_Repository(_db);
            Paquete_Tipo_Eleccion = new Paquete_Tipo_Eleccion_Repository(_db);
            Candidato_Tipo_Eleccion = new Candidato_Tipo_Eleccion_Repository(_db);
            Acta_Estatal = new Acta_Estatal_Repository(_db);
            Acta_Parcial = new Acta_Parcial_Repository(_db);
            #endregion
        }

        #region Catalogos

        public IBoleta_Repository Boleta { get; private set; }

        public IEstado_Repository Estado { get; private set; }

        public IMunicipio_Repository Municipio { get; private set; }

        public IOficina_Repository Oficina { get; private set; }

        public ISeccion_Repository Seccion { get; private set; }

        public ITipo_Casilla_Repository Tipo_Casilla { get; private set; }

        public ICasilla_Repository Casilla { get; private set; }

        public ITipo_Eleccion_Repository Tipo_Eleccion { get; private set; }

        public ICandidato_Repository Candidato { get; private set; }

        public IPaquete_Repository Paquete { get; private set; }

        public IPaquete_Tipo_Eleccion_Repository Paquete_Tipo_Eleccion { get; private set; }

        public ICandidato_Tipo_Eleccion_Repository Candidato_Tipo_Eleccion { get; private set; }

        public IActa_Estatal_Repository Acta_Estatal { get; private set; }

        public IActa_Parcial_Repository Acta_Parcial { get; private set; }
        #endregion
        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
