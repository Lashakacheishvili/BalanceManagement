using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BalanceManagement.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class BalanceController : Controller
    {
        [HttpGet("animals")]
        public string GetAnimals()
        {
            return "Test";
        }
    }
}
