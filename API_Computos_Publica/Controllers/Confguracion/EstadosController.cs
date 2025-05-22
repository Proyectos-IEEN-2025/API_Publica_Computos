using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using API_Computos_Publica.Data.Repository.Interfacez;
using API_Computos_Publica.Models.DTO.Commons;
using API_Computos_Publica.Models.DTO.Configuracion;
using API_Computos_Publica.Utils;

namespace API_Computos_Publica.Controllers.Confguracion
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadosController : ControllerBase
    {
        private readonly IUnitOfWork _ctx;
        private readonly ILogger<EstadosController> _logger;
        private readonly IMapper _mapper;

        public EstadosController(IUnitOfWork ctx, ILogger<EstadosController> logger, IMapper mapper)
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
                var Estados = await _ctx.Estado.GetAllasync();
                var Distritos_DTO = _mapper.Map<IEnumerable<Estado_DTO>>(Estados);
                return Ok(new { success = true, data = Distritos_DTO });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = Commons.MensajeError });
            }
        }

        [HttpGet("{Clave}")]
        public async Task<IActionResult> Get(string Clave)
        {
            try
            {
                var Estado = await _ctx.Estado.GetFirstOrdefaultAsync(x => x.Clave == Clave);
                var Estado_DTO = _mapper.Map<Estado_DTO>(Estado);
                return Ok(new { success = true, data = Estado_DTO });
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
                var Estados = await _ctx.Estado.GetAllasync();
                var Estados_DTO = Estados.Select(C => new Select_List_DTO
                {
                    Label = C.Nombre,
                    Value = C.Clave
                });
                return Ok(new { success = true, data = Estados_DTO });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = Commons.MensajeError });
            }
        }
    }
}
