using System.Linq;

namespace EvaluatingWebsitePerformance.DAL.API.Repositories
{
    public interface IRepository
    {
        void Add<T>(T data) where T : class;

        IQueryable<T> Get<T>() where T : class;

        T Get<T>(int id) where T : class;

        void Save();
    }
}
