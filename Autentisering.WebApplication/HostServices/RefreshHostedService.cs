using Authorization.WebBFFApplication.AppServices.Features.IdentityAndAccess;

namespace Authorization.WebBFFApplication.HostServices
{
    public class RefreshHostedService : BackgroundService
    {
        private readonly IServiceProvider services;
        private readonly ILogger<RefreshHostedService> logger;

        public RefreshHostedService(IServiceProvider services, ILogger<RefreshHostedService> logger)
        {
            this.services = services;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Starting ExecuteAsync");

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = services.CreateScope())
                {
                    var tokenFreshService = scope.ServiceProvider.GetService<TokenFreshService>();
                    try
                    {
                        var success = await DoWorkScopedWork(tokenFreshService);
                    }
                    catch (Exception exp)
                    {
                        logger.LogError(exp, "Running ExecuteAsync {message}", exp.Message);
                    }

                    await Task.Delay(60000);
                }
            }
        }

        async Task<Boolean> DoWorkScopedWork(TokenFreshService tokenFreshService)
        {
             (bool success,string messsage)=   await tokenFreshService.RefreshToken();
            logger.LogInformation("RefreshToken completed {success} : {messsage}", success, messsage);
            return success;
        }

    }
}
