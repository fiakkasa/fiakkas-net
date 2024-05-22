using api.Portfolio.Interfaces;
using api.Portfolio.Models;

namespace api.Portfolio.Services;

public class PortfolioCategoryDataRepository(ILogger<PortfolioCategoryDataRepository> logger, IOptionsSnapshot<PortfolioDataConfig> dataSnapshot)
: AbstractDataRepository<IPortfolioCategory, PortfolioDataConfig>(logger, dataSnapshot)
{
    protected override IPortfolioCategory[]? ResolveSet(PortfolioDataConfig data) => data.PortfolioCategories;
}
