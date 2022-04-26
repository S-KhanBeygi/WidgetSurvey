using System;

namespace DaraSurvey.Core
{
    public abstract class FilterBase
    {
        public DateTime? MinCreated { get; set; }
        public DateTime? MaxCreated { get; set; }

        public DateTime? MinUpdated { get; set; }
        public DateTime? MaxUpdated { get; set; }
    }
}
