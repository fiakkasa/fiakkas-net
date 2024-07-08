using api.GraphExtensions.DataLoaders;
using api.GraphExtensions.TypeExtensions;

namespace api.GraphExtensions.Extensions;

public static class RegistrationExtensions
{
    public static IRequestExecutorBuilder AddApiGraphExtensions(this IRequestExecutorBuilder builder) =>
        builder
            .AddDataLoader<CustomerBatchDataLoader>()
            .AddDataLoader<CustomerByPortfolioCategoryIdGroupDataLoader>()
            .AddDataLoader<CustomerByTechnologyIdGroupDataLoader>()
            .AddDataLoader<EducationItemByResumeCategoryIdGroupDataLoader>()
            .AddDataLoader<PortfolioCategoryBatchDataLoader>()
            .AddDataLoader<PortfolioCategoryByCustomerIdGroupDataLoader>()
            .AddDataLoader<PortfolioCategoryByTechnologyIdGroupDataLoader>()
            .AddDataLoader<PortfolioItemByCustomerIdGroupDataLoader>()
            .AddDataLoader<PortfolioItemByPortfolioCategoryIdGroupDataLoader>()
            .AddDataLoader<PortfolioItemByTechnologyIdGroupDataLoader>()
            .AddDataLoader<PortfolioTechnologyCategoryByCustomerIdGroupDataLoader>()
            .AddDataLoader<PortfolioTechnologyCategoryByPortfolioCategoryIdGroupDataLoader>()
            .AddDataLoader<ResumeCategoryBatchDataLoader>()
            .AddDataLoader<TechnologyCategoryBatchDataLoader>()
            .AddTypeExtension<CustomerTypeExtension>()
            .AddTypeExtension<EducationItemTypeExtension>()
            .AddTypeExtension<ITechnologyCategoryTypeExtension>()
            .AddTypeExtension<PortfolioCategoryTypeExtension>()
            .AddTypeExtension<ResumeCategoryTypeExtension>()
            .AddTypeExtension<PortfolioItemTypeExtension>();
}
