using Currency.Exchange;
using Currency.Exchange.External.Client;
using Currency.Exchange.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.Builders;

public class SymbolServiceBuilder
{
    private Mock<ICacheStore> _cacheStoreMock;
    private Mock<IFixerClient> _fixerClientMock;
    private Mock<ILogger<SymbolService>> _loggerMock;

    public SymbolServiceBuilder WithCacheStore(Mock<ICacheStore>cacheStoreMock)
    {
        _cacheStoreMock = cacheStoreMock;
        return this;
    }

    public SymbolServiceBuilder WithFixerClient(Mock<IFixerClient> fixerClientMock)
    {
        _fixerClientMock = fixerClientMock;
        return this;
    }
    
    public SymbolServiceBuilder WithFixerLogger(Mock<ILogger<SymbolService>> loggerMock)
    {
        _loggerMock = loggerMock;
        return this;
    }
    
    public ISymbolService Build()
    {
        return new SymbolService(_cacheStoreMock.Object, _fixerClientMock.Object, _loggerMock.Object);
    }
}