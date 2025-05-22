using API_Computos_Publica.Models.DTO.Computos;
using API_Computos_Publica.Models.DTO.Configuracion;
using API_Computos_Publica.Models.Entities.Computos;
using API_Computos_Publica.Models.Entities.Configuracion;
using API_Computos_Publica.Utils;
using AutoMapper;
using System.Linq;
using System.Reflection;

namespace API_Computos_Publica.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            #region Configuracion
            CreateMap<Casilla, Casilla_DTO>()
                .ForMember(c => c.Municipio, options => options.MapFrom(c => c.Municipio.Nombre))
                .ForMember(c => c.Seccion, options => options.MapFrom(c => c.Seccion.Nombre))
                .ForMember(c => c.Tipo_Casilla, options => options.MapFrom(c => c.Tipos_Casilla.Nombre));

            CreateMap<Estado, Estado_DTO>();

            CreateMap<Municipio, Municipio_DTO>();

            CreateMap<Seccion, Seccion_DTO>()
                .ForMember(s => s.Municipio, options => options.MapFrom(s => s.Municipio.Nombre))
                .ForMember(s => s.Region, options => options.MapFrom(s => s.Municipio.Region));

            CreateMap<Tipo_Casilla, Tipo_Casilla_DTO>();

            CreateMap<Tipo_Eleccion, Tipo_Eleccion_DTO>();

            CreateMap<Oficina, Oficina_DTO>()
                .ForMember(o => o.Municipio, options => options.MapFrom(o => o.Municipio.Nombre));

            CreateMap<Seccion, Seccion_DTO>()
                .ForMember(s => s.Municipio, options => options.MapFrom(s => s.Municipio.Nombre));

            CreateMap<Tipo_Casilla, Tipo_Casilla_DTO>();

            CreateMap<Candidato, Candidato_DTO>()
                .ForMember(x => x.Tipo_Eleccion, options => options.MapFrom(x => x.Tipo_Eleccion.Nombre))
                .ForMember(x => x.Edad, options => options.MapFrom(x => x.Fecha_Nacimiento == null ? 0 : Commons.CalcularEdad(x.Fecha_Nacimiento.Value)));

            #endregion

            #region Computos
            CreateMap<Paquete_Tipo_Eleccion, Paquete_Tipo_Eleccion_DTO>()
                .ForMember(x => x.Tipo_Eleccion, options => options.MapFrom(x => x.Tipo_Eleccion.Nombre));

            CreateMap<Actas_Parciales, Acta_Parcial_DTO>()
                .ForMember(x => x.Oficina, options => options.MapFrom(x => x.Oficina.Nombre))
                .ForMember(x => x.Tipo_Eleccion, options => options.MapFrom(x => x.Tipo_Eleccion.Nombre));

            CreateMap<Actas_Estatales, Acta_Estatal_DTO>()
                .ForMember(x => x.Tipo_Eleccion, options => options.MapFrom(x => x.Tipo_Eleccion.Nombre));
            #endregion
        }
    }
}
