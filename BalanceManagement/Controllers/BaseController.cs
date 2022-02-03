using Microsoft.AspNetCore.Mvc;

namespace BalanceManagement.Controllers
{
    public class BaseController : Controller
    {
        protected string GetHeaderLanguage()
        {
            return Request.Headers["accept-language"].ToString();
        }
    }
}
