﻿using System.Web.Mvc;
using EvaluatingWebsitePerformance.BL.API.Handler;
using EvaluatingWebsitePerformance.Bootstrap;
using EvaluatingWebsitePerformance.Common.Interfaces;
using Ninject;

namespace EvaluatingWebsitePerformance.Controllers
{
    public class BaseController<TDto> : Controller where TDto : class, IBase
    {
        protected IKernel kernel;
        protected IHandler<TDto> handler;
        
        protected BaseController()
        {
            this.kernel = Kernel.Initialize();
            this.handler = kernel.Get<IHandler<TDto>>();
        }
        
    }
}