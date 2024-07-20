using api.EducationItems.Interfaces;
using api.EducationItems.Models;

namespace api.EducationItems.Services;

public sealed class EducationItemDataRepository(ILogger<EducationItemDataRepository> logger, IOptionsSnapshot<EducationItemsDataConfig> dataSnapshot)
: InMemoryAbstractDataRepository<IEducationItem, EducationItemsDataConfig>(logger, dataSnapshot)
{
    protected override IEducationItem[]? ResolveSet(EducationItemsDataConfig data) => data.EducationItems;
}
