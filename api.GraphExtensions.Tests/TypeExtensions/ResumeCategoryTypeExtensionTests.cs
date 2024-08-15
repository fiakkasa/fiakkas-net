using api.EducationItems.Interfaces;
using api.EducationItems.Models;
using api.GraphExtensions.DataLoaders;
using api.GraphExtensions.TypeExtensions;

namespace api.GraphExtensions.Tests.TypeExtensions;

public class ResumeCategoryTypeExtensionTests
{
    [Fact]
    public async Task GetCustomers_Should_Return_Data()
    {
        var dataRepository = new MockDataRepository<IEducationItem>(
        [
            new EducationItem
            {
                Id = new("38898c62-161e-40f2-8a9f-39bf1ff46224"),
                CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedAt = null,
                Version = 1,
                CategoryId = new("eb9d6258-99c4-46bd-bd44-23d35b19965d"),
                TimePeriod = new()
                {
                    Start = new(2024, 1, 1),
                    End = null
                },
                Title = "Title",
                Href = new("/test", UriKind.Relative),
                Location = "Location",
                Description = "Description",
                Subjects = ["Subject"]
            }
        ]);
        var dataLoader = new EducationItemByResumeCategoryIdGroupDataLoader(
            dataRepository,
            AutoBatchScheduler.Default
        );
        var sut = new ResumeCategoryTypeExtension();

        var result = await sut.GetEducationItems(
            new()
            {
                Id = new("eb9d6258-99c4-46bd-bd44-23d35b19965d")
            },
            dataLoader,
            CancellationToken.None
        );

        result.Should().ContainSingle();
        result.MatchSnapshot();
    }
}
