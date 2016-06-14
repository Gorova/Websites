using System.Collections.Generic;
using EvaluatingWebsitePerformance.Common.Interfaces;

namespace EvaluatingWebsitePerformance.BL.API.Handler
{
    public interface IHandler<TDto> where TDto : class, IBase
    {
        TDto Get(int id);

        void Add(TDto data);

        IEnumerable<TDto> GetAll();

        void Update(TDto data);
    }
}
