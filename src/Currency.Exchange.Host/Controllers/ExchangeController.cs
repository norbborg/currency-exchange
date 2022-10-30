using Currency.Exchange.Public.Contracts.Requests;
using Currency.Exchange.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Currency.Exchange.Host.Controllers;

[ApiController]
[Route("[controller]")]
public class ExchangeController : ControllerBase
{
    private readonly IValidator<TradeRequest> _validator;
    private readonly IExchangeService _exchangeService;
    public ExchangeController(IExchangeService exchangeService, IValidator<TradeRequest> validator)
    {
        _exchangeService = exchangeService;
        _validator = validator;
    }
    
    [HttpPost]
    public async Task<ActionResult<string>> Trade([FromBody]TradeRequest request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors.ToCurrencyExchangeErrors());
        }

        var result = await _exchangeService.AddTrade(request);
        
        return result.ToString();
    }
}