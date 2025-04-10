using DietManagementSystem.Application.RequestFeatures;
using System.Text.Json;

namespace DietManagementSystem.WebApi.Extensions;

public static class HttpExtensions
{
    public static void AddPaginationHeader(this HttpResponse response, MetaData metaData)
    {
        response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metaData));
    }
}