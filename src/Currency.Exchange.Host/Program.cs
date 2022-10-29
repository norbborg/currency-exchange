using Currency.Exchange.External.Client;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Configure IPStack configuration.
builder.Services.Configure<FixerOptions>(builder.Configuration.GetSection(FixerOptions.SectionName));

// Refit client.
builder.Services.AddRefitClient<IFixerClient>().ConfigureHttpClient(client =>
{
    client.BaseAddress =
        new Uri(builder.Configuration.GetSection(FixerOptions.SectionName).Get<FixerOptions>().BaseUrl);
    client.DefaultRequestHeaders.Add(FixerOptions.ApiKeyHeaderName,
        builder.Configuration.GetSection(FixerOptions.SectionName).Get<FixerOptions>().ApiKey);
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