using api.GraphExtensions.DataLoaders;
using api.GraphExtensions.TypeExtensions;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace api.GraphExtensions.Extensions;

public static class RegistrationExtensions
{
    public static IRequestExecutorBuilder AddApiGraphExtensions(this IRequestExecutorBuilder builder) =>
        builder
            .AddDataLoader<CustomerBatchDataLoader>()
            .AddDataLoader<CustomerByPortfolioCategoryIdGroupDataLoader>()
            .AddDataLoader<CustomerByTechnologyIdGroupDataLoader>()
            .AddDataLoader<PortfolioCategoryByCustomerIdGroupDataLoader>()
            .AddDataLoader<PortfolioCategoryByTechnologyIdGroupDataLoader>()
            .AddDataLoader<PortfolioItemByCustomerIdGroupDataLoader>()
            .AddDataLoader<PortfolioItemByTechnologyIdGroupDataLoader>()
            .AddDataLoader<TechnologyBatchDataLoader>()
            .AddDataLoader<TechnologyByCustomerIdGroupDataLoader>()
            .AddDataLoader<TechnologyByPortfolioCategoryIdGroupDataLoader>()
            .AddTypeExtension<CustomerTypeExtension>()
            .AddTypeExtension<PortfolioCategoryTypeExtension>()
            .AddTypeExtension<PortfolioItemTypeExtension>()
            .AddTypeExtension<TechnologyTypeExtension>();
}
