using Scalar.AspNetCore;
using Serilog;

//TODO: Verificar se � a melhor forma de configurar o Serilog
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    Log.Information("Iniciando a aplica��o...");

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    //builder.Services.AddSwaggerGen(); //Forma antiga de usar o swagger

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
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Ocorreu um erro ao iniciar a aplica��o!");
}
finally
{
    Log.Information("Encerrando a aplica��o...");
    Log.CloseAndFlush();
};