namespace Kat.Infrastructure.Domain
{
    public class LookupDto
    {
        public Result[]? Results { get; set; }
        public PaginationObject? Pagination { get; set; }

        public class Result
        {
            public string? Id { get; set; }
            public string? Text { get; set; }
        }

        public class PaginationObject
        {
            public bool More { get; set; }
        }

    }
}
