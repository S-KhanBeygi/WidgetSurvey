namespace DaraSurvey.WidgetServices.Models
{
    public abstract class ViewModelBase : WidgetModelBase
    {
        public abstract bool UserResponseIsValid(string userResponse);
    }
}