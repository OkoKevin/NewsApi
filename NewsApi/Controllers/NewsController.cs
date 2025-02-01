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
        private string? _apiKey { get; set; }

        public NewsController(IConfiguration configuration)
        {
            _apiKey = configuration.GetValue<string>("ApiKey");
        }

        [HttpGet("articles/{numberOfArticles}")]
        public async Task<IActionResult> GetNArticles(int numberOfArticles)
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
                string errorResponse = "The Api Key for the GNews API could not be found!";
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
            if (numberOfArticles <= 0 || numberOfArticles > 100)
            {
                string errorResponse = "The number of requested articles has to be between 1 and 100!";
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
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
