using Microsoft.AspNetCore.Mvc;

namespace Currency.Exchange.Host.Controllers;

[ApiController]
[Route("[controller]")]
public class ExchangeController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> Index()
    {
        return "Hello";
    }
}