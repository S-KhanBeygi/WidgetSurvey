using DaraSurvey.WidgetServices.Models;
using System.Collections.Generic;
using System.Linq;

namespace DaraSurvey.Widgets.StarRaiting
{
    public class ViewModel : ViewModelBase
    {
        public string Icon { get; set; }
        public IEnumerable<Item> Items { get; set; }

        public override bool UserResponseIsValid(string userResponse)
        {
            var validResponses = Items.Select(o => o.Id);
            return validResponses.Contains(userResponse)
                ? true
                : false;
        }
    }
}
