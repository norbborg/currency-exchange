using Currency.Exchange.Public.Contracts.Requests;
using Currency.Exchange.Services;
using FluentValidation;

namespace Currency.Exchange.Validators;

public class TradeRequestValidator : AbstractValidator<TradeRequest>
{
    private readonly ISymbolService _symbolService;
    
    public TradeRequestValidator(ISymbolService symbolService)
    {
        _symbolService = symbolService;
        
        RuleFor(request => request.ClientId)
            .NotEmpty();

        RuleFor(request => request.FromCurrencyCode)
            .NotEmpty()
            .MustAsync(async (s, token) => await DoSymbolExist(s)).WithMessage("Invalid currency code.");
        
        RuleFor(request => request.ToCurrencyCode)
            .NotEmpty()
            .MustAsync(async (s, token) => await DoSymbolExist(s)).WithMessage("Invalid currency code.")
            .Must((request, s) => !s.Equals(request.FromCurrencyCode)).WithMessage("From and To currencies should be different.");
        
        RuleFor(request => request.Amount)
            .NotEmpty();
    }

    private async Task<bool> DoSymbolExist(string symbol)
    {
        var symbols = await _symbolService.GetSymbols();
        return symbols.ContainsKey(symbol);
    }
}