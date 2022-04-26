using DaraSurvey.WidgetServices.Models;
using System.Collections.Generic;
using System.Linq;

namespace DaraSurvey.Widgets.Dropdown
{
    public class ViewModel : ViewModelBase
    {
        public IEnumerable<Item> Items { get; set; }

        public override bool UserResponseIsValid(string userResponse)
        {
            var validValues = Items.Select(o => o.Id.ToString());

            return validValues.Contains(userResponse)
                ? true
                : false;
        }
    }
}