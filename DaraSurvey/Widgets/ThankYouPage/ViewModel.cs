using DaraSurvey.WidgetServices.Models;

namespace DaraSurvey.Widgets.ThankYouPage
{
    public class ViewModel : ViewModelBase
    {
        public string Text { get; set; }

        public string Image { get; set; }

        public string ReturnUrl { get; set; }

        public override bool UserResponseIsValid(string userResponse) => true;
    }
}
