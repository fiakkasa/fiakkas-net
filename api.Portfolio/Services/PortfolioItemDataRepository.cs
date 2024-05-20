using api.Portfolio.Interfaces;
using api.Portfolio.Models;

namespace api.Portfolio.Services;

public class PortfolioItemDataRepository(ILogger<PortfolioItemDataRepository> logger, IOptionsSnapshot<PortfolioDataConfig> dataSnapshot)
: AbstractDataRepository<IPortfolioItem, PortfolioDataConfig>(logger, dataSnapshot)
{
    protected override T[]? ResolveSet<T>(PortfolioDataConfig data) => data.PortfolioItems switch
    {
        T[] result => result,
        _ => null,
    };
}
