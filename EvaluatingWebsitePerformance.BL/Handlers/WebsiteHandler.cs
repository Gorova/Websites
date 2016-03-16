using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
