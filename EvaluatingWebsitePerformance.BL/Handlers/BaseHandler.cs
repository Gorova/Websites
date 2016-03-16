using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
