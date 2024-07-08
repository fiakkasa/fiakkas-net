using api.EducationItems.Interfaces;
using api.EducationItems.Models;

namespace api.EducationItems.Services;

public sealed class EducationItemDataRepository(ILogger<EducationItemDataRepository> logger, IOptionsSnapshot<EducationItemsDataConfig> dataSnapshot)
: AbstractDataRepository<IEducationItem<EducationTimePeriod>, EducationItemsDataConfig>(logger, dataSnapshot)
{
    protected override IEducationItem<EducationTimePeriod>[]? ResolveSet(EducationItemsDataConfig data) => data.EducationItems;
}
