using DaraSurvey.WidgetServices.Models;

namespace DaraSurvey.Widgets.Text
{
    public class ViewModel : ViewModelBase
    {
        public string Text { get; set; }
        public string View { get; set; }
        public int MaximumLength { get; set; }

        public override bool UserResponseIsValid(string userResponse)
        {
            return userResponse.Length <= this.MaximumLength
                 ? true
                 : false;
        }
    }
}