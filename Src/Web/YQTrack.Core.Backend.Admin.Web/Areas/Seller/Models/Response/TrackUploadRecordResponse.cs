namespace YQTrack.Core.Backend.Admin.Web.Areas.Seller.Models.Response
{
    public class TrackUploadRecordResponse
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { set; get; }
        /// <summary>
        /// 总行数
        /// </summary>
        public string RowTotal { set; get; }
        /// <summary>
        /// 成功行数
        /// </summary>
        public string SuccessTotal { set; get; }
        /// <summary>
        /// 错误行数
        /// </summary>
        public string ErrorTotal { set; get; }
        /// <summary>
        /// 错误详细信息。Json数据
        /// </summary>
        public string ErrorDetail { set; get; }
    }
}
