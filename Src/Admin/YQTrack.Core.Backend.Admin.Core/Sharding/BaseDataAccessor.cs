using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YQTrack.Core.Backend.Admin.Core.Sharding
{
    /// <summary>
    /// .NET EF Core框架帮助基础类
    /// </summary>
    internal class BaseDataAccessor<TContext>
    {
        private DbContext context;
        private static Type contextType = null;


        /// <summary>
        /// 使用该类前必须调用此方法指明DbContext类型，若传入的类型不是继承于DbContext，将冒ArgumentException
        /// </summary>
        /// <param name="t">Context类型</param>
        /// <exception cref="ArgumentException">context 类型设置错误</exception>
        public static void SetContextType(Type t)
        {
            if (t.BaseType != typeof(DbContext))
            {
                throw new ArgumentException("ContextType must inherits from DbContext");
            }
            else
            {
                contextType = t;
            }
        }

        /// <summary>
        /// 创建Helper对象，调用前请确保已设定Context类型，若未设定DbContext类型则会冒ArgumentNullException
        /// </summary>
        /// <exception cref="ArgumentNullException">context 未设置</exception>
        public BaseDataAccessor()
        {
            if (typeof(TContext).BaseType != typeof(DbContext))
            {
                throw new ArgumentException("ContextType must inherits from DbContext");
            }
            else
            {
                contextType = typeof(TContext);
            }
            context = (DbContext)Activator.CreateInstance(contextType);
        }

        public BaseDataAccessor(ICollection<TableMappingRule> rules)
        {
            if (contextType == null)
            {
                throw new ArgumentNullException("You have not set context type");
            }
            context = (DbContext)Activator.CreateInstance(contextType, rules);
        }

        /// <summary>
        /// 是否已关闭
        /// </summary>
        /// <returns></returns>
        public bool IsClose()
        {
            return context == null;
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            if (context != null)
            {
                context.Dispose();
                context = null;
            }
        }

        /// <summary>
        /// 获取查询数据源（不跟踪）
        /// </summary>
        /// <typeparam name="T">查询的类</typeparam>
        /// <returns></returns>
        public IQueryable<T> GetQueryable<T>() where T : class
        {
            return context.Set<T>().AsNoTracking();
        }

        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public DbSet<T> GetDbSet<T>() where T : class
        {
            return context.Set<T>();
        }

        /// <summary>
        /// 获取数据上下文
        /// </summary>
        /// <returns></returns>
        public DbContext GetDbContext()
        {
            return context;
        }

        /// <summary>
        /// 提交对数据进行的处理，如无处理返回-1
        /// </summary>
        /// <returns></returns>
        public int Save()
        {
            if (context != null)
                return context.SaveChanges();
            else
                return -1;
        }

        /// <summary>
        /// 提交对数据进行的处理，如无处理返回-1
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveAsync()
        {
            if (context != null)
                return await context.SaveChangesAsync();
            else
                return -1;
        }

        /// <summary>
        /// 异步获取一个事务对象
        /// </summary>
        /// <returns></returns>
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            var ret = await context.Database.BeginTransactionAsync();
            return ret;
        }

        /// <summary>
        /// 获取一个事务对象
        /// </summary>
        /// <returns></returns>
        public IDbContextTransaction BeginTransaction()
        {
            return context.Database.BeginTransaction();
        }
    }
}
