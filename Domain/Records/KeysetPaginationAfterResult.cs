using MR.AspNetCore.Pagination;

namespace Domain.Records
{
    public record KeysetPaginationAfterResult<T> : KeysetPaginationResult<T>
    {
        public string? After;
        public KeysetPaginationResult<T> KeysetPagination;
        public KeysetPaginationAfterResult(string? After, IReadOnlyList<T> Data, int TotalCount, int PageSize, bool HasPrevious, bool HasNext) : base(Data, TotalCount, PageSize, HasPrevious, HasNext)
        {
            this.After = After;
            KeysetPaginationResult<T> result = new(Data, TotalCount, PageSize, HasPrevious, HasNext);
            KeysetPagination = result;
        }

        public KeysetPaginationAfterResult(string? After, KeysetPaginationResult<T> result)
            : base(result)
        {
            this.After = After;
            KeysetPagination = result;
        }
    }
}
