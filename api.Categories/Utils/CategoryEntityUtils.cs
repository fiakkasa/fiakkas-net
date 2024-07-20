using api.Categories.Enums;
using api.Categories.Interfaces;

namespace api.Categories.Utils;

public static class CategoryEntityUtils
{
    public static bool IsPortfolioCategory(ICategoryEntity item) =>
        item.Kind == CategoryType.Portfolio;

    public static bool IsResumeCategory(ICategoryEntity item) =>
        item.Kind == CategoryType.Resume;

    public static bool IsSoftwareDevelopmentCategory(ICategoryEntity item) =>
        item.Kind == CategoryType.SoftwareDevelopment;

    public static bool IsInformationTechnologyCategory(ICategoryEntity item) =>
        item.Kind == CategoryType.InformationTechnology;

    public static bool IsTechnologyCategory(ICategoryEntity item) =>
        IsSoftwareDevelopmentCategory(item)
        || IsInformationTechnologyCategory(item);

    public static bool IsOtherCategory(ICategoryEntity item) =>
        item.Kind == CategoryType.Other;

    public static bool IsUnknownCategory(ICategoryEntity item) =>
        item.Kind == CategoryType.None;
}
