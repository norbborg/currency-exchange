using System.Net;
using Currency.Exchange;
using Currency.Exchange.External.Client;
using Currency.Exchange.External.Client.Models;
using Currency.Exchange.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Refit;
using UnitTests.Builders;
using Xunit;

namespace UnitTests.Tests;

[Trait("Category", "Unit")]
public class SymbolServiceTests
{
    private Mock<ICacheStore> _cacheStoreMock;
    private Mock<IFixerClient> _fixerClientMock;
    private Mock<ILogger<SymbolService>> _loggerMock;
    private const string CacheKey = "Symbols";

    private ISymbolService _sut;

    private readonly Dictionary<string, string> _mockedCachedSymbols = new()
    {
        { "EUR", "Euro" }
    };

    public SymbolServiceTests()
    {
        _cacheStoreMock = new Mock<ICacheStore>();
        _fixerClientMock = new Mock<IFixerClient>();
        _loggerMock = new Mock<ILogger<SymbolService>>();
    }

    [Fact]
    public async void GetSymbols_ExistInCache_ReturnsFromCache()
    {
        // Arrange
        _cacheStoreMock.Setup(c => c.Get<IDictionary<string, string>>(It.IsAny<string>()))
            .Returns(_mockedCachedSymbols);

        _sut = new SymbolServiceBuilder()
            .WithCacheStore(_cacheStoreMock)
            .WithFixerClient(_fixerClientMock)
            .WithFixerLogger(_loggerMock)
            .Build();

        // Act
        var result = await _sut.GetSymbolsAsync();

        // Assert
        Assert.NotEmpty(result);
        _cacheStoreMock.Verify(c => c.Get<IDictionary<string, string>>(CacheKey), Times.Once);
        
        VerifyNoOtherCalls();
    }

    [Fact]
    public async void GetSymbols_NotInCache_CallsThirdParty()
    {
        // Arrange
        _fixerClientMock.Setup(c => c.GetSymbolsAsync()).ReturnsAsync(new SymbolsResponse
        {
            Success = true,
            Symbols = _mockedCachedSymbols
        });

        _sut = new SymbolServiceBuilder()
            .WithCacheStore(_cacheStoreMock)
            .WithFixerClient(_fixerClientMock)
            .WithFixerLogger(_loggerMock)
            .Build();

        // Act
        var result = await _sut.GetSymbolsAsync();

        // Assert
        Assert.NotEmpty(result);
        _cacheStoreMock.Verify(c => c.Get<IDictionary<string, string>>(CacheKey), Times.Once);
        _fixerClientMock.Verify(c => c.GetSymbolsAsync(), Times.Once);
        _cacheStoreMock.Verify(c => c.Add(CacheKey, It.IsAny<IDictionary<string, string>>(), It.IsAny<TimeSpan>()),
            Times.Once);
        
        VerifyNoOtherCalls();
    }

    [Fact]
    public async void GetSymbols_ThirdPartyNotAvailable_ThrowsFixerUnsuccessfulException()
    {
        // Arrange
        var exception = await ApiException.Create(new HttpRequestMessage(HttpMethod.Get, "uri"), HttpMethod.Get,
            new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = It.IsAny<HttpContent>()
            }, null);
        _fixerClientMock.Setup(c => c.GetSymbolsAsync()).ThrowsAsync(exception);

        _sut = new SymbolServiceBuilder()
            .WithCacheStore(_cacheStoreMock)
            .WithFixerClient(_fixerClientMock)
            .WithFixerLogger(_loggerMock)
            .Build();

        // Act
        await Assert.ThrowsAsync<ApiException>(() => _sut.GetSymbolsAsync());

        // Assert
        _cacheStoreMock.Verify(c => c.Get<IDictionary<string, string>>(CacheKey), Times.Once);
        _fixerClientMock.Verify(c => c.GetSymbolsAsync(), Times.Once);
        _loggerMock.Verify(c => c.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()
        ), Times.Once);
        
        VerifyNoOtherCalls();
    }

    private void VerifyNoOtherCalls()
    {
        _cacheStoreMock.VerifyNoOtherCalls();
        _fixerClientMock.VerifyNoOtherCalls();
        _loggerMock.VerifyNoOtherCalls();
    }
}