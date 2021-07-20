using MediatR;
using YQTrack.Core.Backend.Admin.Data.Entity;

namespace YQTrack.Core.Backend.Admin.Data.MediatR
{
    public class OperationLogEvent : OperationLog, INotification
    {

    }
}