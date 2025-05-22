using Microsoft.EntityFrameworkCore;
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
    public class Paquete_Repository : Repository<Paquete>, IPaquete_Repository
    {
        private readonly ApplicationDbContext _db;
        public Paquete_Repository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Paquete> Data_Assign(Paquete Item)
        {
            var Paquete = await _db.Paquetes
                .Include(p => p.Paquetes_Tipos_Elecciones)
                .ThenInclude(pt => pt.Tipo_Eleccion)
                .FirstOrDefaultAsync(p => p.Id == Item.Id);

            if (Paquete != null)
            {
                Paquete.Bina_Id = Item.Bina_Id;
                Paquete.Mesa_Trabajo_Id = Item.Mesa_Trabajo_Id;
                Paquete.Usuario_Modificacion = Item.Usuario_Modificacion;
                Paquete.Fecha_Modificacion = DateTime.Now;
            }

            return Paquete;
        }

        public async Task CreatePaquetes()
        {
            var Casillas = await _db.Casillas.Where(x => x.Eliminado == false).ToListAsync();
            var Tipos_Elecciones = await _db.Tipos_Eleccion.ToListAsync();
            var Paquetes = Casillas.Select(c => new Paquete
            {
                Casilla_Id = c.Id,
                Usuario_Registro = "Sistema",
                Paquetes_Tipos_Elecciones = Tipos_Elecciones.Select(te => new Paquete_Tipo_Eleccion
                {
                    Tipo_Eleccion_Id = te.Id,
                    Usuario_Registro = "Sistema"
                }).ToList()
            }).ToList();
            await _db.Paquetes.AddRangeAsync(Paquetes);  
            await _db.SaveChangesAsync();
        }

        public async Task SoftDelete(int Id, string Usuario)
        {
            var Itemdb = await _db.Paquetes.FindAsync(Id);
            Itemdb.Eliminado = true;
            Itemdb.Fecha_Eliminacion = DateTime.Now;
            Itemdb.Usuario_Eliminacion = Usuario;
        }

        public async Task Update(Paquete paquete)
        {
            var Itemdb = await _db.Paquetes.FindAsync(paquete.Id);
            Itemdb.Bina_Id = paquete.Bina_Id;
            Itemdb.Usuario_Modificacion = paquete.Usuario_Modificacion;
            Itemdb.Fecha_Modificacion = DateTime.Now;
            Itemdb.Mesa_Trabajo_Id = paquete.Mesa_Trabajo_Id;
            Itemdb.Bina_Id = paquete.Bina_Id;

            Itemdb.Total_Personas_Voto = paquete.Total_Personas_Voto;
            Itemdb.Votos_Magistratura_Tribunal_Diciplina = paquete.Votos_Magistratura_Tribunal_Diciplina;
            Itemdb.Votos_Magistratura_Tribunal_Superior_Justicia = paquete.Votos_Magistratura_Tribunal_Superior_Justicia;
            Itemdb.Votos_Juezas_Jueces = paquete.Votos_Juezas_Jueces;
        }

        public async Task AsignarBina(int PaqueteId, int? BinaId)
        {
            var ItemDb = await _db.Paquetes.FindAsync(PaqueteId);
            ItemDb.Bina_Id = BinaId;
        }

        public async Task LiberarPaquete(int Id)
        {
            var Itemdb = await _db.Paquetes.FindAsync(Id);
            Itemdb.Computada = true;
        }

        public async Task CrearPaquetesTiposEleccion()
        {
            var Paquetes = await _db.Paquetes.ToListAsync();
            var Tipos_Elecciones = await _db.Tipos_Eleccion.ToListAsync();
            foreach (var Paquete in Paquetes)
            {
                var Paquete_Tipos_Elecciones = Tipos_Elecciones.Select(te => new Paquete_Tipo_Eleccion
                {
                    Tipo_Eleccion_Id = te.Id,
                    Paquete_Id = Paquete.Id,
                    Usuario_Registro = "Sistema"
                }).ToList();
                await _db.Paquetes_Tipos_Elecciones.AddRangeAsync(Paquete_Tipos_Elecciones);
            }
        }
    }
}
