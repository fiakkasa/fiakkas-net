namespace api.EducationItems.Interfaces;

public interface IEducationItem<TTimePeriod> : IBaseData
    where TTimePeriod : class, ITimePeriod, new()
{
    Guid CategoryId { get; init; }
    TTimePeriod TimePeriod { get; init; }
    string Title { get; init; }
    Uri? Href { get; init; }
    string Location { get; init; }
    string Description { get; init; }
    string[] Subjects { get; init; }
}

public interface IEducationItem : IEducationItem<TimePeriod> { }
