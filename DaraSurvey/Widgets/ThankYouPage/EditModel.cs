using DaraSurvey.WidgetServices.Models;

namespace DaraSurvey.Widgets.ThankYouPage
{
    public class EditModel : EditModelBase
    {
        public string Text { get; set; }

        public string Image { get; set; }

        public string ReturnUrl { get; set; }
    }
}
