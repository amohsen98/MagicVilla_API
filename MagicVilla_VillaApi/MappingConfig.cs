using AutoMapper;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.DTO;
using System.Runtime;

namespace MagicVilla_VillaApi
{
    public class MappingConfig : Profile 
    {
        public MappingConfig() {

            CreateMap<Villa, VillaDTO>();
            CreateMap<VillaDTO, Villa>();
            CreateMap<Villa, VillaCreateDTO>().ReverseMap();
            CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();

        }
    }
}
