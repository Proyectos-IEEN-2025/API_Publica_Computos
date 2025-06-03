using API_Computos_Publica.Data.Repository.Interfacez;
using API_Computos_Publica.Models.DTO.Commons;
using API_Computos_Publica.Models.DTO.Computos;
using API_Computos_Publica.Models.DTO.Configuracion;
using API_Computos_Publica.Models.Entities.Computos;
using API_Computos_Publica.Models.Entities.Configuracion;
using API_Computos_Publica.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using OfficeOpenXml;
using System.Security.Claims;
using System.Security.Policy;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API_Computos_Publica.Controllers.Confguracion
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatosController : ControllerBase
    {
        private readonly IUnitOfWork _ctx;
        private readonly ILogger<CandidatosController> _logger;
        private readonly IWebHostEnvironment _host;
        private readonly IMapper _mapper;

        public CandidatosController(IUnitOfWork ctx, ILogger<CandidatosController> logger, IMapper mapper, IWebHostEnvironment host)
        {
            _ctx = ctx;
            _logger = logger;
            _mapper = mapper;
            _host = host;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var Candidatos = await _ctx.Candidato.GetAllasync(
                    include: source => source.Include(x => x.Tipo_Eleccion));
                var Candidatos_DTO = _mapper.Map<IEnumerable<Candidato_DTO>>(Candidatos);
                return Ok(new { success = true, data = Candidatos_DTO });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = Commons.MensajeError });
            }
        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> Get(int Id)
        {
            try
            {
                var Candidato = await _ctx.Candidato.GetFirstOrdefaultAsync(x => x.Id == Id,
                    include: source => source.Include(x => x.Tipo_Eleccion));
                var Candidato_DTO = _mapper.Map<Candidato_DTO>(Candidato);
                return Ok(new { success = true, data = Candidato_DTO });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = Commons.MensajeError });
            }
        }

        [HttpGet("ByJuntaAuxiliar/{TipoEleccionId:int}/{OficinaId:int}")]
        [OutputCache(PolicyName = "consultas")]
        public async Task<IActionResult> ByJuntaAuxiliar(int TipoEleccionId, int OficinaId)
        {
            try
            {
                var Oficina = await _ctx.Oficina.GetFirstOrdefaultAsync(x => x.Id == OficinaId, include: source => source.Include(x => x.Municipio));
                var Tipo_Eleccion_Paquetes = new List<Paquete_Tipo_Eleccion>();
                var Votos_Candidatos = new List<Candidatos_Tipo_Eleccion>();

                var Paquetes_Adicional = new List<Paquete_Tipo_Eleccion_Adicional>();
                var Votos_Candidatos_Adicional = new List<Candidatos_Tipo_Eleccion_Adicional>();

                var Paquete = new List<Paquete_Tipo_Eleccion>();
                var Paquete_adicional = new List<Paquete_Tipo_Eleccion_Adicional>();

                var Candidatos = new List<Candidato>();

                var List_Elecciones = new List<int>() { 3, 4, 5, 6 };

                if (List_Elecciones.Contains(TipoEleccionId))
                {
                    Candidatos = (await _ctx.Candidato.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id) && x.Activo == true, include: source => source.Include(x => x.Tipo_Eleccion))).ToList();
                    Votos_Candidatos = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id) && x.Paquete_Tipo_Eleccion.Paquete.Casilla.Municipio.Region == Oficina.Municipio.Region && x.Paquete_Tipo_Eleccion.Publicado == true, include: source => source.Include(x => x.Paquete_Tipo_Eleccion))).ToList();

                    Votos_Candidatos_Adicional = (await _ctx.Candidato_Tipo_Eleccion_Adicional.GetAllasync(x => List_Elecciones.Contains(x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id) && x.Paquete_Tipo_Eleccion.Paquete.Casilla.Municipio.Region == Oficina.Municipio.Region && x.Paquete_Tipo_Eleccion.Publicado == true, include: source => source.Include(x => x.Paquete_Tipo_Eleccion))).ToList();

                    Paquete = (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id) && x.Paquete.Casilla.Municipio.Region == Oficina.Municipio.Region)).ToList();
                    Paquete_adicional = (await _ctx.Paquete_Tipo_Eleccion_Adicional.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id) && x.Paquete.Casilla.Municipio.Region == Oficina.Municipio.Region)).ToList();
                }
                else
                {
                    Candidatos = (await _ctx.Candidato.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Activo == true, include: source => source.Include(x => x.Tipo_Eleccion))).ToList();
                    Votos_Candidatos = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(x => x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete_Tipo_Eleccion.Paquete.Casilla.Municipio.Region == Oficina.Municipio.Region && x.Paquete_Tipo_Eleccion.Publicado == true, include: source => source.Include(x => x.Paquete_Tipo_Eleccion))).ToList();

                    Votos_Candidatos_Adicional = (await _ctx.Candidato_Tipo_Eleccion_Adicional.GetAllasync(x => x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete_Tipo_Eleccion.Paquete.Casilla.Municipio.Region == Oficina.Municipio.Region && x.Paquete_Tipo_Eleccion.Publicado == true, include: source => source.Include(x => x.Paquete_Tipo_Eleccion))).ToList();

                    Paquete = (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete.Casilla.Municipio.Region == Oficina.Municipio.Region)).ToList();
                    Paquete_adicional = (await _ctx.Paquete_Tipo_Eleccion_Adicional.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete.Casilla.Municipio.Region == Oficina.Municipio.Region)).ToList();
                }


                var Candidatos_DTO = _mapper.Map<IEnumerable<Candidato_DTO>>(Candidatos);

                var Candidatos_Unicos = Votos_Candidatos.GroupBy(x => x.Candidato_Id).Select(g => new { Candidato_Id = g.Key, Votos = g.Sum(x => x.Votos) }).ToList();
                var Votos_Candidatos_Totales = Votos_Candidatos.Sum(x => x.Votos);

                var Candidatos_Unicos_Adicional = Votos_Candidatos_Adicional.GroupBy(x => x.Candidato_Id).Select(g => new { Candidato_Id = g.Key, Votos = g.Sum(x => x.Votos) }).ToList();
                var Votos_Candidatos_Totales_Adicional = Votos_Candidatos_Adicional.Sum(x => x.Votos);

                var Votos_Nulos = List_Elecciones.Contains(TipoEleccionId) ? (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id) && x.Paquete.Casilla.Municipio.Region == Oficina.Municipio.Region)).Sum(x => x.Votos_Nulos) : (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete.Casilla.Municipio.Region == Oficina.Municipio.Region)).Sum(x => x.Votos_Nulos);
                var Votos_Nulos_Adicional = List_Elecciones.Contains(TipoEleccionId) ? (await _ctx.Paquete_Tipo_Eleccion_Adicional.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id) && x.Paquete.Casilla.Municipio.Region == Oficina.Municipio.Region)).Sum(x => x.Votos_Nulos) : (await _ctx.Paquete_Tipo_Eleccion_Adicional.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete.Casilla.Municipio.Region == Oficina.Municipio.Region)).Sum(x => x.Votos_Nulos);

                Candidatos_DTO = Candidatos_DTO
                .Select(x =>
                {
                    var candidato = Candidatos_Unicos.FirstOrDefault(c => c.Candidato_Id == x.Id);
                    var candidatoAdicional = Candidatos_Unicos_Adicional.FirstOrDefault(c => c.Candidato_Id == x.Id);
                    var total_Votos_candidato = candidato?.Votos ?? 0 + candidatoAdicional?.Votos ?? 0;
                    x.Porcentaje = total_Votos_candidato != null && total_Votos_candidato > 0 ? (decimal)total_Votos_candidato / Paquete.Sum(x => x.Total_Votos) + Paquete_adicional.Sum(x => x.Total_Votos) : 0;
                    x.Votos = total_Votos_candidato;
                    x.Fotografia_URL = x.Fotografia_URL != null ? x.Fotografia_URL.Replace("http", "https").Replace("Candidatos//", "Candidatos/") : null;
                    return x;
                })
                .ToList();

                return Ok(new { success = true, data = Candidatos_DTO });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = Commons.MensajeError });
            }
        }

        [HttpGet("ByMunicipio/{TipoEleccionId:int}/{MunicipioId:int}")]
        [OutputCache(PolicyName = "consultas")]
        public async Task<IActionResult> ByMunicipio(int TipoEleccionId, int MunicipioId)
        {
            try
            {
                var Tipo_Eleccion_Paquetes = new List<Paquete_Tipo_Eleccion>();
                var Votos_Candidatos = new List<Candidatos_Tipo_Eleccion>();

                var Paquetes_Adicional = new List<Paquete_Tipo_Eleccion_Adicional>();
                var Votos_Candidatos_Adicional = new List<Candidatos_Tipo_Eleccion_Adicional>();

                var Paquete = new List<Paquete_Tipo_Eleccion>();
                var Paquete_adicional = new List<Paquete_Tipo_Eleccion_Adicional>();

                var Candidatos = new List<Candidato>();

                var List_Elecciones = new List<int>() { 3, 4, 5, 6 };

                if (List_Elecciones.Contains(TipoEleccionId))
                {
                    Candidatos = (await _ctx.Candidato.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id) && x.Activo == true, include: source => source.Include(x => x.Tipo_Eleccion))).ToList();
                    Votos_Candidatos = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id) && x.Paquete_Tipo_Eleccion.Paquete.Casilla.Municipio_Id == MunicipioId && x.Paquete_Tipo_Eleccion.Publicado == true, include: source => source.Include(x => x.Paquete_Tipo_Eleccion))).ToList();

                    Votos_Candidatos_Adicional = (await _ctx.Candidato_Tipo_Eleccion_Adicional.GetAllasync(x => List_Elecciones.Contains(x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id) && x.Paquete_Tipo_Eleccion.Paquete.Casilla.Municipio_Id == MunicipioId && x.Paquete_Tipo_Eleccion.Publicado == true, include: source => source.Include(x => x.Paquete_Tipo_Eleccion))).ToList();

                    Paquete = (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id) && x.Paquete.Casilla.Municipio_Id == MunicipioId)).ToList();
                    Paquete_adicional = (await _ctx.Paquete_Tipo_Eleccion_Adicional.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id) && x.Paquete.Casilla.Municipio_Id == MunicipioId)).ToList();
                }
                else
                {
                    Candidatos = (await _ctx.Candidato.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Activo == true, include: source => source.Include(x => x.Tipo_Eleccion))).ToList();
                    Votos_Candidatos = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(x => x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete_Tipo_Eleccion.Paquete.Casilla.Municipio_Id == MunicipioId && x.Paquete_Tipo_Eleccion.Publicado == true, include: source => source.Include(x => x.Paquete_Tipo_Eleccion))).ToList();

                    Votos_Candidatos_Adicional = (await _ctx.Candidato_Tipo_Eleccion_Adicional.GetAllasync(x => x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete_Tipo_Eleccion.Paquete.Casilla.Municipio_Id == MunicipioId && x.Paquete_Tipo_Eleccion.Publicado == true, include: source => source.Include(x => x.Paquete_Tipo_Eleccion))).ToList();

                    Paquete = (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete.Casilla.Municipio_Id == MunicipioId)).ToList();
                    Paquete_adicional = (await _ctx.Paquete_Tipo_Eleccion_Adicional.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete.Casilla.Municipio_Id == MunicipioId)).ToList();

                }


                var Candidatos_DTO = _mapper.Map<IEnumerable<Candidato_DTO>>(Candidatos);

                var Candidatos_Unicos = Votos_Candidatos.GroupBy(x => x.Candidato_Id).Select(g => new { Candidato_Id = g.Key, Votos = g.Sum(x => x.Votos) }).ToList();
                var Votos_Candidatos_Totales = Votos_Candidatos.Sum(x => x.Votos);

                var Candidatos_Unicos_Adicional = Votos_Candidatos_Adicional.GroupBy(x => x.Candidato_Id).Select(g => new { Candidato_Id = g.Key, Votos = g.Sum(x => x.Votos) }).ToList();
                var Votos_Candidatos_Totales_Adicional = Votos_Candidatos_Adicional.Sum(x => x.Votos);

                var Votos_Nulos = List_Elecciones.Contains(TipoEleccionId) ? (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id) && x.Paquete.Casilla.Municipio_Id == MunicipioId)).Sum(x => x.Votos_Nulos) : (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete.Casilla.Municipio_Id == MunicipioId)).Sum(x => x.Votos_Nulos);
                var Votos_Nulos_Adicional = List_Elecciones.Contains(TipoEleccionId) ? (await _ctx.Paquete_Tipo_Eleccion_Adicional.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id) && x.Paquete.Casilla.Municipio_Id == MunicipioId)).Sum(x => x.Votos_Nulos) : (await _ctx.Paquete_Tipo_Eleccion_Adicional.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete.Casilla.Municipio_Id == MunicipioId)).Sum(x => x.Votos_Nulos);

                Candidatos_DTO = Candidatos_DTO
                    .Select(x =>
                    {
                        var candidato = Candidatos_Unicos.FirstOrDefault(c => c.Candidato_Id == x.Id);
                        var candidatoAdicional = Candidatos_Unicos_Adicional.FirstOrDefault(c => c.Candidato_Id == x.Id);
                        var total_Votos_candidato = candidato?.Votos ?? 0 + candidatoAdicional?.Votos ?? 0;
                        x.Porcentaje = total_Votos_candidato != null && total_Votos_candidato > 0 ? (decimal)total_Votos_candidato / Paquete.Sum(x => x.Total_Votos) + Paquete_adicional.Sum(x => x.Total_Votos) : 0;
                        x.Votos = total_Votos_candidato;
                        x.Fotografia_URL = x.Fotografia_URL != null ? x.Fotografia_URL.Replace("http", "https").Replace("Candidatos//", "Candidatos/") : null;
                        return x;
                    })
                    .ToList();
                return Ok(new { success = true, data = Candidatos_DTO });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = Commons.MensajeError });
            }
        }

        [HttpGet("ByTipoEleccion/{TipoEleccionId:int}")]
        [OutputCache(PolicyName = "consultas")]
        public async Task<IActionResult> ByTipoEleccion(int TipoEleccionId)
        {
            try
            {
                var Tipo_Eleccion_Paquetes = new List<Paquete_Tipo_Eleccion>();
                var Votos_Candidatos = new List<Candidatos_Tipo_Eleccion>();

                var Paquetes_Adicional = new List<Paquete_Tipo_Eleccion_Adicional>();
                var Votos_Candidatos_Adicional = new List<Candidatos_Tipo_Eleccion_Adicional>();

                var Paquete = new List<Paquete_Tipo_Eleccion>();
                var Paquete_adicional = new List<Paquete_Tipo_Eleccion_Adicional>();

                var Candidatos = new List<Candidato>();

                var List_Elecciones = new List<int>() { 3, 4, 5, 6 };

                if (List_Elecciones.Contains(TipoEleccionId))
                {
                    Candidatos = (await _ctx.Candidato.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id) && x.Activo == true, include: source => source.Include(x => x.Tipo_Eleccion))).ToList();
                    Votos_Candidatos = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id), include: source => source.Include(x => x.Paquete_Tipo_Eleccion))).ToList();

                    Votos_Candidatos_Adicional = (await _ctx.Candidato_Tipo_Eleccion_Adicional.GetAllasync(x => List_Elecciones.Contains(x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id), include: source => source.Include(x => x.Paquete_Tipo_Eleccion))).ToList();

                    Paquete = (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id))).ToList();
                    Paquete_adicional = (await _ctx.Paquete_Tipo_Eleccion_Adicional.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id))).ToList();

                }
                else
                {
                    Candidatos = (await _ctx.Candidato.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Activo == true, include: source => source.Include(x => x.Tipo_Eleccion))).ToList();
                    Votos_Candidatos = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(x => x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id == TipoEleccionId, include: source => source.Include(x => x.Paquete_Tipo_Eleccion))).ToList();

                    Votos_Candidatos_Adicional = (await _ctx.Candidato_Tipo_Eleccion_Adicional.GetAllasync(x => x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id == TipoEleccionId, include: source => source.Include(x => x.Paquete_Tipo_Eleccion))).ToList();
                    Paquete = (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId)).ToList();
                    Paquete_adicional = (await _ctx.Paquete_Tipo_Eleccion_Adicional.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId)).ToList();
                }


                var Candidatos_DTO = _mapper.Map<IEnumerable<Candidato_DTO>>(Candidatos);

                var Candidatos_Unicos = Votos_Candidatos.GroupBy(x => x.Candidato_Id).Select(g => new { Candidato_Id = g.Key, Votos = g.Sum(x => x.Votos) }).ToList();
                var Votos_Candidatos_Totales = Votos_Candidatos.Sum(x => x.Votos);

                var Candidatos_Unicos_Adicional = Votos_Candidatos_Adicional.GroupBy(x => x.Candidato_Id).Select(g => new { Candidato_Id = g.Key, Votos = g.Sum(x => x.Votos) }).ToList();
                var Votos_Candidatos_Totales_Adicional = Votos_Candidatos_Adicional.Sum(x => x.Votos);

                var Votos_Nulos = List_Elecciones.Contains(TipoEleccionId) ? (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id))).Sum(x => x.Votos_Nulos) : (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId)).Sum(x => x.Votos_Nulos);
                var Votos_Nulos_Adicional = List_Elecciones.Contains(TipoEleccionId) ? (await _ctx.Paquete_Tipo_Eleccion_Adicional.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id))).Sum(x => x.Votos_Nulos) : (await _ctx.Paquete_Tipo_Eleccion_Adicional.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId)).Sum(x => x.Votos_Nulos);

                Candidatos_DTO = Candidatos_DTO
                    .Select(x =>
                    {
                        var candidato = Candidatos_Unicos.FirstOrDefault(c => c.Candidato_Id == x.Id);
                        var candidatoAdicional = Candidatos_Unicos_Adicional.FirstOrDefault(c => c.Candidato_Id == x.Id);
                        var total_Votos_candidato = candidato?.Votos ?? 0 + candidatoAdicional?.Votos ?? 0;
                        x.Porcentaje = total_Votos_candidato != null && total_Votos_candidato > 0 ? (decimal) total_Votos_candidato / Paquete.Sum(x => x.Total_Votos) + Paquete_adicional.Sum(x => x.Total_Votos) : 0;
                        x.Votos = total_Votos_candidato;
                        x.Fotografia_URL = x.Fotografia_URL != null ? x.Fotografia_URL.Replace("http", "https").Replace("Candidatos//", "Candidatos/") : null;
                        return x;
                    })
                    .ToList();

                return Ok(new { success = true, data = Candidatos_DTO });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = Commons.MensajeError });
            }
        }

        [HttpGet("BySeccion/{SeccionId:int}/{TipoEleccionId:int}")]
        [OutputCache(PolicyName = "consultas")]
        public async Task<IActionResult> BySeccion(int SeccionId, int TipoEleccionId)
        {
            try
            {
                var Casillas = await _ctx.Casilla.GetAllasync(x => x.Seccion_Id == SeccionId, include: source => source.Include(x => x.Seccion));
                var Tipo_Eleccion = await _ctx.Tipo_Eleccion.GetFirstOrdefaultAsync(x => x.Id == TipoEleccionId);
                var Lista = new List<Votos_Secciones_DTO>();

                foreach (var Casilla in Casillas)
                {
                    var Tipo_Eleccion_Paquetes = new List<Paquete_Tipo_Eleccion>();
                    var Votos_Candidatos = new List<Candidatos_Tipo_Eleccion>();

                    var Tipo_Eleccion_Paquetes_Adicional = new List<Paquete_Tipo_Eleccion_Adicional>();
                    var Votos_Candidatos_Adicional = new List<Candidatos_Tipo_Eleccion_Adicional>();

                    var Candidatos = new List<Candidato>();

                    var List_Elecciones = new List<int>() { 3, 4, 5, 6 };

                    if (List_Elecciones.Contains(TipoEleccionId))
                    {
                        Candidatos = (await _ctx.Candidato.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id) && x.Activo == true, include: source => source.Include(x => x.Tipo_Eleccion))).ToList();
                        Votos_Candidatos = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id) && x.Paquete_Tipo_Eleccion.Paquete.Casilla_Id == Casilla.Id && x.Paquete_Tipo_Eleccion.Publicado == true, include: source => source.Include(x => x.Paquete_Tipo_Eleccion))).ToList();

                        Votos_Candidatos_Adicional = (await _ctx.Candidato_Tipo_Eleccion_Adicional.GetAllasync(x => List_Elecciones.Contains(x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id) && x.Paquete_Tipo_Eleccion.Paquete.Casilla_Id == Casilla.Id && x.Paquete_Tipo_Eleccion.Publicado == true, include: source => source.Include(x => x.Paquete_Tipo_Eleccion))).ToList();
                    }
                    else
                    {
                        Candidatos = (await _ctx.Candidato.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Activo == true, include: source => source.Include(x => x.Tipo_Eleccion))).ToList();
                        Votos_Candidatos = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(x => x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete_Tipo_Eleccion.Paquete.Casilla_Id == Casilla.Id && x.Paquete_Tipo_Eleccion.Publicado == true, include: source => source.Include(x => x.Paquete_Tipo_Eleccion))).ToList();

                        Votos_Candidatos_Adicional = (await _ctx.Candidato_Tipo_Eleccion_Adicional.GetAllasync(x => x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete_Tipo_Eleccion.Paquete.Casilla_Id == Casilla.Id && x.Paquete_Tipo_Eleccion.Publicado == true, include: source => source.Include(x => x.Paquete_Tipo_Eleccion))).ToList();

                    }

                    var Candidatos_DTO = _mapper.Map<IEnumerable<Candidato_DTO>>(Candidatos);

                    var Candidatos_Unicos = Votos_Candidatos.GroupBy(x => x.Candidato_Id).Select(g => new { Candidato_Id = g.Key, Votos = g.Sum(x => x.Votos) }).ToList();
                    var Votos_Candidatos_Totales = Votos_Candidatos.Sum(x => x.Votos);

                    var Votos_Nulos = List_Elecciones.Contains(TipoEleccionId) ? (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id) && x.Paquete.Casilla_Id == Casilla.Id)) : (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete.Casilla_Id == Casilla.Id));
                   
                    Candidatos_DTO = Candidatos_DTO
                        .Select(x =>
                        {
                            var candidato = Candidatos_Unicos.FirstOrDefault(c => c.Candidato_Id == x.Id);
                            var totalVotos = Votos_Candidatos_Totales + (Votos_Nulos != null ? Votos_Nulos.Sum(n => n.Votos_Nulos) : 0);
                            x.Votos = candidato != null ? (candidato.Votos) : 0;
                            x.Porcentaje = (x.Votos.HasValue && totalVotos != 0) ? (decimal)x.Votos.Value / totalVotos : 0;
                            x.Fotografia_URL = x.Fotografia_URL != null ? x.Fotografia_URL.Replace("http", "https").Replace("Candidatos//", "Candidatos/") : null;
                            return x;
                        })
                        .ToList();

                    var Voto = new Votos_Secciones_DTO
                    {
                        Seccion_Id = Casilla.Seccion_Id,
                        Seccion = Casilla.Seccion.Nombre,
                        Casilla_Id = Casilla.Id,
                        Casilla = $"{int.Parse(Casilla.Seccion.Nombre).ToString("D4")} {Casilla.Nombre}",
                        Tipo_Eleccion_Id = Tipo_Eleccion.Id,
                        Tipo_Eleccion = Tipo_Eleccion.Nombre,
                        Acta_URL = Votos_Nulos.Count() != 0 ? Votos_Nulos.FirstOrDefault(x => x.Tipo_Eleccion_Id == TipoEleccionId).Acta_Url_Local : null,
                        Votos_Candidaturas = Votos_Nulos.Count() != 0 ? Candidatos_DTO.Sum(x => x.Votos) : null,
                        Votos_Nulos = Votos_Nulos.Count() != 0 ? Votos_Nulos.Sum(x => x.Votos_Nulos) : null,
                        Total_Votos = Votos_Nulos.Count() != 0 ? (Candidatos_DTO.Sum(x => x.Votos) + (Votos_Nulos.Count() != 0 ? Votos_Nulos.Sum(x => x.Votos_Nulos) : 0)) : null,
                        Candidaturas = Candidatos_DTO
                    };
                    Lista.Add(Voto);

                    var Votos_Nulos_Adicional = List_Elecciones.Contains(TipoEleccionId) ? (await _ctx.Paquete_Tipo_Eleccion_Adicional.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id) && x.Paquete.Casilla_Id == Casilla.Id)) : (await _ctx.Paquete_Tipo_Eleccion_Adicional.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete.Casilla_Id == Casilla.Id));

                    var JAO = Votos_Nulos_Adicional.Where(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Jao == true).Count();
                    var Estatal = Votos_Nulos_Adicional.Where(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Jao == false).Count();

                    if (JAO > 0)
                    {
                        var Candidatos_Adicional_JAO_DTO = _mapper.Map<IEnumerable<Candidato_DTO>>(Candidatos);

                        var Candidatos_Unicos_Adicional_JAO = Votos_Candidatos_Adicional.Where(x => x.Adicional_JAO == true).GroupBy(x => x.Candidato_Id).Select(g => new { Candidato_Id = g.Key, Votos = g.Sum(x => x.Votos) }).ToList();
                        var Votos_Candidatos_Totales_Adicional_JAO = Votos_Candidatos_Adicional.Where(x => x.Adicional_JAO == true).Sum(x => x.Votos);

                        Candidatos_Adicional_JAO_DTO = Candidatos_Adicional_JAO_DTO
                            .Select(x =>
                            {
                                var candidatoA = Candidatos_Unicos_Adicional_JAO.FirstOrDefault(c => c.Candidato_Id == x.Id);
                                var totalVotosA = Votos_Candidatos_Totales_Adicional_JAO + (Votos_Nulos_Adicional != null ? Votos_Nulos_Adicional.Where(x => x.Jao == true).Sum(n => n.Votos_Nulos) : 0);
                                x.Votos = candidatoA?.Votos;
                                x.Porcentaje = (x.Votos.HasValue && totalVotosA != 0) ? (decimal)x.Votos.Value / totalVotosA : 0;
                                x.Fotografia_URL = x.Fotografia_URL != null ? x.Fotografia_URL.Replace("http", "https").Replace("Candidatos//", "Candidatos/") : null;
                                return x;
                            })
                            .ToList();

                        var VotoA = new Votos_Secciones_DTO
                        {
                            Seccion_Id = Casilla.Seccion_Id,
                            Seccion = Casilla.Seccion.Nombre,
                            Casilla_Id = Casilla.Id,
                            Casilla = $"{int.Parse(Casilla.Seccion.Nombre).ToString("D4")} {Casilla.Nombre} (Adicional JAO)",
                            Tipo_Eleccion_Id = Tipo_Eleccion.Id,
                            Tipo_Eleccion = Tipo_Eleccion.Nombre,
                            Acta_URL = Votos_Nulos_Adicional.Count() != 0 ? Votos_Nulos_Adicional.FirstOrDefault(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Jao == true).Acta_Url_Local : null,
                            Votos_Candidaturas = Votos_Nulos_Adicional.Count() != 0 ? Candidatos_Adicional_JAO_DTO.Sum(x => x.Votos) : null,
                            Votos_Nulos = Votos_Nulos_Adicional.Count() != 0 ? Votos_Nulos_Adicional.Where(x => x.Jao == true).Sum(x => x.Votos_Nulos) : null,
                            Total_Votos = Votos_Nulos_Adicional.Count() != 0 ? (Candidatos_Adicional_JAO_DTO.Sum(x => x.Votos) + (Votos_Nulos_Adicional.Count() != 0 ? Votos_Nulos_Adicional.Where(x => x.Jao == true).Sum(x => x.Votos_Nulos) : 0)) : null,
                            Candidaturas = Candidatos_Adicional_JAO_DTO
                        };
                        Lista.Add(VotoA);
                    }

                    if (Estatal > 0)
                    {
                        var Candidatos_Adicional_DTO = _mapper.Map<IEnumerable<Candidato_DTO>>(Candidatos);

                        var Candidatos_Unicos_Adicional = Votos_Candidatos_Adicional.Where(x => x.Adicional == true).GroupBy(x => x.Candidato_Id).Select(g => new { Candidato_Id = g.Key, Votos = g.Sum(x => x.Votos) }).ToList();
                        var Votos_Candidatos_Totales_Adicional = Votos_Candidatos_Adicional.Where(x => x.Adicional == true).Sum(x => x.Votos);

                        Candidatos_Adicional_DTO = Candidatos_Adicional_DTO
                            .Select(x =>
                            {
                                var candidatoA = Candidatos_Unicos_Adicional.FirstOrDefault(c => c.Candidato_Id == x.Id);
                                var totalVotosA = Votos_Candidatos_Totales_Adicional + (Votos_Nulos_Adicional != null ? Votos_Nulos_Adicional.Where(x => x.Jao == false).Sum(n => n.Votos_Nulos) : 0);
                                x.Votos = candidatoA?.Votos;
                                x.Porcentaje = (x.Votos.HasValue && totalVotosA != 0) ? (decimal)x.Votos.Value / totalVotosA : 0;
                                x.Fotografia_URL = x.Fotografia_URL != null ? x.Fotografia_URL.Replace("http", "https").Replace("Candidatos//", "Candidatos/") : null;
                                return x;
                            })
                            .ToList();

                        var VotoA = new Votos_Secciones_DTO
                        {
                            Seccion_Id = Casilla.Seccion_Id,
                            Seccion = Casilla.Seccion.Nombre,
                            Casilla_Id = Casilla.Id,
                            Casilla = $"{int.Parse(Casilla.Seccion.Nombre).ToString("D4")} {Casilla.Nombre} (Adicional Estatal)",
                            Tipo_Eleccion_Id = Tipo_Eleccion.Id,
                            Tipo_Eleccion = Tipo_Eleccion.Nombre,
                            Acta_URL = Votos_Nulos_Adicional.Count() != 0 ? Votos_Nulos_Adicional.FirstOrDefault(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Jao == false).Acta_Url_Local : null,
                            Votos_Candidaturas = Votos_Nulos_Adicional.Count() != 0 ? Candidatos_Adicional_DTO.Sum(x => x.Votos) : null,
                            Votos_Nulos = Votos_Nulos_Adicional.Count() != 0 ? Votos_Nulos_Adicional.Where(x => x.Jao == false).Sum(x => x.Votos_Nulos) : null,
                            Total_Votos = Votos_Nulos_Adicional.Count() != 0 ? (Candidatos_Adicional_DTO.Sum(x => x.Votos) + (Votos_Nulos_Adicional.Count() != 0 ? Votos_Nulos_Adicional.Where(x => x.Jao == false).Sum(x => x.Votos_Nulos) : 0)) : null,
                            Candidaturas = Candidatos_Adicional_DTO
                        };
                        Lista.Add(VotoA);
                    }
                }

                return Ok(new { success = true, data = Lista });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = Commons.MensajeError });
            }
        }

        [HttpGet("BySeccionesMunicipio/{MunicipioId:int}/{TipoEleccionId:int}")]
        [OutputCache(PolicyName = "consultas")]
        public async Task<IActionResult> BySeccionesMunicipio(int MunicipioId, int TipoEleccionId)
        {
            try
            {
                var Casillas = await _ctx.Casilla.GetAllasync(x => x.Municipio_Id == MunicipioId, include: source => source.Include(x => x.Seccion));
                var Tipo_Eleccion = await _ctx.Tipo_Eleccion.GetFirstOrdefaultAsync(x => x.Id == TipoEleccionId);
                var Lista = new List<Votos_Secciones_DTO>();

                foreach (var Casilla in Casillas)
                {
                    var Tipo_Eleccion_Paquetes = new List<Paquete_Tipo_Eleccion>();
                    var Votos_Candidatos = new List<Candidatos_Tipo_Eleccion>();

                    var Tipo_Eleccion_Paquetes_Adicional = new List<Paquete_Tipo_Eleccion_Adicional>();
                    var Votos_Candidatos_Adicional = new List<Candidatos_Tipo_Eleccion_Adicional>();

                    var Candidatos = new List<Candidato>();

                    var List_Elecciones = new List<int>() { 3, 4, 5, 6 };

                    if (List_Elecciones.Contains(TipoEleccionId))
                    {
                        Candidatos = (await _ctx.Candidato.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id) && x.Activo == true, include: source => source.Include(x => x.Tipo_Eleccion))).ToList();
                        Votos_Candidatos = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id) && x.Paquete_Tipo_Eleccion.Paquete.Casilla_Id == Casilla.Id && x.Paquete_Tipo_Eleccion.Publicado == true, include: source => source.Include(x => x.Paquete_Tipo_Eleccion))).ToList();

                        Votos_Candidatos_Adicional = (await _ctx.Candidato_Tipo_Eleccion_Adicional.GetAllasync(x => List_Elecciones.Contains(x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id) && x.Paquete_Tipo_Eleccion.Paquete.Casilla_Id == Casilla.Id && x.Paquete_Tipo_Eleccion.Publicado == true, include: source => source.Include(x => x.Paquete_Tipo_Eleccion))).ToList();

                    }
                    else
                    {
                        Candidatos = (await _ctx.Candidato.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Activo == true, include: source => source.Include(x => x.Tipo_Eleccion))).ToList();
                        Votos_Candidatos = (await _ctx.Candidato_Tipo_Eleccion.GetAllasync(x => x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete_Tipo_Eleccion.Paquete.Casilla_Id == Casilla.Id && x.Paquete_Tipo_Eleccion.Publicado == true, include: source => source.Include(x => x.Paquete_Tipo_Eleccion))).ToList();
                        
                        Votos_Candidatos_Adicional = (await _ctx.Candidato_Tipo_Eleccion_Adicional.GetAllasync(x => x.Paquete_Tipo_Eleccion.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete_Tipo_Eleccion.Paquete.Casilla_Id == Casilla.Id && x.Paquete_Tipo_Eleccion.Publicado == true, include: source => source.Include(x => x.Paquete_Tipo_Eleccion))).ToList();
                    }

                    var Candidatos_DTO = _mapper.Map<IEnumerable<Candidato_DTO>>(Candidatos);

                    var Candidatos_Unicos = Votos_Candidatos.GroupBy(x => x.Candidato_Id).Select(g => new { Candidato_Id = g.Key, Votos = g.Sum(x => x.Votos) }).ToList();
                    var Votos_Candidatos_Totales = Votos_Candidatos.Sum(x => x.Votos);

                    var Votos_Nulos = List_Elecciones.Contains(TipoEleccionId) ? (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id) && x.Paquete.Casilla_Id == Casilla.Id)) : (await _ctx.Paquete_Tipo_Eleccion.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete.Casilla_Id == Casilla.Id));

                    Candidatos_DTO = Candidatos_DTO
                        .Select(x =>
                        {
                            var candidato = Candidatos_Unicos.FirstOrDefault(c => c.Candidato_Id == x.Id);
                            var totalVotos = Votos_Candidatos_Totales + (Votos_Nulos != null ? Votos_Nulos.Sum(n => n.Votos_Nulos) : 0);
                            x.Votos = candidato?.Votos;
                            x.Porcentaje = (x.Votos.HasValue && totalVotos != 0) ? (decimal)x.Votos.Value / totalVotos : 0;
                            x.Fotografia_URL = x.Fotografia_URL != null ? x.Fotografia_URL.Replace("http", "https").Replace("Candidatos//", "Candidatos/") : null;
                            return x;
                        })
                        .ToList();

                    var Voto = new Votos_Secciones_DTO
                    {
                        Seccion_Id = Casilla.Seccion_Id,
                        Seccion = Casilla.Seccion.Nombre,
                        Casilla_Id = Casilla.Id,
                        Casilla = $"{int.Parse(Casilla.Seccion.Nombre).ToString("D4")} {Casilla.Nombre}",
                        Tipo_Eleccion_Id = Tipo_Eleccion.Id,
                        Tipo_Eleccion = Tipo_Eleccion.Nombre,
                        Acta_URL = Votos_Nulos.Count() != 0 ? Votos_Nulos.FirstOrDefault(x => x.Tipo_Eleccion_Id == TipoEleccionId).Acta_Url_Local : null,
                        Votos_Candidaturas = Votos_Nulos.Count() != 0 ? Candidatos_DTO.Sum(x => x.Votos) : null,
                        Votos_Nulos = Votos_Nulos.Count() != 0 ? Votos_Nulos.Sum(x => x.Votos_Nulos) : null,
                        Total_Votos = Votos_Nulos.Count() != 0 ? (Candidatos_DTO.Sum(x => x.Votos) + (Votos_Nulos.Count() != 0 ? Votos_Nulos.Sum(x => x.Votos_Nulos) : 0)) : null,
                        Candidaturas = Candidatos_DTO
                    };
                    Lista.Add(Voto);

                    var Votos_Nulos_Adicional = List_Elecciones.Contains(TipoEleccionId) ? (await _ctx.Paquete_Tipo_Eleccion_Adicional.GetAllasync(x => List_Elecciones.Contains(x.Tipo_Eleccion_Id) && x.Paquete.Casilla_Id == Casilla.Id)) : (await _ctx.Paquete_Tipo_Eleccion_Adicional.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Paquete.Casilla_Id == Casilla.Id));

                    var JAO = Votos_Nulos_Adicional.Where(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Jao == true).Count();
                    var Estatal = Votos_Nulos_Adicional.Where(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Jao == false).Count();

                    if (JAO > 0)
                    {
                        var Candidatos_Adicional_JAO_DTO = _mapper.Map<IEnumerable<Candidato_DTO>>(Candidatos);

                        var Candidatos_Unicos_Adicional_JAO = Votos_Candidatos_Adicional.Where(x => x.Adicional_JAO == true).GroupBy(x => x.Candidato_Id).Select(g => new { Candidato_Id = g.Key, Votos = g.Sum(x => x.Votos) }).ToList();
                        var Votos_Candidatos_Totales_Adicional_JAO = Votos_Candidatos_Adicional.Where(x => x.Adicional_JAO == true).Sum(x => x.Votos);

                        Candidatos_Adicional_JAO_DTO = Candidatos_Adicional_JAO_DTO
                            .Select(x =>
                            {
                                var candidatoA = Candidatos_Unicos_Adicional_JAO.FirstOrDefault(c => c.Candidato_Id == x.Id);
                                var totalVotosA = Votos_Candidatos_Totales_Adicional_JAO + (Votos_Nulos_Adicional != null ? Votos_Nulos_Adicional.Where(x => x.Jao == true).Sum(n => n.Votos_Nulos) : 0);
                                x.Votos = candidatoA?.Votos;
                                x.Porcentaje = (x.Votos.HasValue && totalVotosA != 0) ? (decimal)x.Votos.Value / totalVotosA : 0;
                                x.Fotografia_URL = x.Fotografia_URL != null ? x.Fotografia_URL.Replace("http", "https").Replace("Candidatos//", "Candidatos/") : null;
                                return x;
                            })
                            .ToList();

                        var VotoA = new Votos_Secciones_DTO
                        {
                            Seccion_Id = Casilla.Seccion_Id,
                            Seccion = Casilla.Seccion.Nombre,
                            Casilla_Id = Casilla.Id,
                            Casilla = $"{int.Parse(Casilla.Seccion.Nombre).ToString("D4")} {Casilla.Nombre} (Adicional JAO)",
                            Tipo_Eleccion_Id = Tipo_Eleccion.Id,
                            Tipo_Eleccion = Tipo_Eleccion.Nombre,
                            Acta_URL = Votos_Nulos_Adicional.Count() != 0 ? Votos_Nulos_Adicional.FirstOrDefault(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Jao == true).Acta_Url_Local : null,
                            Votos_Candidaturas = Votos_Nulos_Adicional.Count() != 0 ? Candidatos_Adicional_JAO_DTO.Sum(x => x.Votos) : null,
                            Votos_Nulos = Votos_Nulos_Adicional.Count() != 0 ? Votos_Nulos_Adicional.Where(x => x.Jao == true).Sum(x => x.Votos_Nulos) : null,
                            Total_Votos = Votos_Nulos_Adicional.Count() != 0 ? (Candidatos_Adicional_JAO_DTO.Sum(x => x.Votos) + (Votos_Nulos_Adicional.Count() != 0 ? Votos_Nulos_Adicional.Where(x => x.Jao == true).Sum(x => x.Votos_Nulos) : 0)) : null,
                            Candidaturas = Candidatos_Adicional_JAO_DTO
                        };
                        Lista.Add(VotoA);
                    }

                    if (Estatal > 0)
                    {
                        var Candidatos_Adicional_DTO = _mapper.Map<IEnumerable<Candidato_DTO>>(Candidatos);

                        var Candidatos_Unicos_Adicional = Votos_Candidatos_Adicional.Where(x => x.Adicional == true).GroupBy(x => x.Candidato_Id).Select(g => new { Candidato_Id = g.Key, Votos = g.Sum(x => x.Votos) }).ToList();
                        var Votos_Candidatos_Totales_Adicional = Votos_Candidatos_Adicional.Where(x => x.Adicional == true).Sum(x => x.Votos);

                        Candidatos_Adicional_DTO = Candidatos_Adicional_DTO
                            .Select(x =>
                            {
                                var candidatoA = Candidatos_Unicos_Adicional.FirstOrDefault(c => c.Candidato_Id == x.Id);
                                var totalVotosA = Votos_Candidatos_Totales_Adicional + (Votos_Nulos_Adicional != null ? Votos_Nulos_Adicional.Where(x => x.Jao == false).Sum(n => n.Votos_Nulos) : 0);
                                x.Votos = candidatoA?.Votos;
                                x.Porcentaje = (x.Votos.HasValue && totalVotosA != 0) ? (decimal)x.Votos.Value / totalVotosA : 0;
                                x.Fotografia_URL = x.Fotografia_URL != null ? x.Fotografia_URL.Replace("http", "https").Replace("Candidatos//", "Candidatos/") : null;
                                return x;
                            })
                            .ToList();

                        var VotoA = new Votos_Secciones_DTO
                        {
                            Seccion_Id = Casilla.Seccion_Id,
                            Seccion = Casilla.Seccion.Nombre,
                            Casilla_Id = Casilla.Id,
                            Casilla = $"{int.Parse(Casilla.Seccion.Nombre).ToString("D4")} {Casilla.Nombre} (Adicional Estatal)",
                            Tipo_Eleccion_Id = Tipo_Eleccion.Id,
                            Tipo_Eleccion = Tipo_Eleccion.Nombre,
                            Acta_URL = Votos_Nulos_Adicional.Count() != 0 ? Votos_Nulos_Adicional.FirstOrDefault(x => x.Tipo_Eleccion_Id == TipoEleccionId && x.Jao == false).Acta_Url_Local : null,
                            Votos_Candidaturas = Votos_Nulos_Adicional.Count() != 0 ? Candidatos_Adicional_DTO.Sum(x => x.Votos) : null,
                            Votos_Nulos = Votos_Nulos_Adicional.Count() != 0 ? Votos_Nulos_Adicional.Where(x => x.Jao == false).Sum(x => x.Votos_Nulos) : null,
                            Total_Votos = Votos_Nulos_Adicional.Count() != 0 ? (Candidatos_Adicional_DTO.Sum(x => x.Votos) + (Votos_Nulos_Adicional.Count() != 0 ? Votos_Nulos_Adicional.Where(x => x.Jao == false).Sum(x => x.Votos_Nulos) : 0)) : null,
                            Candidaturas = Candidatos_Adicional_DTO
                        };
                        Lista.Add(VotoA);
                    }
                }

                return Ok(new { success = true, data = Lista });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = Commons.MensajeError });
            }
        }

        [HttpGet("GetImage/{Id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetImage(int Id)
        {
            try
            {
                var Candidato = await _ctx.Candidato.GetFirstOrdefaultAsync(x => x.Id == Id);
                var Nombre_Archivo = Path.GetFileName(Candidato.Fotografia_URL);
                var Extension = Path.GetExtension(Nombre_Archivo);
                var RutaImagen = Path.Combine(_host.WebRootPath, "Imagenes", "Candidatos", Nombre_Archivo);
                if (!System.IO.File.Exists(RutaImagen))
                    return NotFound("Imagen no encontrada en el servidor");
                
                var mimeType = Extension switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".gif" => "image/gif",
                    _ => "application/octet-stream"
                };

                var bytes = System.IO.File.ReadAllBytes(RutaImagen);
                var base64 = $"data:{mimeType};base64,{Convert.ToBase64String(bytes)}";
                return Ok(new { success = true, data = base64 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = Commons.MensajeError });
            }
        }

        [HttpGet("GetLista")]
        public async Task<IActionResult> GetLista()
        {
            try
            {
                var Candidatos = await _ctx.Candidato.GetAllasync();
                var Candidatos_DTO = Candidatos.Select(x => new Select_List_DTO
                {
                    Label = $"{x.Nombres} {x.Apellido_Paterno ?? ""} {x.Apellido_Materno ?? ""}",
                    Value = x.Id.ToString()
                });

                return Ok(new { success = true, data = Candidatos_DTO });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = Commons.MensajeError });
            }
        }

        [HttpGet("GetListaByTipoEleccion/{TipoEleccionId:int}")]
        public async Task<IActionResult> GetListaByTipoEleccion(int TipoEleccionId)
        {
            try
            {
                var Candidatos = await _ctx.Candidato.GetAllasync(x => x.Tipo_Eleccion_Id == TipoEleccionId);
                var Candidatos_DTO = Candidatos.Select(x => new Select_List_DTO
                {
                    Label = $"{x.Nombres} {x.Apellido_Paterno ?? ""} {x.Apellido_Materno ?? ""}",
                    Value = x.Id.ToString()
                });

                return Ok(new { success = true, data = Candidatos_DTO });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = Commons.MensajeError });
            }
        }
    }
}
