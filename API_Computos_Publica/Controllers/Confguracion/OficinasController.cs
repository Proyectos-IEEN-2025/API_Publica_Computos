using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Security.Claims;
using API_Computos_Publica.Data.Repository.Interfacez;
using API_Computos_Publica.Models.DTO.Commons;
using API_Computos_Publica.Models.DTO.Configuracion;
using API_Computos_Publica.Utils;

namespace API_Computos_Publica.Controllers.Confguracion
{
    [Route("api/[controller]")]
    [ApiController]
    public class OficinasController : ControllerBase
    {
        private readonly IUnitOfWork _ctx;
        private readonly ILogger<OficinasController> _logger;
        private readonly IMapper _mapper;

        public OficinasController(IUnitOfWork ctx, ILogger<OficinasController> logger, IMapper mapper)
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
                var Oficinas = await _ctx.Oficina.GetAllasync(x => x.OPLE == false, include: source => source.Include(x => x.Municipio));
                var Oficinas_DTO = _mapper.Map<IEnumerable<Oficina_DTO>>(Oficinas);
                Oficinas_DTO = Oficinas_DTO.OrderBy(x => x.No_Oficina);
                return Ok(new { success = true, data = Oficinas_DTO });
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
                var Oficina = await _ctx.Oficina.GetFirstOrdefaultAsync(x => x.Id == Id, include: source => source.Include(x => x.Municipio));
                var Oficina_DTO = _mapper.Map<Oficina_DTO>(Oficina);
                return Ok(new { success = true, data = Oficina_DTO });
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
                var Oficinas = await _ctx.Oficina.GetAllasync(x => x.OPLE == false);
                var Oficinas_DTO = Oficinas.Select(C => new Select_List_DTO
                {
                    Label = C.Nombre,
                    Value = C.Id.ToString()
                });
                return Ok(new { success = true, data = Oficinas_DTO });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = Commons.MensajeError });
            }
        }
    }
}
