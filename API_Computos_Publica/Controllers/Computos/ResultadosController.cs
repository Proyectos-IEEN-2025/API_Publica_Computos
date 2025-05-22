using AutoMapper;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Spreadsheet;
using iText.IO.Util;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using API_Computos_Publica.Data.Repository.Interfacez;
using API_Computos_Publica.Models.DTO.Computos;
using API_Computos_Publica.Models.Entities.Computos;
using API_Computos_Publica.Models.Entities.Configuracion;
using API_Computos_Publica.Models.System;
using API_Computos_Publica.Utils;
using Microsoft.AspNetCore.OutputCaching;

namespace API_Computos_Publica.Controllers.Computos
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultadosController(IUnitOfWork ctx, ILogger<ResultadosController> logger, IMapper mapper, IWebHostEnvironment host) : ControllerBase
    {
        private readonly IUnitOfWork _ctx = ctx;
        private readonly ILogger<ResultadosController> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IWebHostEnvironment _host = host;

        [HttpGet("PaquetesTipoEleccion/{TipoEleccionId:int}")]
        [OutputCache(PolicyName = "consultas")]
        public async Task<IActionResult> PaquetesTipoEleccion(int TipoEleccionId)
        {
            try
            {
                var Paquetes = await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId, include: source => source.Include(x => x.Paquete.Casilla.Seccion.Municipio));
                var Paquetes_DTO = _mapper.Map<List<Paquete_Tipo_Eleccion_DTO>>(Paquetes);
                return Ok(new { success = true, data = Paquetes_DTO });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = "Error en el servidor" });
            }
        }

        [HttpGet("ResultadosByEleccionPaquete/{TipoEleccionId:int}/{PaqueteId:int}")]
        [OutputCache(PolicyName = "consultas")]
        public async Task<IActionResult> ResultadosByEleccionPaquete(int TipoEleccionId, int PaqueteId)
        {
            try
            {
                var PaquetesId = (await _ctx.Boleta.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete_Id == PaqueteId && x.Adicional == false)).Select(x => x.Paquete_Id).Distinct().ToList();

                var List_Elecciones = new List<int>() { 3, 4, 5, 6 };
                var Tipo_Eleccion_Paquetes = new List<Paquete_Tipo_Eleccion>();
                var Tipo_Eleccion_Paquete = await _ctx.Paquete_Tipo_Eleccion.GetFirstOrdefaultAsync(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete_Id == PaqueteId);
                var Votos_Candidatos = new List<Candidatos_Tipo_Eleccion>();
                if (List_Elecciones.Contains(TipoEleccionId))
                {
                    Tipo_Eleccion_Paquetes = (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id) && PaquetesId.Contains(x.Paquete_Id))).ToList();
                    Votos_Candidatos = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(
                                               x => List_Elecciones.Contains(x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id) && Tipo_Eleccion_Paquetes.Select(x => x.Id).Contains(x.Paquete_Tipo_Eleccion_Id),
                                                                      include: source => source.Include(x => x.Candidato),
                                                                                             orderBy: source => source.OrderBy(x => x.Candidato.Orden).ThenBy(x => x.Candidato.Tipo_Eleccion_Id))).ToList();
                }
                else
                {
                    Tipo_Eleccion_Paquetes = (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId && PaquetesId.Contains(x.Paquete_Id))).ToList();
                    Votos_Candidatos = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(
                                               x => x.Paquete_Tipo_Eleccion_Id == Tipo_Eleccion_Paquetes.First().Id,
                                                                      include: source => source.Include(x => x.Candidato),
                                                                                             orderBy: source => source.OrderBy(x => x.Candidato.Orden).ThenBy(x => x.Candidato.Tipo_Eleccion_Id))).ToList();
                }
                var Tipo_Eleccion_Paquete_DTO = _mapper.Map<Paquete_Tipo_Eleccion_DTO>(Tipo_Eleccion_Paquete);
                var Votos_Candidatos_DTO = _mapper.Map<List<Candidato_Tipo_Eleccion_DTO>>(Votos_Candidatos);
                var Resultados = new Resultados_Paquete_Tipo_Eleccion_DTO
                {
                    Paquete_Tipo_Eleccion = Tipo_Eleccion_Paquete_DTO,
                    Candidatos = Votos_Candidatos_DTO,
                    Ruta_Acta = Tipo_Eleccion_Paquete.Acta_Url_Local

                };
                return Ok(new { success = true, data = Resultados });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = "Error en el servidor" });
            }
        }

        [HttpGet("ResultadosByJuntaEleccion/{OficinaId:int}/{TipoEleccionId:int}")]
        [OutputCache(PolicyName = "consultas")]
        public async Task<IActionResult> ResultadosByJuntaTipoEleccion(int OficinaId, int TipoEleccionId)
        {
            try
            {
                var Oficina = await _ctx.Oficina.GetFirstOrdefaultAsync(x => x.Id == OficinaId, include: source => source.Include(x => x.Municipio));
                var PaquetesId = (await _ctx.Boleta.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete.Casilla.Municipio_Id == Oficina.Municipio_Id && x.Adicional == false, include: source => source.Include(x => x.Paquete).ThenInclude(x => x.Casilla))).Select(x => x.Paquete_Id).Distinct().ToList();

                var Acta = await _ctx.Acta_Parcial.GetFirstOrdefaultAsync(x => x.Oficina_Id == OficinaId && x.Tipo_Eleccion_Id == TipoEleccionId);
                var Tipo_Eleccion = await _ctx.Tipo_Eleccion.GetFirstOrdefaultAsync(x => x.Id == TipoEleccionId);
                var Paquetes = await _ctx.Paquete.GetAllasync(
                   x => x.Casilla.Seccion.Municipio.Region == Oficina.Municipio.Region && x.Computada == true,
                   include: source => source.Include(x => x.Casilla.Seccion.Municipio));
                var Tipo_Eleccion_Paquetes = new List<Paquete_Tipo_Eleccion>();
                var Votos_Candidatos = new List<Candidatos_Tipo_Eleccion>();
                var List_Elecciones = new List<int>() { 3, 4, 5, 6 };

                if (List_Elecciones.Contains(TipoEleccionId))
                {
                    Votos_Candidatos = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id) && x.Paquete_Tipo_Eleccion.Paquete.Casilla.Municipio.Region == Oficina.Municipio.Region,
                       include: source => source.Include(x => x.Candidato).Include(x => x.Paquete_Tipo_Eleccion).ThenInclude(x => x.Paquete).ThenInclude(x => x.Casilla).ThenInclude(x => x.Municipio),
                       orderBy: source => source.OrderBy(x => x.Candidato.Orden).ThenBy(x => x.Candidato.Tipo_Eleccion_Id))).ToList();

                    Tipo_Eleccion_Paquetes = (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id) && x.Paquete.Casilla.Municipio.Region == Oficina.Municipio.Region && PaquetesId.Contains(x.Paquete_Id),
                          include: source => source.Include(x => x.Paquete.Casilla.Seccion.Municipio),
                          orderBy: source => source.OrderBy(x => x.Id))).ToList();
                }
                else
                {
                    Votos_Candidatos = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(x => x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete_Tipo_Eleccion.Paquete.Casilla.Municipio.Region == Oficina.Municipio.Region,
                        include: source => source.Include(x => x.Candidato).Include(x => x.Paquete_Tipo_Eleccion).ThenInclude(x => x.Paquete).ThenInclude(x => x.Casilla).ThenInclude(x => x.Municipio),
                        orderBy: source => source.OrderBy(x => x.Candidato.Orden).ThenBy(x => x.Candidato.Tipo_Eleccion_Id))).ToList();

                    Tipo_Eleccion_Paquetes = (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete.Casilla.Municipio.Region == Oficina.Municipio.Region && PaquetesId.Contains(x.Paquete_Id),
                        include: source => source.Include(x => x.Paquete.Casilla.Seccion.Municipio),
                        orderBy: source => source.OrderBy(x => x.Id))).ToList();
                }

                var Candidatos_Unicos = Votos_Candidatos.GroupBy(x => new { x.Candidato_Id, x.Candidato.Nombres, x.Candidato.Apellido_Paterno, x.Candidato.Apellido_Materno, x.Candidato.No_Formula }).Select(g => new { Candidato_Id = g.Key, Candidato = $"{g.Key.Nombres} {g.Key.Apellido_Paterno} {g.Key.Apellido_Materno}", No_Formula = g.Key.No_Formula, Votos = g.Sum(x => x.Votos) }).ToList();


                var Resultados = new
                {
                    JAO = new
                    {
                        Oficina_Id = Oficina.Id,
                        No_Oficina = Oficina.No_Oficina,
                        Oficina = Oficina.Nombre,
                        Tipo_Eleccion_Id = Tipo_Eleccion.Id,
                        Tipo_Eleccion = Tipo_Eleccion.Nombre,
                        boletas_Sobrantes = Tipo_Eleccion_Paquetes.Sum(x => x.Boletas_Sobrantes),
                        boletas_Capturadas = Tipo_Eleccion_Paquetes.Sum(x => x.Boletas_Capturadas),
                        votos_Nulos = Tipo_Eleccion_Paquetes.Sum(x => x.Votos_Nulos),
                        campos_No_Utilizados = Tipo_Eleccion_Paquetes.Sum(x => x.Campos_No_Utilizados),
                        total_Votos = Tipo_Eleccion_Paquetes.Sum(x => x.Total_Votos),
                        computadas = Paquetes.Count(),
                        acta_Url = Acta != null ? Acta.Acta_Url : "",
                    },
                    ruta_Acta = Acta != null ? Acta.Acta_Url_Local : "",
                    Candidatos = Candidatos_Unicos,
                };
                return Ok(new { success = true, data = Resultados });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = "Error en el servidor" });
            }
        }

        [HttpGet("ResultadosByMunicipio/{MunicipioId:int}/{TipoEleccionId:int}")]
        [OutputCache(PolicyName = "consultas")]
        public async Task<IActionResult> ResultadosByMunicipio(int MunicipioId, int TipoEleccionId)
        {
            try
            {
                var Municipio = await _ctx.Municipio.GetFirstOrdefaultAsync(x => x.Id == MunicipioId);
                var Tipo_Eleccion = await _ctx.Tipo_Eleccion.GetFirstOrdefaultAsync(x => x.Id == TipoEleccionId);
                var Paquetes = await _ctx.Paquete.GetAllasync(
                   x => x.Casilla.Seccion.Municipio_Id == MunicipioId && x.Computada == true,
                   include: source => source.Include(x => x.Casilla.Seccion.Municipio));
                var Tipo_Eleccion_Paquetes = new List<Paquete_Tipo_Eleccion>();
                var Votos_Candidatos = new List<Candidatos_Tipo_Eleccion>();
                var List_Elecciones = new List<int>() { 3, 4, 5, 6 };

                if (List_Elecciones.Contains(TipoEleccionId))
                {
                    Votos_Candidatos = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id) && x.Paquete_Tipo_Eleccion.Paquete.Casilla.Municipio_Id == MunicipioId,
                       include: source => source.Include(x => x.Candidato).Include(x => x.Paquete_Tipo_Eleccion).ThenInclude(x => x.Paquete).ThenInclude(x => x.Casilla).ThenInclude(x => x.Municipio),
                       orderBy: source => source.OrderBy(x => x.Candidato.Orden).ThenBy(x => x.Candidato.Tipo_Eleccion_Id))).ToList();

                    Tipo_Eleccion_Paquetes = (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id) && x.Paquete.Casilla.Municipio_Id == MunicipioId,
                          include: source => source.Include(x => x.Paquete.Casilla.Seccion.Municipio),
                          orderBy: source => source.OrderBy(x => x.Id))).ToList();
                }
                else
                {
                    Votos_Candidatos = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(x => x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete_Tipo_Eleccion.Paquete.Casilla.Municipio_Id == MunicipioId,
                        include: source => source.Include(x => x.Candidato).Include(x => x.Paquete_Tipo_Eleccion).ThenInclude(x => x.Paquete).ThenInclude(x => x.Casilla).ThenInclude(x => x.Municipio),
                        orderBy: source => source.OrderBy(x => x.Candidato.Orden).ThenBy(x => x.Candidato.Tipo_Eleccion_Id))).ToList();

                    Tipo_Eleccion_Paquetes = (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete.Casilla.Municipio_Id == MunicipioId,
                        include: source => source.Include(x => x.Paquete.Casilla.Seccion.Municipio),
                        orderBy: source => source.OrderBy(x => x.Id))).ToList();
                }

                var Candidatos_Unicos = Votos_Candidatos.GroupBy(x => new { x.Candidato_Id, x.Candidato.Nombres, x.Candidato.Apellido_Paterno, x.Candidato.Apellido_Materno, x.Candidato.No_Formula }).Select(g => new { Candidato_Id = g.Key, Candidato = $"{g.Key.Nombres} {g.Key.Apellido_Paterno} {g.Key.Apellido_Materno}", No_Formula = g.Key.No_Formula, Votos = g.Sum(x => x.Votos) }).ToList();


                var Resultados = new
                {
                    Municipio = new
                    {
                        Municipio_Id = Municipio.Id,
                        Municipio = Municipio.Nombre,
                        Tipo_Eleccion_Id = Tipo_Eleccion.Id,
                        Tipo_Eleccion = Tipo_Eleccion.Nombre,
                        boletas_Sobrantes = Tipo_Eleccion_Paquetes.Sum(x => x.Boletas_Sobrantes),
                        boletas_Capturadas = Tipo_Eleccion_Paquetes.Sum(x => x.Boletas_Capturadas),
                        votos_Nulos = Tipo_Eleccion_Paquetes.Sum(x => x.Votos_Nulos),
                        campos_No_Utilizados = Tipo_Eleccion_Paquetes.Sum(x => x.Campos_No_Utilizados),
                        total_Votos = Tipo_Eleccion_Paquetes.Sum(x => x.Total_Votos),
                        computadas = Paquetes.Count(),
                    },
                    Candidatos = Candidatos_Unicos,
                };
                return Ok(new { success = true, data = Resultados });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = "Error en el servidor" });
            }
        }

        [HttpGet("ResultadosBySeccion/{SeccionId:int}/{TipoEleccionId:int}")]
        [OutputCache(PolicyName = "consultas")]
        public async Task<IActionResult> ResultadosBySeccion(int SeccionId, int TipoEleccionId)
        {
            try
            {
                var Seccion = await _ctx.Seccion.GetFirstOrdefaultAsync(x => x.Id == SeccionId);
                var Tipo_Eleccion = await _ctx.Tipo_Eleccion.GetFirstOrdefaultAsync(x => x.Id == TipoEleccionId);
                var Paquetes = await _ctx.Paquete.GetAllasync(
                   x => x.Casilla.Seccion_Id == SeccionId && x.Computada == true,
                   include: source => source.Include(x => x.Casilla.Seccion.Municipio));
                var Tipo_Eleccion_Paquetes = new List<Paquete_Tipo_Eleccion>();
                var Votos_Candidatos = new List<Candidatos_Tipo_Eleccion>();
                var List_Elecciones = new List<int>() { 3, 4, 5, 6 };

                if (List_Elecciones.Contains(TipoEleccionId))
                {
                    Votos_Candidatos = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id) && x.Paquete_Tipo_Eleccion.Paquete.Casilla.Seccion_Id == SeccionId,
                       include: source => source.Include(x => x.Candidato).Include(x => x.Paquete_Tipo_Eleccion).ThenInclude(x => x.Paquete).ThenInclude(x => x.Casilla).ThenInclude(x => x.Municipio),
                       orderBy: source => source.OrderBy(x => x.Candidato.Orden).ThenBy(x => x.Candidato.Tipo_Eleccion_Id))).ToList();

                    Tipo_Eleccion_Paquetes = (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id) && x.Paquete.Casilla.Seccion_Id == SeccionId,
                          include: source => source.Include(x => x.Paquete.Casilla.Seccion.Municipio),
                          orderBy: source => source.OrderBy(x => x.Id))).ToList();
                }
                else
                {
                    Votos_Candidatos = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(x => x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete_Tipo_Eleccion.Paquete.Casilla.Seccion_Id == SeccionId,
                        include: source => source.Include(x => x.Candidato).Include(x => x.Paquete_Tipo_Eleccion).ThenInclude(x => x.Paquete).ThenInclude(x => x.Casilla).ThenInclude(x => x.Municipio),
                        orderBy: source => source.OrderBy(x => x.Candidato.Orden).ThenBy(x => x.Candidato.Tipo_Eleccion_Id))).ToList();

                    Tipo_Eleccion_Paquetes = (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete.Casilla.Seccion_Id == SeccionId,
                        include: source => source.Include(x => x.Paquete.Casilla.Seccion.Municipio),
                        orderBy: source => source.OrderBy(x => x.Id))).ToList();
                }

                var Candidatos_Unicos = Votos_Candidatos.GroupBy(x => new { x.Candidato_Id, x.Candidato.Nombres, x.Candidato.Apellido_Paterno, x.Candidato.Apellido_Materno, x.Candidato.No_Formula }).Select(g => new { Candidato_Id = g.Key, Candidato = $"{g.Key.Nombres} {g.Key.Apellido_Paterno} {g.Key.Apellido_Materno}", No_Formula = g.Key.No_Formula, Votos = g.Sum(x => x.Votos) }).ToList();


                var Resultados = new
                {
                    Seccion = new
                    {
                        Seccion_Id = Seccion.Id,
                        Seccion = Seccion.Nombre,
                        Tipo_Eleccion_Id = Tipo_Eleccion.Id,
                        Tipo_Eleccion = Tipo_Eleccion.Nombre,
                        boletas_Sobrantes = Tipo_Eleccion_Paquetes.Sum(x => x.Boletas_Sobrantes),
                        boletas_Capturadas = Tipo_Eleccion_Paquetes.Sum(x => x.Boletas_Capturadas),
                        votos_Nulos = Tipo_Eleccion_Paquetes.Sum(x => x.Votos_Nulos),
                        campos_No_Utilizados = Tipo_Eleccion_Paquetes.Sum(x => x.Campos_No_Utilizados),
                        total_Votos = Tipo_Eleccion_Paquetes.Sum(x => x.Total_Votos),
                        computadas = Paquetes.Count(),
                    },
                    Candidatos = Candidatos_Unicos,
                };
                return Ok(new { success = true, data = Resultados });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = "Error en el servidor" });
            }
        }

        [HttpGet("ResultadoEstatal/{TipoEleccionId:int}")]
        [OutputCache(PolicyName = "consultas")]
        public async Task<IActionResult> ResultadoEstatal(int TipoEleccionId)
        {
            try
            {
                var Acta = await _ctx.Acta_Estatal.GetFirstOrdefaultAsync(x => x.Tipo_Eleccion_Id == TipoEleccionId);
                var Oficina = await _ctx.Oficina.GetFirstOrdefaultAsync(x => x.Nombre == "Central IEEN", include: source => source.Include(x => x.Municipio));
                var Tipo_Eleccion = await _ctx.Tipo_Eleccion.GetFirstOrdefaultAsync(x => x.Id == TipoEleccionId);
                var Paquetes = await _ctx.Paquete.GetAllasync(x => x.Computada == true,
                    include: source => source.Include(x => x.Casilla.Seccion.Municipio));
                var Votos_Candidatos = new List<Candidatos_Tipo_Eleccion>();
                var Tipo_Eleccion_Paquetes = new List<Paquete_Tipo_Eleccion>();

                var List_Elecciones = new List<int>() { 3, 4, 5, 6 };

                if (List_Elecciones.Contains(TipoEleccionId))
                {
                    Votos_Candidatos = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id),
                       include: source => source.Include(x => x.Candidato).Include(x => x.Paquete_Tipo_Eleccion).ThenInclude(x => x.Paquete).ThenInclude(x => x.Casilla).ThenInclude(x => x.Municipio),
                       orderBy: source => source.OrderBy(x => x.Candidato.Orden).ThenBy(x => x.Candidato.Tipo_Eleccion_Id))).ToList();

                    Tipo_Eleccion_Paquetes = (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id),
                          include: source => source.Include(x => x.Paquete.Casilla.Seccion.Municipio),
                          orderBy: source => source.OrderBy(x => x.Id))).ToList();
                }
                else
                {
                    Votos_Candidatos = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(x => x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id == TipoEleccionId,
                        include: source => source.Include(x => x.Candidato).Include(x => x.Paquete_Tipo_Eleccion).ThenInclude(x => x.Paquete).ThenInclude(x => x.Casilla).ThenInclude(x => x.Municipio),
                        orderBy: source => source.OrderBy(x => x.Candidato.Orden).ThenBy(x => x.Candidato.Tipo_Eleccion_Id))).ToList();

                    Tipo_Eleccion_Paquetes = (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId,
                        include: source => source.Include(x => x.Paquete.Casilla.Seccion.Municipio),
                        orderBy: source => source.OrderBy(x => x.Id))).ToList();
                }

                var Candidatos_Unicos = Votos_Candidatos.GroupBy(x => new { x.Candidato_Id, x.Candidato.Nombres, x.Candidato.Apellido_Paterno, x.Candidato.Apellido_Materno, x.Candidato.No_Formula }).Select(g => new { Candidato_Id = g.Key, Candidato = $"{g.Key.Nombres} {g.Key.Apellido_Paterno} {g.Key.Apellido_Materno}", No_Formula = g.Key.No_Formula, Votos = g.Sum(x => x.Votos) }).ToList();

                var Resultados = new
                {
                    Estatal = new
                    {
                        Oficina_Id = Oficina.Id,
                        No_Oficina = Oficina.No_Oficina,
                        Oficina = Oficina.Nombre,
                        Tipo_Eleccion_Id = Tipo_Eleccion.Id,
                        Tipo_Eleccion = Tipo_Eleccion.Nombre,
                        boletas_Sobrantes = Tipo_Eleccion_Paquetes.Sum(x => x.Boletas_Sobrantes),
                        boletas_Capturadas = Tipo_Eleccion_Paquetes.Sum(x => x.Boletas_Capturadas),
                        votos_Nulos = Tipo_Eleccion_Paquetes.Sum(x => x.Votos_Nulos),
                        campos_No_Utilizados = Tipo_Eleccion_Paquetes.Sum(x => x.Campos_No_Utilizados),
                        total_Votos = Tipo_Eleccion_Paquetes.Sum(x => x.Total_Votos),
                        computadas = Paquetes.Count(),
                        acta_Url = Acta != null ? Acta.Acta_Url : "",
                    },
                    ruta_Acta = Acta != null ? Acta.Acta_Url_Local : "",
                    Candidatos = Candidatos_Unicos,
                };
                return Ok(new { success = true, data = Resultados });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = "Error en el servidor" });
            }
        }

        [HttpGet("ExcelByTipoEleccion/{TipoEleccionId:int}")]
        public async Task<IActionResult> ExcelByTipoEleccion(int TipoEleccionId)
        {
            try
            {
                var Resp = await GeneraExcelByTipoEleccion(TipoEleccionId);
                return File(Resp, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileDownloadName: "Candidatos.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = Commons.MensajeError });
            }
        }

        private async Task<byte[]> GeneraExcelByTipoEleccion(int Tipo_Eleccion_Id)
        {
            try
            {
                var Candidatos_Bd = new List<Candidato>();
                var Tipos_Eleccion = new List<int> { 3, 4, 5, 6 };
                var Paquetes_Tipo_Eleccion = new List<Paquete_Tipo_Eleccion>();
                var Paquetes_Tipo_Eleccion_Id = new List<int>();
                var Votos_Candidatos = new List<Candidatos_Tipo_Eleccion>();

                if (Tipo_Eleccion_Id == 3)
                {
                    Candidatos_Bd = (await _ctx.Candidato.GetAllasync(
                        x => Tipos_Eleccion.Contains(x.Tipo_Eleccion_Id) && x.Activo == true,
                        include: source => source.Include(x => x.Tipo_Eleccion), 
                        orderBy: source => source.OrderBy(x => x.Tipo_Eleccion_Id).ThenBy(x => x.Tipo_Eleccion.Orden))).ToList();

                    Paquetes_Tipo_Eleccion = (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => Tipos_Eleccion.Contains(x.Tipo_Eleccion_Id), 
                        include: source => source.Include(x => x.Paquete.Casilla.Seccion.Municipio)
                        .Include(x => x.Paquete.Casilla.Tipos_Casilla)
                        .Include(x => x.Tipo_Eleccion), 
                        orderBy: source => source.OrderBy(x => x.Paquete.Casilla.Seccion_Id).ThenBy(x => x.Paquete.Casilla.Municipio_Id))).ToList();

                    Paquetes_Tipo_Eleccion_Id = Paquetes_Tipo_Eleccion.Select(x => x.Id).ToList();

                    Votos_Candidatos = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(
                      x => Tipos_Eleccion.Contains(x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id) && Paquetes_Tipo_Eleccion_Id.Contains(x.Paquete_Tipo_Eleccion_Id),
                      include: source => source.Include(x => x.Candidato),
                      orderBy: source => source.OrderBy(x => x.Candidato.Orden))).ToList();
                } else
                {
                    Candidatos_Bd = (await _ctx.Candidato.GetAllasync(
                        x => x.Tipo_Eleccion_Id == Tipo_Eleccion_Id && x.Activo == true, 
                        include: source => source.Include(x => x.Tipo_Eleccion))).ToList();

                    Paquetes_Tipo_Eleccion = (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => x.Tipo_Eleccion_Id == Tipo_Eleccion_Id, 
                        include: source => source.Include(x => x.Paquete.Casilla.Seccion.Municipio)
                        .Include(X => X.Paquete.Casilla.Tipos_Casilla)
                        .Include(x => x.Tipo_Eleccion),
                        orderBy: source => source.OrderBy(x => x.Paquete.Casilla.Seccion_Id).ThenBy(x => x.Paquete.Casilla.Municipio_Id))).ToList();

                    Paquetes_Tipo_Eleccion_Id = Paquetes_Tipo_Eleccion.Select(x => x.Id).ToList();

                    Votos_Candidatos = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(
                      x => x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id == Tipo_Eleccion_Id && Paquetes_Tipo_Eleccion_Id.Contains(x.Paquete_Tipo_Eleccion_Id),
                      include: source => source.Include(x => x.Candidato),
                      orderBy: source => source.OrderBy(x => x.Candidato.Orden))).ToList();
                }

                var Tabla_Final = new List<Lista_Excel>();
                foreach(var Paquete in Paquetes_Tipo_Eleccion.DistinctBy(x => x.Paquete_Id))
                {
                    
                    var Votos_Candidatos_Paquete =
                        Votos_Candidatos.Where(x => x.Paquete_Tipo_Eleccion.Paquete_Id == Paquete.Paquete_Id).ToList();

                    var Registro = new Lista_Excel();
                    Registro.ESTADO = 18;
                    Registro.NOMBRE_ESTADO = "NAYARIT";
                    Registro.ID_MUNICIPIO = Paquete.Paquete.Casilla.Municipio.Clave;
                    Registro.MUNICIPIO = Paquete.Paquete.Casilla.Municipio.Nombre;
                    Registro.SECCION = Paquete.Paquete.Casilla.Seccion.Nombre;
                    Registro.ID_CASILLA = Paquete.Paquete.Casilla.Tipo_Casilla_Id;
                    Registro.TIPO_CASILLA = Paquete.Paquete.Casilla.Tipos_Casilla.Nombre;
                    Registro.EXT_CONTIGUA = 0;
                    Registro.TIPO_ACTA = Paquete.Tipo_Eleccion.Siglas;

                    for(int i = 0; i < Candidatos_Bd.Count; i++)
                    {
                        var Candidato = Candidatos_Bd[i];
                        var Votos_Validados = Votos_Candidatos_Paquete
                            .FirstOrDefault(x => x.Candidato_Id == Candidato.Id);
                        var Votos = Votos_Validados == null ? 0 : Votos_Validados.Votos;

                        string key = $"CAND_{(i + 1):D2}";
                        Registro.CANDIDATOS[key] = Votos;
                    }
                    if(Tipo_Eleccion_Id == 3)
                    {
                        Registro.NULOS = Paquetes_Tipo_Eleccion.Where(x => x.Paquete_Id == Paquete.Paquete_Id).Sum(x => x.Votos_Nulos);
                    } else
                    {
                        Registro.NULOS = Paquete.Votos_Nulos;
                    }
                    Registro.TOTAL_VOTOS = Paquete.Total_Votos;
                    Registro.LISTA_NOMINAL = Paquete.Paquete.Casilla.Listado_Nominal;
                    Registro.ESTATUS_ACTA = Paquete.Paquete.Computada == true ? "COMPUTADA" : "NO COMPUTADA";
                    Tabla_Final.Add(Registro);
                }

                var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Resultados");

                var headers = new List<string>
                {
                    "ESTADO", "NOMBRE_ESTADO", "ID_MUNICIPIO", "MUNICIPIO",
                    "SECCION", "ID_CASILLA", "TIPO_CASILLA", "EXT_CONTIGUA", "TIPO_ACTA"
                };

                // Agregar encabezados dinámicos de candidatos
                for (int i = 0; i < Candidatos_Bd.Count; i++)
                {
                    string key = $"CAND_{(i + 1):D2}";
                    headers.Add(key);
                }

                // Agregar los encabezados finales si lo necesitas
                headers.Add("NULOS");
                headers.Add("TOTAL_VOTOS");
                headers.Add("LISTA_NOMINAL");
                headers.Add("ESTATUS_ACTA");

                // Escribe los encabezados en la hoja
                for (int i = 0; i < headers.Count; i++)
                    ws.Cell(1, i + 1).Value = headers[i];

                // Escribe datos
                for (int r = 0; r < Tabla_Final.Count; r++)
                {
                    var reg = Tabla_Final[r];
                    int c = 1;

                    ws.Cell(r + 2, c++).Value = reg.ESTADO;
                    ws.Cell(r + 2, c++).Value = reg.NOMBRE_ESTADO;
                    ws.Cell(r + 2, c++).Value = reg.ID_MUNICIPIO;
                    ws.Cell(r + 2, c++).Value = reg.MUNICIPIO;
                    ws.Cell(r + 2, c++).Value = reg.SECCION;
                    ws.Cell(r + 2, c++).Value = reg.ID_CASILLA;
                    ws.Cell(r + 2, c++).Value = reg.TIPO_CASILLA;
                    ws.Cell(r + 2, c++).Value = reg.EXT_CONTIGUA;
                    ws.Cell(r + 2, c++).Value = reg.TIPO_ACTA;

                    foreach (var clave in reg.CANDIDATOS.Keys.OrderBy(k => k))
                        ws.Cell(r + 2, c++).Value = reg.CANDIDATOS[clave];

                    ws.Cell(r + 2, c++).Value = reg.NULOS;
                    ws.Cell(r + 2, c++).Value = reg.TOTAL_VOTOS;
                    ws.Cell(r + 2, c++).Value = reg.LISTA_NOMINAL;
                    ws.Cell(r + 2, c++).Value = reg.ESTATUS_ACTA;
                }

                ws.Columns().AdjustToContents();
                wb.SaveAs("Resultados.xlsx");
                using var ms = new MemoryStream();
                wb.SaveAs(ms);
                return ms.ToArray();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return null;
            }
        }

    }
}
