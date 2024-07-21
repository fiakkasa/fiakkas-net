using api.Categories.Enums;
using api.Categories.Interfaces;

namespace api.Categories.Utils;

public static class CategoryEntityUtils
{
    public static bool IsUnknownCategory(ICategory item) =>
        item.Kind == CategoryType.None;

    public static bool IsPortfolioCategory(ICategory item) =>
        item.Kind == CategoryType.Portfolio;

    public static bool IsResumeCategory(ICategory item) =>
        item.Kind == CategoryType.Resume;

    public static bool IsSoftwareDevelopmentCategory(ICategory item) =>
        item.Kind == CategoryType.SoftwareDevelopment;

    public static bool IsInformationTechnologyCategory(ICategory item) =>
        item.Kind == CategoryType.InformationTechnology;

    public static bool IsTechnologyCategory(ICategory item) =>
        IsSoftwareDevelopmentCategory(item)
        || IsInformationTechnologyCategory(item);

    public static bool IsOtherCategory(ICategory item) =>
        item.Kind == CategoryType.Other;
}
