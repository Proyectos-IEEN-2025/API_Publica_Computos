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
