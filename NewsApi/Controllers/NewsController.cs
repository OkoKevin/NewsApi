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

        /// <summary>
        /// Requires a 'limit' query parameter between 1 and 100. 
        /// Returns a number of news articles depending on the limit
        /// </summary>
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
            string responseBody = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, responseBody);
            }
            ApiResponse data = JsonConvert.DeserializeObject<ApiResponse>(responseBody);
            List<Article> articles = data.Articles;
            return Ok(articles);
        }

        /// <summary>
        /// Searches for news articles filtered by the keyword in the 'query' parameter. 
        /// Returns news articles filtered by the keyword in the 'query' parameter. 
        /// </summary>
        [HttpGet("articles/search")]
        public async Task<IActionResult> SearchArticles([FromQuery] string? query, [FromQuery] int limit = 1)
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
            string url = $"https://gnews.io/api/v4/top-headlines?q={query}&max={limit}&apikey={_apiKey}";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, responseBody);
            }
            var data = JsonConvert.DeserializeObject<ApiResponse>(responseBody);
            List<Article> articles = data.Articles;
            return Ok(articles);
        }

        /// <summary>
        /// Filters news articles by date. 
        /// Requires a 'from' and a 'to' date in the following format: 2022-08-21T16:27:09Z 
        /// Returns news articles filtered by 'from' and 'to' date.
        /// </summary>
        [HttpGet("articles/bydate")]
        public async Task<IActionResult> FilterArticlesByDate([FromQuery] DateTime? from, [FromQuery] DateTime? to,[FromQuery] int limit = 100)
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
            if (!from.HasValue)
            {
                string errorResponse = "A 'from' query parameter is required!";
                return BadRequest(errorResponse);
            }
            if (!to.HasValue)
            {
                string errorResponse = "A 'to' query parameter is required!";
                return BadRequest(errorResponse);
            }
            //implemented according to the reference implementation from the GNews documentation
            string url = $"https://gnews.io/api/v4/top-headlines?from={from}&to={to}&max={limit}&apikey={_apiKey}";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, responseBody);
            }
            var data = JsonConvert.DeserializeObject<ApiResponse>(responseBody);
            List<Article> articles = data.Articles;
            return Ok(articles);
        }
    }
}
