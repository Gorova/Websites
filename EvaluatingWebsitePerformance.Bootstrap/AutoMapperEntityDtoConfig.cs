using AutoMapper;
using EvaluatingWebsitePerformance.Common.DTO;
using EvaluatingWebsitePerformance.DAL.Entities;

namespace EvaluatingWebsitePerformance.Bootstrap
{
    public class AutoMapperEntityDtoConfig
    {
        public static void RegisterMapping()
        {
            EntityToDto();
            DtoToEntity();
        }

        private static void EntityToDto()
        {
            Mapper.CreateMap<Website, WebsiteDto>();
        }

        private static void DtoToEntity()
        {
            Mapper.CreateMap<WebsiteDto, Website>();
        }
    }
}
