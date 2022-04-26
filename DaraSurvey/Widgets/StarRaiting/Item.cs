using System.ComponentModel.DataAnnotations;

namespace DaraSurvey.Widgets.StarRaiting
{
    public class Item
    {
        [Required]
        public string Id { get; set; }
        public string Title { get; set; } // label
    }
}
