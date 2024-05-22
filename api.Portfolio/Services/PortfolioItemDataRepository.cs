using api.Portfolio.Interfaces;
using api.Portfolio.Models;

namespace api.Portfolio.Services;

public class PortfolioItemDataRepository(ILogger<PortfolioItemDataRepository> logger, IOptionsSnapshot<PortfolioDataConfig> dataSnapshot)
: AbstractDataRepository<IPortfolioItem, PortfolioDataConfig>(logger, dataSnapshot)
{
    protected override IPortfolioItem[]? ResolveSet(PortfolioDataConfig data) => data.PortfolioItems;
}
