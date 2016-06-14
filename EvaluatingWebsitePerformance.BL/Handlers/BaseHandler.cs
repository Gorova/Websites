using EvaluatingWebsitePerformance.DAL.API.Repositories;

namespace EvaluatingWebsitePerformance.BL.Handlers
{
    public abstract class BaseHandler 
    {
        protected IRepository repository;

        protected BaseHandler(IRepository repository)
        {
            this.repository = repository;
        }
    }
}
