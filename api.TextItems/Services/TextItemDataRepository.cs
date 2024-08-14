using api.TextItems.Interfaces;
using api.TextItems.Models;

namespace api.TextItems.Services;

public sealed class TextItemDataRepository(
    ILogger<TextItemDataRepository> logger,
    IOptionsSnapshot<TextItemsDataConfig> dataSnapshot
)
    : AbstractInMemoryDataRepository<ITextItem, TextItemsDataConfig>(logger, dataSnapshot)
{
    protected override ITextItem[]? ResolveSet(TextItemsDataConfig data) => data.TextItems;
}
