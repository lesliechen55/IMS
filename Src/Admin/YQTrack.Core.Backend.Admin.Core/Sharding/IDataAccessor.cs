using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YQTrack.Core.Backend.Admin.Core.Sharding
{
    /// <summary>
    /// 数据接入层接口
    /// </summary>
    public interface IDataAccessor<TContext>
    {
        /// <summary>
        /// 更换数据库, 数据表映射使用默认映射规则
        /// </summary>
        /// <param name="connStr">新的数据库连接字符串</param>
        void ChangeDataBase(string connStr);

        /// <summary>
        /// 根据条件改变传入类型的映射数据表(此操作会导致当前操作的context释放掉，调用前需确保context的内容已保存)
        /// </summary>
        /// <typeparam name="T">条件类型</typeparam>
        /// <param name="type">要改变映射的实体类型</param>
        /// <param name="condition">改变条件</param>
        /// <exception cref="ArgumentException">type类型不支持</exception>
        /// <returns>改变后的数据表映射</returns>
        TableAccessMapping ChangeMappingTable(Type type, ITableMappable mapper, object condition);

        /// <summary>
        /// 更换数据库, 数据表映射使用传入的映射规则
        /// </summary>
        /// <param name="connStr">新的数据库连接字符串</param>
        /// <param name="rules"></param>
        /// <exception cref="ArgumentException">type类型不支持</exception>
        void ChangeDataBase(string connStr, List<TableMappingRule> rules);

        /// <summary>
        /// 是否已关闭
        /// </summary>
        /// <returns></returns>
        bool IsClose();

        /// <summary>
        /// 关闭连接
        /// </summary>
        void Close();

        /// <summary>
        /// 获取查询数据源（不跟踪）
        /// </summary>
        /// <typeparam name="T">查询的类</typeparam>
        /// <returns></returns>
        IQueryable<T> GetQueryable<T>() where T : class;

        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        DbSet<T> GetDbSet<T>() where T : class;

        /// <summary>
        /// 提交对数据进行的处理，如无处理返回-1
        /// </summary>
        /// <returns></returns>
        int Save();

        /// <summary>
        /// 提交对数据进行的处理，如无处理返回-1
        /// </summary>
        /// <returns></returns>
        Task<int> SaveAsync();

        /// <summary>
        /// 异步获取一个事务对象
        /// </summary>
        /// <returns></returns>
        Task<IDbContextTransaction> BeginTransactionAsync();

        /// <summary>
        /// 获取一个事务对象
        /// </summary>
        /// <returns></returns>
        IDbContextTransaction BeginTransaction();

        /// <summary>
        /// 获取数据上下文
        /// </summary>
        /// <returns></returns>
        DbContext GetDbContext();
    }
}