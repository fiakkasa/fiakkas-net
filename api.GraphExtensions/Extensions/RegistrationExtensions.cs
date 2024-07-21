using api.GraphExtensions.DataLoaders;
using api.GraphExtensions.TypeExtensions;

namespace api.GraphExtensions.Extensions;

public static class RegistrationExtensions
{
    public static IRequestExecutorBuilder AddApiGraphExtensions(this IRequestExecutorBuilder builder) =>
        builder
            .AddDataLoader<CustomerByPortfolioCategoryIdGroupDataLoader>()
            .AddDataLoader<CustomerByTechnologyIdGroupDataLoader>()
            .AddDataLoader<EducationItemByResumeCategoryIdGroupDataLoader>()
            .AddDataLoader<PortfolioCategoryByCustomerIdGroupDataLoader>()
            .AddDataLoader<PortfolioCategoryByTechnologyIdGroupDataLoader>()
            .AddDataLoader<PortfolioItemByCustomerIdGroupDataLoader>()
            .AddDataLoader<PortfolioItemByPortfolioCategoryIdGroupDataLoader>()
            .AddDataLoader<PortfolioItemByTechnologyIdGroupDataLoader>()
            .AddDataLoader<PortfolioTechnologyCategoryByCustomerIdGroupDataLoader>()
            .AddDataLoader<PortfolioTechnologyCategoryByPortfolioCategoryIdGroupDataLoader>()
            .AddDataLoader<TechnologyCategoryGroupDataLoader>()
            .AddTypeExtension<CustomerTypeExtension>()
            .AddTypeExtension<EducationItemTypeExtension>()
            .AddTypeExtension<IBaseIdTypeExtension>()
            .AddTypeExtension<ITechnologyCategoryTypeExtension>()
            .AddTypeExtension<PortfolioCategoryTypeExtension>()
            .AddTypeExtension<PortfolioItemTypeExtension>()
            .AddTypeExtension<ResumeCategoryTypeExtension>();
}
