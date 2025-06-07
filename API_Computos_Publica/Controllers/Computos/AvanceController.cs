using API_Computos_Publica.Data.Repository.Interfacez;
using API_Computos_Publica.Models.DTO.Computos;
using API_Computos_Publica.Models.DTO.Configuracion;
using API_Computos_Publica.Models.Entities.Computos;
using API_Computos_Publica.Models.Entities.Configuracion;
using API_Computos_Publica.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace API_Computos_Publica.Controllers.Computos
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvanceController(IUnitOfWork ctx, ILogger<AvanceController> logger, IMapper mapper, IWebHostEnvironment host, IOutputCacheStore outputCacheStore) : ControllerBase
    {
        private readonly IUnitOfWork _ctx = ctx;
        private readonly ILogger<AvanceController> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IWebHostEnvironment _host = host;
        private readonly IOutputCacheStore _outputCacheStore = outputCacheStore;

        [HttpGet("AvanceEstatal/{TipoEleccionId:int}")]
        [OutputCache(PolicyName = "consultas")]
        public async Task<IActionResult> AvanceEstatal(int TipoEleccionId)
        {
            try
            {
                var Paquetes_Eleccion = new List<Paquete_Tipo_Eleccion>();
                var Votos_Candidatura = 0;

                var Paquetes_Adicional = new List<Paquete_Tipo_Eleccion_Adicional>();
                var Votos_Candidatos_Adicional = 0;

                var List_Elecciones = new List<int>() { 3, 4, 5, 6 };

                if (List_Elecciones.Contains(TipoEleccionId))
                {
                    var r = await _ctx.Candidato_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id));
                    Votos_Candidatura = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id))).Sum(x => x.Votos);
                    Paquetes_Eleccion = (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id))).ToList();

                    Votos_Candidatos_Adicional = (await _ctx.Candidato_Tipo_Eleccion_Adicional.GetAllasync(x => List_Elecciones.Contains(x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id))).Sum(x => x.Votos);
                    Paquetes_Adicional = (await _ctx.Paquete_Tipo_Eleccion_Adicional.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id))).ToList();
                }
                else
                {
                    Votos_Candidatura = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(x => x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id == TipoEleccionId)).Sum(x => x.Votos);
                    Paquetes_Eleccion = (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId)).ToList();

                    Votos_Candidatos_Adicional = (await _ctx.Candidato_Tipo_Eleccion_Adicional.GetAllasync(x => x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id == TipoEleccionId)).Sum(x => x.Votos);
                    Paquetes_Adicional = (await _ctx.Paquete_Tipo_Eleccion_Adicional.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId)).ToList();
                }

                var Paquetes_Publicados = Paquetes_Eleccion.Select(x => x.Paquete_Id);


                var Paquetes_Publicados_Adicional = Paquetes_Adicional.Select(x => x.Paquete_Id);

                var Casillas = await _ctx.Casilla.GetAllasync();
                var Paquetes = await _ctx.Paquete.GetAllasync(include: source => source.Include(x => x.Casilla.Seccion.Municipio));
                var Computadas = Paquetes.Where(x => x.Computada == true && Paquetes_Publicados.Contains(x.Id)).Count();
                var Esperadas = Paquetes.Count();
                var Avance = (decimal)Computadas / Esperadas;

                var Participacion = List_Elecciones.Contains(TipoEleccionId) ? Paquetes_Eleccion.Sum(x => x.Boletas_Capturadas) / 4 : Paquetes_Eleccion.Sum(x => x.Boletas_Capturadas);
                var ParticipacionAdicional = List_Elecciones.Contains(TipoEleccionId) ? Paquetes_Adicional.Sum(x => x.Boletas_Capturadas) / 4 : Paquetes_Adicional.Sum(x => x.Boletas_Capturadas);

                var Total_Participacion = Participacion + ParticipacionAdicional;
                var Porc_Participion = (decimal)Total_Participacion / Casillas.Sum(x => x.Listado_Nominal);

                var resultado = new Avance_DTO
                {
                    Participacion = Total_Participacion,
                    Porcentaje_Participacion = Porc_Participion * 100,
                    Lista_Nominal = Casillas.Sum(x => x.Listado_Nominal),
                    Votos_Candidatura = Votos_Candidatura + Votos_Candidatos_Adicional,
                    Votos_Nulos = (Paquetes_Eleccion.Sum(x => x.Votos_Nulos) + Paquetes_Adicional.Sum(x => x.Votos_Nulos)),
                    Total = Votos_Candidatura + Votos_Candidatos_Adicional + (Paquetes_Eleccion.Count() != 0 ? Paquetes_Eleccion.Sum(x => x.Votos_Nulos) : 0) + (Paquetes_Adicional.Count() != 0 ? Paquetes_Adicional.Sum(x => x.Votos_Nulos) : 0),
                    Avance = Avance,
                    Casillas_Computadas = Computadas,
                    Casillas_Esperadas = Esperadas,
                };


                return Ok(new { success = true, data = resultado });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = "Error en el servidor" });
            }
        }

        [HttpGet("AvanceJAO/{TipoEleccionId:int}/{OficinaId:int}")]
        [OutputCache(PolicyName = "consultas")]
        public async Task<IActionResult> AvanceJAO(int TipoEleccionId, int OficinaId)
        {
            try
            {
                var Paquetes_Eleccion = new List<Paquete_Tipo_Eleccion>();
                var Votos_Candidatura = 0;

                var Paquetes_Adicional = new List<Paquete_Tipo_Eleccion_Adicional>();
                var Votos_Candidatos_Adicional = 0;

                var List_Elecciones = new List<int>() { 3, 4, 5, 6 };

                var Oficina = await _ctx.Oficina.GetFirstOrdefaultAsync(x => x.Id == OficinaId, include: source => source.Include(x => x.Municipio));

                if (List_Elecciones.Contains(TipoEleccionId))
                {
                    Votos_Candidatura = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id)  && x.Paquete_Tipo_Eleccion.Paquete.Casilla.Municipio.Region == Oficina.Municipio.Region)).Sum(x => x.Votos);
                    Paquetes_Eleccion = (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id)  && x.Paquete.Casilla.Municipio.Region == Oficina.Municipio.Region)).ToList();

                    Votos_Candidatos_Adicional = (await _ctx.Candidato_Tipo_Eleccion_Adicional.GetAllasync(x => List_Elecciones.Contains(x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id)  && x.Paquete_Tipo_Eleccion.Paquete.Casilla.Municipio.Region == Oficina.Municipio.Region)).Sum(x => x.Votos);
                    Paquetes_Adicional = (await _ctx.Paquete_Tipo_Eleccion_Adicional.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id)  && x.Paquete.Casilla.Municipio.Region == Oficina.Municipio.Region)).ToList();
                }
                else
                {
                    Votos_Candidatura = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(x => x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id == TipoEleccionId  && x.Paquete_Tipo_Eleccion.Paquete.Casilla.Municipio.Region == Oficina.Municipio.Region)).Sum(x => x.Votos);
                    Paquetes_Eleccion = (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId  && x.Paquete.Casilla.Municipio.Region == Oficina.Municipio.Region)).ToList();

                    Votos_Candidatos_Adicional = (await _ctx.Candidato_Tipo_Eleccion_Adicional.GetAllasync(x => x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id == TipoEleccionId  && x.Paquete_Tipo_Eleccion.Paquete.Casilla.Municipio.Region == Oficina.Municipio.Region)).Sum(x => x.Votos);
                    Paquetes_Adicional = (await _ctx.Paquete_Tipo_Eleccion_Adicional.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId  && x.Paquete.Casilla.Municipio.Region == Oficina.Municipio.Region)).ToList();
                }

                var Paquetes_Publicados = Paquetes_Eleccion.Select(x => x.Paquete_Id);
                var Paquetes_Publicados_Adicional = Paquetes_Eleccion.Select(x => x.Paquete_Id);
                var Casillas = await _ctx.Casilla.GetAllasync(x => x.Municipio.Region == Oficina.Municipio.Region);

                var Paquetes = await _ctx.Paquete.GetAllasync(X => X.Casilla.Municipio.Region == Oficina.Municipio.Region, include: source => source.Include(x => x.Casilla.Seccion.Municipio));
                var Computadas = Paquetes.Where(x => x.Computada == true && Paquetes_Publicados.Contains(x.Id)).Count();
                var Esperadas = Paquetes.Count();
                var Avance = (decimal)Computadas / Esperadas;

                var Participacion = List_Elecciones.Contains(TipoEleccionId) ? Paquetes_Eleccion.Sum(x => x.Boletas_Capturadas) / 4 : Paquetes_Eleccion.Sum(x => x.Boletas_Capturadas);
                var ParticipacionAdicional = List_Elecciones.Contains(TipoEleccionId) ? Paquetes_Adicional.Sum(x => x.Boletas_Capturadas) / 4 : Paquetes_Adicional.Sum(x => x.Boletas_Capturadas);

                var Total_Participacion = Participacion + ParticipacionAdicional;
                var Porc_Participion = (decimal)Total_Participacion / Casillas.Sum(x => x.Listado_Nominal);
                var resultado = new Avance_DTO
                {
                    Participacion = Participacion + ParticipacionAdicional,
                    Porcentaje_Participacion = Porc_Participion * 100,
                    Lista_Nominal = Casillas.Sum(x => x.Listado_Nominal),
                    Votos_Candidatura = Votos_Candidatura + Votos_Candidatos_Adicional,
                    Votos_Nulos = (Paquetes_Eleccion.Sum(x => x.Votos_Nulos) + Paquetes_Adicional.Sum(x => x.Votos_Nulos)),
                    Total = Votos_Candidatura + Votos_Candidatos_Adicional + (Paquetes_Eleccion.Count() != 0 ? Paquetes_Eleccion.Sum(x => x.Votos_Nulos) : 0) + (Paquetes_Adicional.Count() != 0 ? Paquetes_Adicional.Sum(x => x.Votos_Nulos) : 0),
                    Avance = Avance,
                    Casillas_Computadas = Computadas,
                    Casillas_Esperadas = Esperadas,
                };


                return Ok(new { success = true, data = resultado });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = "Error en el servidor" });
            }
        }

        [HttpGet("AvanceMunicipio/{TipoEleccionId:int}/{MunicipioId:int}")]
        [OutputCache(PolicyName = "consultas")]
        public async Task<IActionResult> AvanceMunicipio(int TipoEleccionId, int MunicipioId)
        {
            try
            {
                var Paquetes_Eleccion = new List<Paquete_Tipo_Eleccion>();
                var Votos_Candidatura = 0;

                var Paquetes_Adicional = new List<Paquete_Tipo_Eleccion_Adicional>();
                var Votos_Candidatos_Adicional = 0; 
                
                var List_Elecciones = new List<int>() { 3, 4, 5, 6 };

                if (List_Elecciones.Contains(TipoEleccionId))
                {
                    Votos_Candidatura = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id)  && x.Paquete_Tipo_Eleccion.Paquete.Casilla.Municipio_Id == MunicipioId)).Sum(x => x.Votos);
                    Paquetes_Eleccion = (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id)  && x.Paquete.Casilla.Municipio_Id == MunicipioId)).ToList();

                    Votos_Candidatos_Adicional = (await _ctx.Candidato_Tipo_Eleccion_Adicional.GetAllasync(x => List_Elecciones.Contains(x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id)  && x.Paquete_Tipo_Eleccion.Paquete.Casilla.Municipio_Id == MunicipioId)).Sum(x => x.Votos);
                    Paquetes_Adicional = (await _ctx.Paquete_Tipo_Eleccion_Adicional.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id)  && x.Paquete.Casilla.Municipio_Id == MunicipioId)).ToList();
                }
                else
                {
                    Votos_Candidatura = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(x => x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id == TipoEleccionId  && x.Paquete_Tipo_Eleccion.Paquete.Casilla.Municipio_Id == MunicipioId)).Sum(x => x.Votos);
                    Paquetes_Eleccion = (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId  && x.Paquete.Casilla.Municipio_Id == MunicipioId)).ToList();

                    Votos_Candidatos_Adicional = (await _ctx.Candidato_Tipo_Eleccion_Adicional.GetAllasync(x => x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id == TipoEleccionId  && x.Paquete_Tipo_Eleccion.Paquete.Casilla.Municipio_Id == MunicipioId)).Sum(x => x.Votos);
                    Paquetes_Adicional = (await _ctx.Paquete_Tipo_Eleccion_Adicional.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId  && x.Paquete.Casilla.Municipio_Id == MunicipioId)).ToList();
                }

                var Paquetes_Publicados = Paquetes_Eleccion.Select(x => x.Paquete_Id);
                var Paquetes_Publicados_Adicional = Paquetes_Adicional.Select(x => x.Paquete_Id);

                var Casillas = await _ctx.Casilla.GetAllasync(x => x.Municipio_Id == MunicipioId);

                var Paquetes = await _ctx.Paquete.GetAllasync(X => X.Casilla.Municipio_Id == MunicipioId, include: source => source.Include(x => x.Casilla.Seccion.Municipio));
                var Computadas = Paquetes.Where(x => x.Computada == true && Paquetes_Publicados.Contains(x.Id)).Count();
                var Esperadas = Paquetes.Count();
                var Avance = (decimal)Computadas / Esperadas;

                var Participacion = List_Elecciones.Contains(TipoEleccionId) ? Paquetes_Eleccion.Sum(x => x.Boletas_Capturadas) / 4 : Paquetes_Eleccion.Sum(x => x.Boletas_Capturadas);
                var ParticipacionAdicional = List_Elecciones.Contains(TipoEleccionId) ? Paquetes_Adicional.Sum(x => x.Boletas_Capturadas) / 4 : Paquetes_Adicional.Sum(x => x.Boletas_Capturadas);

                var Total_Participacion = Participacion + ParticipacionAdicional;
                var Porc_Participion = (decimal)Total_Participacion / Casillas.Sum(x => x.Listado_Nominal);

                var resultado = new Avance_DTO
                {
                    Participacion = Participacion + ParticipacionAdicional,
                    Porcentaje_Participacion = Porc_Participion * 100,
                    Lista_Nominal = Casillas.Sum(x => x.Listado_Nominal),
                    Votos_Candidatura = Votos_Candidatura + Votos_Candidatos_Adicional,
                    Votos_Nulos = (Paquetes_Eleccion.Sum(x => x.Votos_Nulos) + Paquetes_Adicional.Sum(x => x.Votos_Nulos)),
                    Total = Votos_Candidatura + Votos_Candidatos_Adicional + (Paquetes_Eleccion.Count() != 0 ? Paquetes_Eleccion.Sum(x => x.Votos_Nulos) : 0) + (Paquetes_Adicional.Count() != 0 ? Paquetes_Adicional.Sum(x => x.Votos_Nulos) : 0),
                    Avance = Avance,
                    Casillas_Computadas = Computadas,
                    Casillas_Esperadas = Esperadas,
                };


                return Ok(new { success = true, data = resultado });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = "Error en el servidor" });
            }
        }

        [HttpGet("Hora_Consulta")]
        public async Task<IActionResult> Hora_Consulta()
        {
            try
            {
                DateTime ahoraUtc = DateTime.UtcNow;

                TimeZoneInfo zonaNayarit = TimeZoneInfo.FindSystemTimeZoneById("America/Mazatlan");
                DateTime fechaNayarit = TimeZoneInfo.ConvertTimeFromUtc(ahoraUtc, zonaNayarit);

                // Redondear hacia abajo a bloques de 15 minutos
                int minutosRedondeados = (fechaNayarit.Minute / 15) * 15;
                DateTime fechaRedondeada = new DateTime(
                    fechaNayarit.Year,
                    fechaNayarit.Month,
                    fechaNayarit.Day,
                    fechaNayarit.Hour + 1,
                    minutosRedondeados,
                    0
                );

                // Formato
                var cultura = new CultureInfo("es-MX");
                string linea1 = fechaRedondeada.ToString("dd MMMM yyyy", cultura);
                string linea2 = "Tiempo de Nayarit";
                string linea3 = fechaRedondeada.ToString("HH:mm") + " h UTC-7";

                var resultado = new
                {
                    success = true,
                    data = new
                    {
                        fecha = linea1,
                        zona = linea2,
                        hora = linea3
                    }
                };

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = Commons.MensajeError });
            }
        }

        [HttpGet("Borrar_Cache")]
        public async Task<IActionResult> Borrar_Cache()
        {
            try
            {
                await outputCacheStore.EvictByTagAsync("consultas", default);
                return Ok(new { success = true, data = "Cache Eliminado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = Commons.MensajeError });
            }
        }
    }
}