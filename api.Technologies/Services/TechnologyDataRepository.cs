using api.Technologies.Interfaces;
using api.Technologies.Models;

namespace api.Technologies.Services;

public class TechnologyDataRepository(ILogger<TechnologyDataRepository> logger, IOptionsSnapshot<TechnologyDataConfig> dataSnapshot)
: AbstractDataRepository<ITechnology, TechnologyDataConfig>(logger, dataSnapshot)
{
    protected override ITechnology[]? ResolveSet(TechnologyDataConfig data) => data.Technologies;
}
