using System.ComponentModel.DataAnnotations;

namespace EvaluatingWebsitePerformance.ViewModel
{
    public class WebsiteViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "field '{0}' should be fill in")]
        [StringLength(200, ErrorMessage = "Max length of field '{0}'- 200 symbols")]
        public string Name { get; set; }

        [Required(ErrorMessage = "field '{0}' should be fill in")]
        [StringLength(200, ErrorMessage = "Max length of field '{0}'- 200 symbols")]
        public string Url { get; set; }
    }
}