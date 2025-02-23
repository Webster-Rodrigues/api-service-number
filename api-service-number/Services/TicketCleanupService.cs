using api_service_number.Services;

public class TicketCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<TicketCleanupService> _logger;

    public TicketCleanupService(IServiceScopeFactory serviceScopeFactory, ILogger<TicketCleanupService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Verificando tickets expirados...");
            
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var ticketService = scope.ServiceProvider.GetRequiredService<TicketService>();
                await ticketService.CancelTicket();
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); // Verifica a cada 30 segundos. Tempo definido só pra fazer testes 
        }
    }
}