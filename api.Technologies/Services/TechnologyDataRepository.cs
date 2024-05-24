using api.Technologies.Interfaces;
using api.Technologies.Models;

namespace api.Technologies.Services;

public class TechnologyDataRepository(ILogger<TechnologyDataRepository> logger, IOptionsSnapshot<TechnologiesDataConfig> dataSnapshot)
: AbstractDataRepository<ITechnology, TechnologiesDataConfig>(logger, dataSnapshot)
{
    protected override ITechnology[]? ResolveSet(TechnologiesDataConfig data) => data.Technologies;
}
