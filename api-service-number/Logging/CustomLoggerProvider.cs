using System.Collections.Concurrent;
using APISenha.Logging;

namespace api_service_number.Logging;

public class CustomLoggerProvider : ILoggerProvider
{
    readonly CustomLoggerProviderConfiguration loggerConfig;
    
    readonly ConcurrentDictionary<string, CustomLogger> loggers = new ConcurrentDictionary<string, CustomLogger>();

    public CustomLoggerProvider(CustomLoggerProviderConfiguration loggerConfig)
    {
        this.loggerConfig = loggerConfig;
    }

    public ILogger CreateLogger(string logCategory)
    {
        return loggers.GetOrAdd(logCategory, name => new CustomLogger(name, loggerConfig));//Valida se já existe um log
    }


    public void Dispose()
    {
        loggers.Clear();
    }
}

