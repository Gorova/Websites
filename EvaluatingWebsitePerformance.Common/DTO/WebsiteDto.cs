using EvaluatingWebsitePerformance.Common.Interfaces;

namespace EvaluatingWebsitePerformance.Common.DTO
{
    public class WebsiteDto : IBase
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public long MillisecondsOfLoading { get; set; }

        public int ParentId { get; set; }

        public bool IsProcessed { get; set; }
    }
}
