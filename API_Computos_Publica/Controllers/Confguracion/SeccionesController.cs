using API_Computos_Publica.Data.Repository.Interfacez;
using API_Computos_Publica.Models.DTO.Commons;
using API_Computos_Publica.Models.DTO.Configuracion;
using API_Computos_Publica.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Security.Claims;

namespace API_Computos_Publica.Controllers.Confguracion
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeccionesController : ControllerBase
    {
        private readonly IUnitOfWork _ctx;
        private readonly ILogger<SeccionesController> _logger;
        private readonly IMapper _mapper;

        public SeccionesController(IUnitOfWork ctx, ILogger<SeccionesController> logger, IMapper mapper)
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
                var Secciones = await _ctx.Seccion.GetAllasync(
                    include: source => source.Include(x => x.Municipio));
                var Secciones_DTO = _mapper.Map<IEnumerable<Seccion_DTO>>(Secciones);
                return Ok(new { success = true, data = Secciones_DTO });
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
                var Seccion = await _ctx.Seccion.GetFirstOrdefaultAsync(
                    x => x.Id == Id, 
                    include: source => source.Include(x => x.Municipio));
                var Seccion_DTO = _mapper.Map<Seccion_DTO>(Seccion);
                return Ok(new { success = true, data = Seccion_DTO });
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
                var Secciones = await _ctx.Seccion.GetAllasync();
                var Secciones_DTO = Secciones.Select(C => new Select_List_DTO
                {
                    Label = C.Nombre,
                    Value = C.Id.ToString()
                });
                return Ok(new { success = true, data = Secciones_DTO });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = Commons.MensajeError });
            }
        }
    }
}
