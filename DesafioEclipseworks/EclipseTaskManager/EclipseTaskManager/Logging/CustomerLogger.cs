using Microsoft.Extensions.Configuration;
namespace EclipseTaskManager.Logging;

public class CustomerLogger: ILogger
{
    private readonly string loggerName;
    private readonly CustomLoggerProviderConfiguration loggerConfig;
    private readonly string logDir;
    private readonly string logFile;
    private readonly string logPath;

    public CustomerLogger(string name, CustomLoggerProviderConfiguration config, IConfiguration configuration)
    {
        loggerName = name;
        loggerConfig = config;

        // Ler valores do appsettings.json
        logDir = configuration["Logging:LogDir"] ?? "Logs";
        logFile = configuration["Logging:LogFile"] ?? "app.log";


        if (!Directory.Exists(logDir))
        {
            Directory.CreateDirectory(logDir);
        }
        logPath = Path.Combine(logDir, logFile);
    }

    public CustomerLogger(string name, CustomLoggerProviderConfiguration config)
    {
        loggerName = name;
        loggerConfig = config;

        // Ler valores do appsettings.json
        logDir =  "Logs";
        logFile = "app.log";


        if (!Directory.Exists(logDir))
        {
            Directory.CreateDirectory(logDir);
        }
        logPath = Path.Combine(logDir, logFile);
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == loggerConfig.LogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string message = $"{DateTime.UtcNow}\t{logLevel.ToString()}\t{eventId.Id}\t{formatter(state, exception)}";
        WriteOnFile(message);
    }

    private void WriteOnFile(string msg) 
    {
        using (StreamWriter streamWriter = new StreamWriter(logPath, true))
        {
            try
            {
                streamWriter.WriteLine(msg);
                streamWriter.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
