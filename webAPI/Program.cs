using Azure.AI.OpenAI;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddOpenAIClient(builder.Configuration.GetSection("OpenAI"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/chat", async (OpenAIClient openAI, CancellationToken cancellationToken) =>
    {
        var response = await openAI.GetCompletionsAsync(new CompletionsOptions()
        {
            DeploymentName = "text-davinci-003", // assumes a matching model deployment or model name
            Prompts = {"Hello, world!"},
        }, cancellationToken);
        return response.Value.Choices;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);
}
