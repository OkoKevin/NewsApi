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
using System.Security.Policy;

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

        [HttpGet("articles")]
        public async Task<IActionResult> GetNArticles([FromQuery] int limit)
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
                string errorResponse = "The Api Key for the GNews API could not be found!";
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
            if (limit <= 0 || limit > 100)
            {
                string errorResponse = "A 'limit' query parameter between 1 and 100 is required!";
                return BadRequest(errorResponse);
            }
            //implemented according to the reference implementation from the GNews documentation
            string url = $"https://gnews.io/api/v4/top-headlines?max={limit}&apikey={_apiKey}";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            ApiResponse data = JsonConvert.DeserializeObject<ApiResponse>(responseBody);
            List<Article> articles = data.Articles;
            return Ok(articles);
        }

        [HttpGet("articles/search")]
        public async Task<IActionResult> SearchArticles([FromQuery] string query, [FromQuery] int limit)
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
                string errorResponse = "The Api Key for the GNews API could not be found!";
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
            if (limit <= 0 || limit > 100)
            {
                string errorResponse = "A 'limit' query parameter between 1 and 100 is required!";
                return BadRequest(errorResponse);
            }
            if (string.IsNullOrEmpty(query))
            {
                string errorResponse = "A 'query' query parameter is required!";
                return BadRequest(errorResponse);
            }
            //implemented according to the reference implementation from the GNews documentation
            string url = $"https://gnews.io/api/v4/search?q={query}&max={limit}&apikey={_apiKey}";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ApiResponse>(responseBody);
            List<Article> articles = data.Articles;
            return Ok(articles);
        }
    }
}
