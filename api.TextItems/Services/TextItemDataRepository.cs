using api.TextItems.Interfaces;
using api.TextItems.Models;

namespace api.TextItems.Services;

public sealed class TextItemDataRepository(
    ILogger<TextItemDataRepository> logger,
    IOptionsSnapshot<TextItemsDataConfig> dataSnapshot
) : AbstractReadOnlyInMemoryDataRepository<ITextItem, TextItemsDataConfig>(logger, dataSnapshot)
{
    protected override IReadOnlyCollection<ITextItem>? ResolveSet(TextItemsDataConfig data) => data.TextItems;
}
