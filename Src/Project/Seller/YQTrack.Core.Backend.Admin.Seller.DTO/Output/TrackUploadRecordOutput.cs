namespace YQTrack.Core.Backend.Admin.Seller.DTO.Output
{
    public class TrackUploadRecordOutput
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FFilePath { set; get; }
        /// <summary>
        /// 总行数
        /// </summary>
        public int FRowTotal { set; get; }
        /// <summary>
        /// 成功行数
        /// </summary>
        public int FSuccessTotal { set; get; }
        /// <summary>
        /// 错误行数
        /// </summary>
        public int FErrorTotal { set; get; }
        /// <summary>
        /// 错误详细信息。Json数据
        /// </summary>
        public string FErrorDetail { set; get; }
    }
}
