using System.ComponentModel.DataAnnotations;

namespace DaraSurvey.WidgetServices.Models
{
    public abstract class WidgetModelBase
    {
        [Required]
        public string Type { get; set; }

        [Required]
        public string Id { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Url { get; set; }

        public string ViewData { get; set; }
    }
}