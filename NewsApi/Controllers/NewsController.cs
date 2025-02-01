using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NewsApi.Models;
using Serilog;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace NewsApi.Controllers
{
    [Route("[controller]")]
    public class NewsController : ControllerBase
    {
        private string? _apiKey;

        public NewsController(IHttpContextAccessor httpContextAccessor)
        {
            _apiKey = (string?)httpContextAccessor.HttpContext.Items["apiKey"];
        }

        [HttpGet("articles/{numberOfArticles}")]
        public async Task<IActionResult> GetNArticles(int numberOfArticles)
        {
            //implemented according to the reference implementation from the GNews documentation
            string url = $"https://gnews.io/api/v4/top-headlines?max={numberOfArticles}&apikey={_apiKey}";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            ApiResponse data = JsonConvert.DeserializeObject<ApiResponse>(responseBody);
            List<Article> articles = data.Articles;
            return Ok(articles);
        }
    }
}
