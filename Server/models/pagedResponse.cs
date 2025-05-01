namespace StratzClone.Server.Models
{
    public record PagedResponse<T>(IReadOnlyList<T> Items,
                                   int Page,
                                   int PageSize,
                                   int TotalCount);
}