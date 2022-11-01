namespace Currency.Exchange.Exceptions;

public class FixerUnsuccessfulException : Exception
{
    public FixerUnsuccessfulException(string message): base(message)
    {
        
    }   
}