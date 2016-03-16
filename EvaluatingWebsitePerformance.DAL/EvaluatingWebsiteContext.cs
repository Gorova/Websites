using System.Data.Entity;
using EvaluatingWebsitePerformance.DAL.Entities;

namespace EvaluatingWebsitePerformance.DAL
{
    public class EvaluatingWebsiteContext : DbContext
    {
        public EvaluatingWebsiteContext()
            : base("EvaluatingWebsite")
        {
        }

        public DbSet<Website> Websites { get; set; }
    }
}
