using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using OfficeOpenXml;
using System.ComponentModel;
using System.IO;
using API_Computos_Publica.Data.Repository.Interfacez;
using API_Computos_Publica.Models.DTO.Commons;
using API_Computos_Publica.Models.DTO.Configuracion;
using API_Computos_Publica.Utils;

namespace API_Computos_Publica.Controllers.Confguracion
{
    [Route("api/[controller]")]
    [ApiController]
    public class CasillasController : ControllerBase
    {
        private readonly IUnitOfWork _ctx;
        private readonly ILogger<CasillasController> _logger;
        private readonly IMapper _mapper;

        public CasillasController(IUnitOfWork ctx, ILogger<CasillasController> logger, IMapper mapper)
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
                var Casillas = await _ctx.Casilla.GetAllasync(include: source => source.Include(x => x.Municipio).Include(x => x.Seccion).Include(x => x.Tipos_Casilla));
                var Casillas_DTO = _mapper.Map<IEnumerable<Casilla_DTO>>(Casillas);
                return Ok(new { success = true, data = Casillas_DTO });
            }
            catch(Exception ex)
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
                var Casilla = await _ctx.Casilla.GetFirstOrdefaultAsync(x => x.Id == Id, include: source => source.Include(x => x.Municipio).Include(x => x.Seccion).Include(x => x.Tipos_Casilla));
                var Casilla_DTO = _mapper.Map<Casilla_DTO>(Casilla);
                return Ok(new { success = true, data = Casilla_DTO });
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
                var Casillas = await _ctx.Casilla.GetAllasync();
                var Casillas_DTO = Casillas.Select(C => new Select_List_DTO
                {
                    Label = C.Nombre,
                    Value = C.Id.ToString()
                });
                return Ok(new { success = true, data = Casillas_DTO });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = Commons.MensajeError });
            }
        }

        [HttpGet("GetListaByMunicipio/{MunicipioId:int}")]
        public async Task<IActionResult> GetListaByMunicipio(int MunicipioId)
        {
            try
            {
                var Casillas = await _ctx.Casilla.GetAllasync(x => x.Municipio_Id == MunicipioId);
                var Casillas_DTO = Casillas.Select(C => new Select_List_DTO
                {
                    Label = C.Nombre,
                    Value = C.Id.ToString()
                });
                return Ok(new { success = true, data = Casillas_DTO });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode(500, new { codigo = "500", data = Commons.MensajeError });
            }
        }
    }
}
