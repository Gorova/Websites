using EvaluatingWebsitePerformance.Common.Interfaces;

namespace EvaluatingWebsitePerformance.Common.DTO
{
    public class WebsiteDto : IBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }
    }
}
