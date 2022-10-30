using Currency.Exchange;
using Currency.Exchange.Data;
using Currency.Exchange.Data.DbContext;
using Currency.Exchange.External.Client;
using Currency.Exchange.Host;
using Currency.Exchange.Profiles;
using Currency.Exchange.Services;
using Currency.Exchange.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Configure IPStack configuration.
builder.Services.Configure<FixerOptions>(builder.Configuration.GetSection(FixerOptions.SectionName));

// Register dbContext.
builder.Services.AddDbContext<exchangeContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// Refit client.
builder.Services.AddRefitClient<IFixerClient>().ConfigureHttpClient(client =>
{
    client.BaseAddress =
        new Uri(builder.Configuration.GetSection(FixerOptions.SectionName).Get<FixerOptions>().BaseUrl);
    client.DefaultRequestHeaders.Add(FixerOptions.ApiKeyHeaderName,
        builder.Configuration.GetSection(FixerOptions.SectionName).Get<FixerOptions>().ApiKey);
});

// Register repositories.
builder.Services.AddScoped<ICurrencyExchangeRepository, CurrencyExchangeRepository>();

// Register services.
builder.Services.AddScoped<IExchangeService, ExchangeService>();
builder.Services.AddScoped<ISymbolService, SymbolService>();
builder.Services.AddScoped<ICacheStore, DistributedCacheStore>();

// Register validators.
builder.Services.AddValidatorsFromAssemblyContaining<TradeRequestValidator>();

// Register mapping profiles.
builder.Services.AddAutoMapper(typeof(MappingProfiles));

// Register Redis distributed cache.
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = Constants.RedisInstanceName;
});

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();