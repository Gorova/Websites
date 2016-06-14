using AutoMapper;
using EvaluatingWebsitePerformance.Common.DTO;
using EvaluatingWebsitePerformance.ViewModel;

namespace EvaluatingWebsitePerformance.App_Start
{
    public class AutoMapperConfig
    {
        public static void RegisterMapping()
        {
            DtoToViewModel();
            ViewModelToDto();
        }

        private static void DtoToViewModel()
        {
            Mapper.CreateMap<WebsiteDto, WebsiteViewModel>();
        }

        private static void ViewModelToDto()
        {
            Mapper.CreateMap<WebsiteViewModel, WebsiteDto>();
        }
    }
}