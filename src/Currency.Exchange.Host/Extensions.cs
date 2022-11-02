using FluentValidation.Results;

namespace Currency.Exchange.Host;

public static class Extensions
{
    public static IEnumerable<ErrorMessage> ToCurrencyExchangeErrors(this IEnumerable<ValidationFailure> errors)
    {
        return errors.Select(e => new ErrorMessage
        {
            PropertyName = e.PropertyName,
            Error = e.ErrorMessage
        });
    }
}