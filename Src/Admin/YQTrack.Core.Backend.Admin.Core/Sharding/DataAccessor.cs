using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YQTrack.Core.Backend.Admin.Core.Sharding
{
    /// <summary>
    /// .NET EF Core框架帮助类
    /// </summary>
    public class DataAccessor<TContext> : IDataAccessor<TContext>, IMappingMutable, ITransientService
    {
        /// <summary>
        /// 创建Helper对象，调用前请确保已设定Context类型，若未设定DbContext类型则会冒ArgumentNullException
        /// </summary>
        /// <exception cref="ArgumentNullException">context 未设置</exception>
        public DataAccessor()
        {
            BaseAccessor = new BaseDataAccessor<TContext>();
        }

        /// <summary>
        /// 更改映射时的互斥锁
        /// </summary>
        private static object LockObj = new object();

        private BaseDataAccessor<TContext> BaseAccessor;

        /// <summary>
        /// 使用该类前必须调用此方法指明DbContext类型，若传入的类型不是继承于DbContext，将冒ArgumentException
        /// </summary>
        /// <param name="t">Context类型</param>
        /// <exception cref="ArgumentException">context 类型设置错误</exception>
        public static void SetContextType(Type t)
        {
            BaseDataAccessor<TContext>.SetContextType(t);
        }

        /// <summary>
        /// 更换数据库, 数据表映射使用默认映射规则
        /// </summary>
        /// <param name="connStr">新的数据库连接字符串</param>
        public void ChangeDataBase(string connStr)
        {
            // close
            var accessor = BaseAccessor;
            if (!string.IsNullOrWhiteSpace(connStr) && accessor.GetDbContext().Database.GetDbConnection().ConnectionString != connStr)
            {
                if (!accessor.IsClose())
                {
                    accessor.Close();
                }
                // new base accessor
                accessor = new BaseDataAccessor<TContext>();
                accessor.GetDbContext().Database.GetDbConnection().ConnectionString = connStr;
            }
            BaseAccessor = accessor;
        }

        /// <summary>
        /// 更换数据库, 数据表映射使用传入的映射规则
        /// </summary>
        /// <param name="connStr">新的数据库连接字符串</param>
        /// <param name="rules">映射规则</param>
        /// <exception cref="ArgumentException">type类型不支持</exception>
        public void ChangeDataBase(string connStr, List<TableMappingRule> rules)
        {
            var accessor = BaseAccessor;
            // close
            if (!accessor.IsClose())
            {
                accessor.Close();
            }
            lock (LockObj)
            {
                // new base accessor
                accessor = new BaseDataAccessor<TContext>(rules);
                // notity new mapping
                DynamicModelCacheKeyFactory.ChangeTableMapping();
                if (!string.IsNullOrWhiteSpace(connStr) && accessor.GetDbContext().Database.GetDbConnection().ConnectionString != connStr)
                {
                    accessor.GetDbContext().Database.GetDbConnection().ConnectionString = connStr;
                }
                BaseAccessor = accessor;
            }
        }

        /// <summary>
        /// 根据条件改变多个传入类型的映射数据表(此操作会导致当前操作的context释放掉，调用前需确保context的内容已保存)
        /// </summary>
        /// <param name="rules">映射规则</param>
        /// <exception cref="ArgumentException">type类型不支持</exception>
        /// <returns>改变后的数据表映射</returns>
        public List<TableAccessMapping> ChangeMappingTables(List<TableMappingRule> rules)
        {
            List<TableAccessMapping> ret = new List<TableAccessMapping>();
            if (rules != null)
            {
                // close old accessor
                var accessor = BaseAccessor;
                if (!accessor.IsClose())
                {
                    accessor.Close();
                }
                lock (LockObj)
                {
                    // new accessor
                    accessor = new BaseDataAccessor<TContext>(rules);
                    // notity new mapping
                    DynamicModelCacheKeyFactory.ChangeTableMapping();
                    this.BaseAccessor = accessor;
                }
                foreach (var rule in rules)
                {
                    var mapping = GetTableName(rule.MappingType, accessor);
                    ret.Add(mapping);
                }
            }

            return ret;
        }

        /// <summary>
        /// 根据条件改变传入类型的映射数据表(此操作会导致当前操作的context释放掉，调用前需确保context的内容已保存)
        /// </summary>
        /// <typeparam name="T">条件类型</typeparam>
        /// <param name="type">要改变映射的实体类型</param>
        /// <param name="condition">改变条件</param>
        /// <exception cref="ArgumentException">type类型不支持</exception>
        /// <returns>改变后的数据表映射</returns>
        public TableAccessMapping ChangeMappingTable(Type type, ITableMappable mapper, object condition)
        {
            TableMappingRule rule = default(TableMappingRule);
            rule.MappingType = type;
            rule.Mapper = mapper;
            rule.Condition = condition;

            List<TableMappingRule> param = new List<TableMappingRule> { rule };
            var result = ChangeMappingTables(param);
            return result[0];
        }

        /// <summary>
        /// 获取所有实体类的数据表映射结构体
        /// </summary>
        /// <returns>映射关系集合</returns>
        public List<TableAccessMapping> GetTableNames()
        {
            BaseDataAccessor<TContext> helper = BaseAccessor;
            var context = helper.GetDbContext();

            List<TableAccessMapping> ret = new List<TableAccessMapping>();
            var models = context.Model.GetEntityTypes();
            foreach (var model in models)
            {
                string table = model.Relational().TableName;
                Type type = model.ClrType;
                TableAccessMapping mapping = new TableAccessMapping(type, table);
                ret.Add(mapping);
            }

            return ret;
        }

        /// <summary>
        /// 获取指定实体类的数据表映射结构体
        /// </summary>
        /// <param name="mappingType">实体类性</param>
        /// <returns>传入实体类的映射关系</returns>
        public TableAccessMapping GetTableName(Type mappingType)
        {
            BaseDataAccessor<TContext> helper = BaseAccessor;
            var context = helper.GetDbContext();
            var model = context.Model.FindEntityType(mappingType);

            if (model != null)
            {
                string table = model.Relational().TableName;
                return new TableAccessMapping(mappingType, table);
            }
            else
            {
                throw new ArgumentException("Mapping type not found");
            }
        }
        private TableAccessMapping GetTableName(Type mappingType, BaseDataAccessor<TContext> helper)
        {
            var context = helper.GetDbContext();
            var model = context.Model.FindEntityType(mappingType);

            if (model != null)
            {
                string table = model.Relational().TableName;
                return new TableAccessMapping(mappingType, table);
            }
            else
            {
                throw new ArgumentException("Mapping type not found");
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            BaseAccessor?.Close();
        }

        /// <summary>
        /// 是否已关闭
        /// </summary>
        /// <returns></returns>
        public bool IsClose()
        {
            return BaseAccessor.IsClose();
        }

        /// <summary>
        /// 获取查询数据源（不跟踪）
        /// </summary>
        /// <typeparam name="T">查询的类</typeparam>
        /// <returns></returns>
        public IQueryable<T> GetQueryable<T>() where T : class
        {
            return BaseAccessor?.GetQueryable<T>();
        }

        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public DbSet<T> GetDbSet<T>() where T : class
        {
            return BaseAccessor?.GetDbSet<T>();
        }

        /// <summary>
        /// 获取数据上下文
        /// </summary>
        /// <returns></returns>
        public DbContext GetDbContext()
        {
            return BaseAccessor?.GetDbContext();
        }

        /// <summary>
        /// 提交对数据进行的处理，如无处理返回-1
        /// </summary>
        /// <returns></returns>
        public int Save()
        {
            return BaseAccessor.Save();
        }

        /// <summary>
        /// 提交对数据进行的处理，如无处理返回-1
        /// </summary>
        /// <returns></returns>
        public Task<int> SaveAsync()
        {
            return BaseAccessor.SaveAsync();
        }

        /// <summary>
        /// 获取一个事务对象
        /// </summary>
        /// <returns></returns>
        public IDbContextTransaction BeginTransaction()
        {
            return BaseAccessor?.BeginTransaction();
        }

        /// <summary>
        /// 异步获取一个事务对象
        /// </summary>
        /// <returns></returns>
        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return BaseAccessor?.BeginTransactionAsync();
        }

    }
}
