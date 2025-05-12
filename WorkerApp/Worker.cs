using System.Runtime.CompilerServices;
using Azure.Monitor.Ingestion;
using Microsoft.Extensions.Options;
using WorkerApp.Api;
using WorkerApp.Options;

namespace WorkerApp;

public partial class Worker(
    LogsIngestionClient logsClient,
    IOptions<LogIngestionOptions> logOptions,
    ILogger<Worker> logger
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var logs = GenerateTelemetryV1();
                    await UploadToLogsAsync(logs,stoppingToken);

                    logOk();
                }
                catch( Exception ex )
                {
                    logFail(ex);
                }
                await Task.Delay(1000, stoppingToken);
            }            
        }
        catch (TaskCanceledException)
        {
            // Normal exit
        }
        catch (Exception ex)
        {
            logCritical(ex);
        }
    }

    protected ICollection<CustomJamfprotecttelemetryv1> GenerateTelemetryV1()
    {
        var result = new List<CustomJamfprotecttelemetryv1>()
        {
            new()
            {
                Host_info = new{ osversion = "osversion", host_name = "host_name", host_uuid = Guid.NewGuid().ToString() }
            }
        };

        return result;
    }

    public async Task UploadToLogsAsync(IEnumerable<object> dataPoints, CancellationToken stoppingToken)
    {
        try
        {
            var response = await logsClient.UploadAsync
            (
                ruleId: logOptions.Value.DcrImmutableId, 
                streamName: logOptions.Value.Stream,
                logs: dataPoints,
                cancellationToken: stoppingToken
            )
            .ConfigureAwait(false);

            switch (response?.IsError)
            {
                case null:
                    logSendNoResponse();
                    break;

                case true:
                    logSendFail(response.Status);
                    break;

                default:
                    logOk();
                    break;
            }
        }
        catch (Exception ex)
        {
            logFail(ex);
        }
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "{Location}: OK", EventId = 1000)]
    public partial void logOk([CallerMemberName] string? location = null);

    [LoggerMessage(Level = LogLevel.Error, Message = "{Location}: Failed", EventId = 1008)]
    public partial void logFail(Exception ex,[CallerMemberName] string? location = null);

    [LoggerMessage(Level = LogLevel.Critical, Message = "{Location}: Critical failure", EventId = 1009)]
    public partial void logCritical(Exception ex,[CallerMemberName] string? location = null);

    [LoggerMessage(Level = LogLevel.Error, Message = "{Location}: Send failed, returned no response", EventId = 1107)]
    public partial void logSendNoResponse([CallerMemberName] string? location = null);

    [LoggerMessage(Level = LogLevel.Error, Message = "{Location}: Send failed {Status}", EventId = 1108)]
    public partial void logSendFail(int Status, [CallerMemberName] string? location = null);
}
