using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YQTrack.Log;

namespace YQTrack.Core.Backend.Admin.Data.MediatR
{
    public class OperationLogEventHandler : INotificationHandler<OperationLogEvent>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OperationLogEventHandler> _logger;

        public OperationLogEventHandler(IServiceProvider serviceProvider,
            ILogger<OperationLogEventHandler> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task Handle(OperationLogEvent notification, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                using (var serviceScope = _serviceProvider.CreateScope())
                {
                    var adminDbContext = serviceScope.ServiceProvider.GetRequiredService<AdminDbContext>();
                    await adminDbContext.OperationLog.AddAsync(notification, cancellationToken);
                    await adminDbContext.SaveChangesAsync(cancellationToken);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "OperationLogEventHandler处理异常");
                LogHelper.LogObj(new LogDefinition(Log.LogLevel.Error, "OperationLogEventHandler处理异常"), e, notification);
            }
        }
    }
}