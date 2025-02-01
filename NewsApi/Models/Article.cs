namespace NewsApi.Models
{
    public class Article
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Content { get; set; } = "";
        public string Url { get; set; } = "";
        public string Image { get; set; } = "";
        public Source Source { get; set; } = new Source();
    }
}
