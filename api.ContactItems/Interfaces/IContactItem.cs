namespace api.ContactItems.Interfaces;

public interface IContactItem : IBaseData
{
    string Key { get; init; }
    string? Icon { get; init; }
    string Title { get; init; }
    string Description { get; init; }
    Uri? Href { get; init; }
}
