using System;

namespace YQTrack.Core.Backend.Admin.Core
{
    public class BusinessException : Exception
    {
        public BusinessException(string msg) : base(msg)
        {

        }

        public BusinessException(string argumentName, string argumentValueJson) : base($"参数:{argumentName}值错误,错误值:{argumentValueJson},找不到数据")
        {

        }

        public BusinessException(string msg, Exception innerException) : base(msg, innerException)
        {

        }
    }
}