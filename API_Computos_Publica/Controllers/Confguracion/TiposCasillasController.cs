using API_Computos_Publica.Data.Repository.Interfacez;
using API_Computos_Publica.Models.DTO.Commons;
using API_Computos_Publica.Models.DTO.Configuracion;
using API_Computos_Publica.Models.Entities.Configuracion;
using API_Computos_Publica.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace API_Computos_Publica.Controllers.Confguracion
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiposCasillasController : ControllerBase
    {
        private readonly IUnitOfWork _ctx;
        private readonly ILogger<TiposCasillasController> _logger;
        private readonly IMapper _mapper;

        public TiposCasillasController(IUnitOfWork ctx, ILogger<TiposCasillasController> logger, IMapper mapper)
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
                var Tipos_Casilla = await _ctx.Tipo_Casilla.GetAllasync();
                var Tipos_Casilla_DTO = _mapper.Map<IEnumerable<Tipo_Casilla_DTO>>(Tipos_Casilla);
                return Ok(new { success = true, data = Tipos_Casilla_DTO });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = Commons.MensajeError });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int Id)
        {
            try
            {
                var Tipo = await _ctx.Tipo_Casilla.GetFirstOrdefaultAsync(x => x.Id == Id);
                var Tipo_DTO = _mapper.Map<Tipo_Casilla>(Tipo);
                return Ok(new { success = true, data = Tipo_DTO });
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
                var Tipos_Casilla = await _ctx.Tipo_Casilla.GetAllasync();
                var Tipos_Casilla_DTO = Tipos_Casilla.Select(C => new Select_List_DTO
                {
                    Label = C.Nombre,
                    Value = C.Id.ToString()
                });
                return Ok(new { success = true, data = Tipos_Casilla_DTO });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = Commons.MensajeError });
            }
        }
    }
}
