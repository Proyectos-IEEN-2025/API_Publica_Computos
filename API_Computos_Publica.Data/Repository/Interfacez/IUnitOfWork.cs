using API_Computos_Publica.Data.Repository.Interfacez.Computos;
using API_Computos_Publica.Data.Repository.Interfacez.Configuracion;

namespace API_Computos_Publica.Data.Repository.Interfacez
{
    public interface IUnitOfWork : IDisposable
    {
        #region Catalogos
        IEstado_Repository Estado { get; }
        IMunicipio_Repository Municipio { get; }
        IOficina_Repository Oficina { get; }

        ISeccion_Repository Seccion { get; }
        ITipo_Casilla_Repository Tipo_Casilla { get; }
        ICasilla_Repository Casilla { get; }
        ITipo_Eleccion_Repository Tipo_Eleccion { get; }
        ICandidato_Repository Candidato { get; }
        #endregion

        #region Computos
        IBoleta_Repository Boleta { get; }
        IPaquete_Repository Paquete { get; }
        IPaquete_Tipo_Eleccion_Repository Paquete_Tipo_Eleccion { get; }
        IPaquete_Tipo_Eleccion_Adicional_Repository Paquete_Tipo_Eleccion_Adicional { get; }
        ICandidato_Tipo_Eleccion_Repository Candidato_Tipo_Eleccion { get; }
        ICandidato_Tipo_Eleccion_Adicional_Repository Candidato_Tipo_Eleccion_Adicional { get; }
        IActa_Estatal_Repository Acta_Estatal { get; }
        IActa_Parcial_Repository Acta_Parcial { get; }
        #endregion

        void Save();
        Task<int> SaveAsync();
    }
}

