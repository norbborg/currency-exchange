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
    private readonly ILogger<ExchangeController> _logger;

    public ExchangeController(IExchangeService exchangeService, IValidator<TradeRequest> validator,
        ILogger<ExchangeController> logger)
    {
        _exchangeService = exchangeService;
        _validator = validator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<string>> Trade([FromBody] TradeRequest request)
    {
        _logger.LogDebug("{Controller} handling request {Endpoint} with payload {@Payload}",
            nameof(ExchangeController),
            nameof(Trade),
            request);

        try
        {
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors.ToCurrencyExchangeErrors());
            }

            var result = await _exchangeService.AddTradeAsync(request);

            return result.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError("{Controller} handling Request {Endpoint} with Payload {@Payload} failed with Exception {Exception}",
                nameof(ExchangeController),
                nameof(Trade),
                request,
                ex.Message);
            
            throw;
        }
    }
}