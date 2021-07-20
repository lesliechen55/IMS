using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading;

namespace YQTrack.Core.Backend.Admin.Core.Sharding
{
    public class DynamicModelCacheKeyFactory : IModelCacheKeyFactory
    {
        private static int m_Marker = 0;

        public static void ChangeTableMapping()
        {
            Interlocked.Increment(ref m_Marker);
        }

        public object Create(DbContext context)
        {
            return (context.GetType(), m_Marker);
        }
    }
}
