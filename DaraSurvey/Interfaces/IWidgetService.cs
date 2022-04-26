using DaraSurvey.Entities;
using DaraSurvey.WidgetServices.Models;

namespace DaraSurvey.WidgetServices
{
    public interface IWidgetService
    {
        ViewModelBase GetViewModel(int id);
        Widget GetEntity(int id);
        ViewModelBase Create(EditModelBase model);
        ViewModelBase Updata(int id, EditModelBase model);
        void Delete(int id);
        ViewModelBase GetWidget(string widgetData);
    }
}