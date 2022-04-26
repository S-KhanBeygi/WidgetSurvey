using DaraSurvey.WidgetServices.Models;
using System.Collections.Generic;

namespace DaraSurvey.Widgets.StarRaiting
{
    public class EditModel : EditModelBase
    {
        public string Icon { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
}
