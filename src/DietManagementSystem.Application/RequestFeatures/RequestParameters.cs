using DietManagementSystem.Common.Constants;

namespace DietManagementSystem.Application.RequestFeatures;

public record RequestParameters
{
    const int maxPageSize = DefaultSizes.MaxPageSize;
    public int PageNumber { get; set; } = DefaultSizes.DefaultPageNumber;

    private int _pageSize = DefaultSizes.DefaultPageSize;

    public int PageSize
    {
        get { return _pageSize; }
        set { _pageSize = value > maxPageSize ? maxPageSize : value; }
    }
}