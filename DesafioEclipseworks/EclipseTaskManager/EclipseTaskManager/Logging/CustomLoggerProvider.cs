using System.Collections.Concurrent;

namespace EclipseTaskManager.Logging
{
    public class CustomLoggerProvider: ILoggerProvider
    {
        readonly CustomLoggerProviderConfiguration loggerConfig;

        readonly ConcurrentDictionary<string, CustomerLogger> loggers = new ConcurrentDictionary<string, CustomerLogger> ();

        public CustomLoggerProvider(CustomLoggerProviderConfiguration config)
        {
            loggerConfig = config;
        }

        public ILogger CreateLogger(string categoryName)
        {
            // TODO refatorar passando arquivo de configuração
            // return loggers.GetOrAdd(categoryName, nameof => new CustomerLogger(nameof, loggerConfig));
            return loggers.GetOrAdd(categoryName, nameof => new CustomerLogger(nameof, loggerConfig));
        }

        public void Dispose() 
        {
            loggers.Clear();
        }

    }
}
