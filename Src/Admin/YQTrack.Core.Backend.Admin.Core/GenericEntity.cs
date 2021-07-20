namespace YQTrack.Core.Backend.Admin.Core
{
    /// <summary>
    /// 实体泛型类
    /// </summary>
    /// <typeparam name="TPrimaryKey">实体数据库主键</typeparam>
    public abstract class GenericEntity<TPrimaryKey>
    {
        /// <summary>
        /// 主键
        /// </summary>
        public TPrimaryKey FId { get; set; }
    }
}
