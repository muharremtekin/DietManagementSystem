namespace DietManagementSystem.Application.RequestFeatures;

public record MetaData
{
    public int CurrentPage { get; set; } = 1;
    public int TotalPage { get; set; }
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; set; }

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPage;
}
