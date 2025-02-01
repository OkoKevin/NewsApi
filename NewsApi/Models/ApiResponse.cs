namespace NewsApi.Models
{
    public class ApiResponse
    {
        public int TotalArticles { get; set; }
        public List<Article> Articles { get; set; } = new List<Article>();
    }
}