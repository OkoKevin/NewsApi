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
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }
}
