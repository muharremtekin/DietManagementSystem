namespace DietManagementSystem.Application.RequestFeatures;

public class PagedList<T> : List<T>
{
    public MetaData MetaData { get; set; }
    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        MetaData = new MetaData()
        {
            TotalCount = count,
            CurrentPage = pageNumber,
            PageSize = pageSize,
            TotalPage = (int)Math.Ceiling(count / (double)pageSize)
        };
        AddRange(items);
    }
}

