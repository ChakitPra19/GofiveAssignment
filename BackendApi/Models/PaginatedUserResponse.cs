namespace BackendApi.Models
{
    public class PaginatedUserResponse
{
    public List<User> DataSource { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}
}
