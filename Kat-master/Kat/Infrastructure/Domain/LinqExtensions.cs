namespace Kat.Infrastructure.Domain
{
    public static class LinqExtensions
    {
        public static LookupDto GetLookupPaged(this IQueryable<LookupDto.Result> query, int page, int pageSize)
        {
            var result = new LookupDto();

            var pageCountRaw = (double)query.Count() / pageSize;
            var pageCount = (int)Math.Ceiling(pageCountRaw);

            result.Pagination = new LookupDto.PaginationObject()
            {
                More = false//page < pageCount
            };

            var skip = (page - 1) * pageSize;

            query = query.OrderBy(a => a.Text);

            result.Results = query.Skip(skip).Take(pageSize).ToArray();

            return result;
        }
    }
}
