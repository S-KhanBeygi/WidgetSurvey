using System;
using System.ComponentModel.DataAnnotations;

namespace DaraSurvey.Core
{
    public abstract class EntityBase<TId> where TId : IConvertible
    {
        [Key]
        public TId Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        public DateTime? Deleted { get; set; }
    }
}
