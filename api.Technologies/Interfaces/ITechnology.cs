namespace api.Technologies.Interfaces;

public interface ITechnology : IBaseData
{
    string Title { get; init; }
    Uri? Href { get; init; }
}
