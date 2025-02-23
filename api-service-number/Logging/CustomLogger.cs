using api_service_number.Logging;

namespace APISenha.Logging;

public class CustomLogger : ILogger
{
    readonly string logCategory;
    private readonly CustomLoggerProviderConfiguration loggerConfig;

    public CustomLogger(string logCategory, CustomLoggerProviderConfiguration loggerConfig)
    {
        this.logCategory = logCategory;
        this.loggerConfig = loggerConfig;
    }

    public bool IsEnabled(LogLevel level)
    {
        return loggerConfig.LogLevel >= level;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return null;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] ({logCategory}) {formatter(state, exception)}";

        if (exception != null)
        {
            logMessage += $" | Exception: {exception.Message}";
        }

        WriteTextFile(logMessage);
    }

    private void WriteTextFile(string message)
    {
        //GetCurrentDirectory padroniza pegando a pasta que a aplicação está rodando
        string logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs"); 
        
        string filePath = Path.Combine(logDirectory, "CustomLogger.txt");

        try
        {
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(message);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao escrever no log: {ex.Message}");
        }
    }
}