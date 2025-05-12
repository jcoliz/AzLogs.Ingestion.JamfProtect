using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace WorkerApp;

public partial class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                logOk();
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

    [LoggerMessage(Level = LogLevel.Information, Message = "{Location}: OK", EventId = 1000)]
    public partial void logOk([CallerMemberName] string? location = null);

    [LoggerMessage(Level = LogLevel.Error, Message = "{Location}: Failed", EventId = 1008)]
    public partial void logFail(Exception ex,[CallerMemberName] string? location = null);

    [LoggerMessage(Level = LogLevel.Critical, Message = "{Location}: Critical failure", EventId = 1009)]
    public partial void logCritical(Exception ex,[CallerMemberName] string? location = null);
}
