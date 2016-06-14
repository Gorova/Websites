using System.Security.Cryptography.X509Certificates;

namespace EvaluatingWebsitePerformance.DAL.Entities
{
    public class Website
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public long MillisecondsOfLoading { get; set; }

        public int ParentId { get; set; }

        public bool IsProcessed { get; set; }
    }
}
