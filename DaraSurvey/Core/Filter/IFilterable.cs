namespace DaraSurvey.Core
{
    public interface IFilterable { }

    // --------------------

    public interface IOrderedFilterable : IFilterable
    {
        int? Skip { get; set; }
        int? Take { get; set; }
        string Sort { get; set; }
        bool Asc { get; set; }
        bool RndArgmnt { get; set; }
    }
}
