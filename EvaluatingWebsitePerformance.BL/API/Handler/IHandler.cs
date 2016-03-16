using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvaluatingWebsitePerformance.Common.Interfaces;

namespace EvaluatingWebsitePerformance.BL.API.Handler
{
    public interface IHandler<TDto> where TDto : class, IBase
    {
        TDto Get(int id);

        void Add(TDto data);

     }
}
