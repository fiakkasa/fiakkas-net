// using api.Technologies.Interfaces;
// using api.Technologies.Models;

// namespace api.GraphExtensions.DataLoaders.Tests;

// public class TechnologyBatchDataLoaderTests
// {
//     [Fact]
//     public async Task LoadBatchAsync_Should_Return_Data_When_Matches_Found()
//     {
//         var dataRepository = new MockDataRepository<ITechnology>(
//         [
//             new Technology
//             {
//                 Id = new Guid("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3"),
//                 CreatedAt = new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
//                 UpdatedAt = null,
//                 Version = 1,
//                 Title = "Title",
//                 Href = new Uri("/test", UriKind.Relative)
//             }
//         ]);
//         var sut = new TechnologyBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

//         var result = await sut.LoadAsync([new Guid("ca832bf9-b7cb-4c31-bf8d-00f87a276fe3")], CancellationToken.None);

//         result.Should().NotBeEmpty();
//         result.MatchSnapshot();
//     }

//     [Fact]
//     public async Task LoadBatchAsync_Should_Return_Empty_Collection_When_No_Matches_Found()
//     {
//         var dataRepository = new MockDataRepository<ITechnology>();

//         var sut = new TechnologyBatchDataLoader(dataRepository, AutoBatchScheduler.Default);

//         var result = await sut.LoadAsync([Guid.NewGuid()], CancellationToken.None);

//         result.Should().NotBeEmpty();
//         result.MatchSnapshot();
//     }
// }
