namespace api.TextItems.Interfaces;

public interface ITextItem : IBaseData
{
    string Key { get; init; }
    string Title { get; init; }
    string Content { get; init; }
}
