using System.Collections.Generic;
using AutoMapper;
using EvaluatingWebsitePerformance.BL.API.Handler;
using EvaluatingWebsitePerformance.Common.DTO;
using EvaluatingWebsitePerformance.DAL.API.Repositories;
using EvaluatingWebsitePerformance.DAL.Entities;

namespace EvaluatingWebsitePerformance.BL.Handlers
{
    public class WebsiteHandler : BaseHandler, IHandler<WebsiteDto>
    {

        public WebsiteHandler(IRepository repository)
            : base(repository)
        {
        }

        public IEnumerable<WebsiteDto> GetAll()
        {
            var websitesDto = Mapper.Map<IEnumerable<Website>, IEnumerable<WebsiteDto>>(repository.GetAll<Website>());

            return websitesDto;
        }

        public WebsiteDto Get(int id)
        {
            var websiteDto = Mapper.Map<Website, WebsiteDto>(repository.Get<Website>(id));

            return websiteDto;
        }

        public void Add(WebsiteDto websiteDto)
        {
            var website = Mapper.Map<WebsiteDto, Website>(websiteDto);

            repository.Add(website);
            repository.Save();
        }

        public void Update(WebsiteDto websiteDto)
        {
            var website = repository.Get<Website>( websiteDto.Id);

            Mapper.Map(websiteDto, website);
            repository.Save();
        }
    }
}
