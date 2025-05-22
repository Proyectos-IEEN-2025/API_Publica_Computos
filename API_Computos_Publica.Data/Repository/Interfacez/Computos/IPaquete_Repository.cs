using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_Computos_Publica.Models.Entities.Computos;

namespace API_Computos_Publica.Data.Repository.Interfacez.Computos
{
    public interface IPaquete_Repository : IRepository<Paquete>
    {
        Task SoftDelete(int Id, string Usuario);
        Task Update(Paquete paquete);
        Task CreatePaquetes();
        Task LiberarPaquete(int id);
        Task AsignarBina(int PaqueteId, int? BinaId);
        Task<Paquete> Data_Assign(Paquete Item);
        Task CrearPaquetesTiposEleccion();
    }
}
