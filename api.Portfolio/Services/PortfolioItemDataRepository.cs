using api.Portfolio.Interfaces;
using api.Portfolio.Models;

namespace api.Portfolio.Services;

public sealed class PortfolioItemDataRepository(
    ILogger<PortfolioItemDataRepository> logger,
    IOptionsSnapshot<PortfolioDataConfig> dataSnapshot
)
    : AbstractInMemoryDataRepository<IPortfolioItem, PortfolioDataConfig>(logger, dataSnapshot)
{
    protected override IReadOnlyCollection<IPortfolioItem>? ResolveSet(PortfolioDataConfig data) => data.PortfolioItems;
}
