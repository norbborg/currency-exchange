using Currency.Exchange.Public.Contracts.Requests;
using Currency.Exchange.Services;
using Microsoft.AspNetCore.Mvc;

namespace Currency.Exchange.Host.Controllers;

[ApiController]
[Route("[controller]")]
public class ExchangeController : ControllerBase
{
    private readonly IExchangeService _exchangeService;
    public ExchangeController(IExchangeService exchangeService)
    {
        _exchangeService = exchangeService;
    }
    
    [HttpPost]
    public async Task<ActionResult<string>> Trade([FromBody]TradeRequest request)
    {
        var result = await _exchangeService.AddTrade(request);
        
        return result.ToString();
    }
}