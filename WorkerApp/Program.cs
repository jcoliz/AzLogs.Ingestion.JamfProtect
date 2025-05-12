using Azure.Identity;
using Microsoft.Extensions.Azure;
using WorkerApp;
using WorkerApp.Options;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddTomlFile("config.toml", optional: true, reloadOnChange: true);

builder.Services.Configure<LogIngestionOptions>(
    builder.Configuration.GetSection(LogIngestionOptions.Section)
);
builder.Services.Configure<IdentityOptions>(
    builder.Configuration.GetSection(LogIngestionOptions.Section)
);

builder.Services.AddAzureClients(clientBuilder =>
{
    // Create a client secret credential using supplied credentials
    IdentityOptions identityOptions = new();
    builder.Configuration.Bind(IdentityOptions.Section, identityOptions);
    var token = new ClientSecretCredential(identityOptions.TenantId.ToString(), identityOptions.AppId.ToString(), identityOptions.AppSecret);

    // Add a log ingestion client, using endpoint from configuration
    LogIngestionOptions logOptions = new();
    builder.Configuration.Bind(LogIngestionOptions.Section, logOptions);
    clientBuilder.AddLogsIngestionClient(logOptions.EndpointUri);

    // Add the desired Azure credential to the client
    clientBuilder.UseCredential(token);
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
