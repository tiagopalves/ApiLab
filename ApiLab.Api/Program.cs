using ApiLab.Api.Common.IoC;
using ApiLab.CrossCutting.LogManager.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Scalar.AspNetCore;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    //Configuração do Serilog
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();
    Log.Information("Iniciando a aplicação...");

    builder.Host.UseSerilog();

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    //builder.Services.AddSwaggerGen(); //Forma antiga de usar o swagger

    builder.Services.AddHealthChecks();
        //.AddRedis(""); //TODO: Configurar Redis
    builder.Services.AddHealthChecksUI(options =>
        {
            options.SetEvaluationTimeInSeconds(30);
            options.MaximumHistoryEntriesPerEndpoint(10);
            options.AddHealthCheckEndpoint("Api Lab", "/health");
        })
        .AddInMemoryStorage();
    

    //Meus serviços
    builder.Services.AddLoggingService();
    builder.Services.AddServices();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        //Forma antiga de usar o swagger
        //app.UseSwagger();
        //app.UseSwaggerUI();

        //Forma de usar o swagger com o OpenApi do .Net Core 9
        //app.MapOpenApi();
        //app.UseSwaggerUI(options =>
        //{
        //    options.SwaggerEndpoint("/openapi/v1.json", "Api Lab");
        //});

        //Nova forma usando Scalar
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options
                .WithTitle("Lab Api")
                .WithTheme(ScalarTheme.BluePlanet)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.UseHealthChecks("/health", new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
    app.UseHealthChecksUI();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Ocorreu um erro ao iniciar a aplicação!");
}
finally
{
    Log.Information("Encerrando a aplicação...");
    Log.CloseAndFlush();
};