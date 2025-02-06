using Apilab.Application.Validators;
using ApiLab.Api.Common.ExceptionHandlers;
using ApiLab.Api.Common.IoC;
using ApiLab.CrossCutting.Configurations;
using ApiLab.CrossCutting.LogManager.Extensions;
using ApiLab.CrossCutting.Resources;
using FluentValidation;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using System.Text;

try
{
    var builder = WebApplication.CreateBuilder(args);

    #region AppSettings Configuration

    builder.Services.Configure<CommonConfiguration>(builder.Configuration.GetSection(nameof(CommonConfiguration)));
    builder.Services.Configure<HealthChecksConfiguration>(builder.Configuration.GetSection(nameof(HealthChecksConfiguration)));
    builder.Services.Configure<RedisConfiguration>(builder.Configuration.GetSection(nameof(RedisConfiguration)));

    var commonConfiguration = builder.Configuration.GetRequiredSection(nameof(CommonConfiguration)).Get<CommonConfiguration>();
    var healthChecksConfiguration = builder.Configuration.GetRequiredSection(nameof(HealthChecksConfiguration)).Get<HealthChecksConfiguration>();
    var redisConfiguration = builder.Configuration.GetRequiredSection(nameof(RedisConfiguration)).Get<RedisConfiguration>();

    #endregion

    #region Services Configuration

    //Serilog
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

    Log.Information(FriendlyMessages.AppStarting);

    builder.Host.UseSerilog();

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    //builder.Services.AddSwaggerGen(); //Forma antiga de usar o swagger

    //JwtBearer Authentication
    if (commonConfiguration is not null && !string.IsNullOrEmpty(commonConfiguration.ApiSecurityKey))
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(commonConfiguration.ApiSecurityKey))
                };
            });
        builder.Services.AddAuthorization();
    }

    //HealthChecks
    builder.Services.AddHealthChecks()
        .AddRedis(redisConfiguration?.ConnectionString ?? string.Empty, redisConfiguration?.HealthCheckName);
    builder.Services.AddHealthChecksUI(options =>
        {
            if (healthChecksConfiguration is not null && commonConfiguration is not null)
            {
                options.SetEvaluationTimeInSeconds(healthChecksConfiguration.EvaluationTimeInSeconds);
                options.MaximumHistoryEntriesPerEndpoint(healthChecksConfiguration.MaximumHistoryEntriesPerEndpoint);
                options.AddHealthCheckEndpoint(commonConfiguration.AppName, healthChecksConfiguration.HealthCheckEndpointUri);
            }
        })
        .AddInMemoryStorage();

    //ProblemDetails
    builder.Services.AddProblemDetails(options =>
    {
        options.CustomizeProblemDetails = context =>
        {
            var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;

            context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
            context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
            context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
        };
    });

    builder.Services.AddExceptionHandler<GeneralExceptionHandler>();

    builder.Services.AddValidatorsFromAssemblyContaining<ClienteValidator>();

    //Meus Serviços
    builder.Services.AddLoggingService();
    builder.Services.AddServices();

    #endregion

    #region App Configuration

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
                .WithTitle(commonConfiguration?.AppName ?? string.Empty)
                .WithTheme(ScalarTheme.BluePlanet)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseHealthChecks(healthChecksConfiguration?.HealthCheckEndpointUri, new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
    app.UseHealthChecksUI();
    app.UseExceptionHandler();

    app.MapControllers();
    app.Run();

    #endregion
}
catch (Exception ex)
{
    Log.Fatal(ex, FriendlyMessages.AppStartingError);
}
finally
{
    Log.Information(FriendlyMessages.AppEnding);
    Log.CloseAndFlush();
};