using DaraSurvey.WidgetServices.Models;
using System.Collections.Generic;

namespace DaraSurvey.Widgets.Dropdown
{
    public class EditModel : EditModelBase
    {
        public IEnumerable<Item> Items { get; set; }
    }
}
