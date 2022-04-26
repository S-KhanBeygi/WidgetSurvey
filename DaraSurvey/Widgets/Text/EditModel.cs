using DaraSurvey.WidgetServices.Models;
using System.Collections.Generic;
using System.Linq;

namespace DaraSurvey.Widgets.Text
{
    public class EditModel : EditModelBase
    {
        public string Text { get; set; }
        public string View { get; set; }
        public int MaximumLength { get; set; }

        public bool IsValid(IEnumerable<string> value)
        {
            return value.First().Length <= this.MaximumLength;
        }
    }
}