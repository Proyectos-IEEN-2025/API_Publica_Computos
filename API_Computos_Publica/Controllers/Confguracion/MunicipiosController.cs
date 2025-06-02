using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using API_Computos_Publica.Data.Repository.Interfacez;
using API_Computos_Publica.Models.DTO.Commons;
using API_Computos_Publica.Models.DTO.Configuracion;
using API_Computos_Publica.Utils;

namespace API_Computos_Publica.Controllers.Confguracion
{
    [Route("api/[controller]")]
    [ApiController]
    public class MunicipiosController : ControllerBase
    {
        private readonly IUnitOfWork _ctx;
        private readonly ILogger<MunicipiosController> _logger;
        private readonly IMapper _mapper;

        public MunicipiosController(IUnitOfWork ctx, ILogger<MunicipiosController> logger, IMapper mapper)
        {
            _ctx = ctx;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var Municipios = await _ctx.Municipio.GetAllasync(x => x.Estado.Nombre == "Nayarit", include: source => source.Include(x => x.Estado));
                var Municipios_DTO = _mapper.Map<IEnumerable<Municipio_DTO>>(Municipios);
                return Ok(new { success = true, data = Municipios_DTO });
            }
            catch (Exception ex)
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
                var Municipio = await _ctx.Municipio.GetFirstOrdefaultAsync(x => x.Id == Id);
                var Casilla_DTO = _mapper.Map<Municipio_DTO>(Municipio);
                return Ok(new { success = true, data = Casilla_DTO });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = Commons.MensajeError });
            }
        }

        [HttpGet("ByEstado/{EstadoId}")]
        public async Task<IActionResult> ByEstado(string EstadoId)
        {
            try
            {
                var Municipio = await _ctx.Municipio.GetAllasync(x => x.Estado_Id == EstadoId);
                var Municipio_DTO = _mapper.Map<List<Municipio_DTO>>(Municipio);
                return Ok(new { success = true, data = Municipio_DTO });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = Commons.MensajeError });
            }
        }

        [HttpGet("ByOficina/{OficinaId:int}")]
        public async Task<IActionResult> ByOficina(int OficinaId)
        {
            try
            {
                var Oficina = await _ctx.Oficina.GetFirstOrdefaultAsync(x => x.Id == OficinaId, 
                    include: source => source.Include(x => x.Municipio));
                var Municipio = await _ctx.Municipio.GetAllasync(x => x.Region == Oficina.Municipio.Region);
                var Municipio_DTO = _mapper.Map<List<Municipio_DTO>>(Municipio);
                return Ok(new { success = true, data = Municipio_DTO });
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
                var Municipios = await _ctx.Municipio.GetAllasync(x => x.Estado.Nombre == "Nayarit", include: source => source.Include(x => x.Estado));
                var Municipio_DTO = Municipios.Select(C => new Select_List_DTO
                {
                    Label = C.Nombre,
                    Value = C.Id.ToString()
                });
                return Ok(new { success = true, data = Municipio_DTO });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = Commons.MensajeError });
            }
        }
    }
}
