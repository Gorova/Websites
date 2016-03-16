using System.Data.Entity;
using EvaluatingWebsitePerformance.BL.API.Handler;
using EvaluatingWebsitePerformance.BL.Handlers;
using EvaluatingWebsitePerformance.Common.DTO;
using EvaluatingWebsitePerformance.DAL;
using EvaluatingWebsitePerformance.DAL.API.Repositories;
using EvaluatingWebsitePerformance.DAL.Repositories;
using Ninject.Modules;

namespace EvaluatingWebsitePerformance.Bootstrap
{
    public class LibraryModule : NinjectModule
    {
        public override void Load()
        {
            this.InitializeRepositories();
        }

        private void InitializeRepositories()
        {
            Bind<DbContext>().To<EvaluatingWebsiteContext>();
            Bind<IRepository>().To<Repository>();
            Bind<IHandler<WebsiteDto>>().To<WebsiteHandler>();
        }
    }
}
