using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace YQTrack.Core.Backend.Admin.Core
{
    public static class FileHelper
    {
        /// <summary>
        /// 将字符串转换为文件并保存到指定地址
        /// </summary>
        /// <param name="strData">字符串</param>
        /// <param name="savepath">保存地址</param>
        public static void String2File(string strData, string savepath)
        {
            string directory = Path.GetDirectoryName(savepath);
            if (!System.IO.Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            using (FileStream fs = new FileStream(savepath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(strData);
                }
            }
        }

        /// <summary>
        /// 将byte数组转换为文件并保存到指定地址
        /// </summary>
        /// <param name="buff">byte数组</param>
        /// <param name="savepath">保存地址</param>
        public static void Bytes2File(byte[] buff, string savepath)
        {
            string directory = Path.GetDirectoryName(savepath);
            if (!System.IO.Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            using (FileStream fs = new FileStream(savepath, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(buff, 0, buff.Length);
                }
            }
        }

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string ReadFile(string path)
        {
            string ret = string.Empty;
            using (StreamReader myreader = File.OpenText(path))
            {
                ret = myreader.ReadToEnd();//从当前位置读取到文件末尾
            }
            return ret;
        }

        /// <summary>
        /// 将文件转换为byte数组
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <returns>转换后的byte数组</returns>
        public static byte[] File2Bytes(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                return new byte[0];
            }

            FileInfo fi = new FileInfo(path);
            byte[] buff = new byte[fi.Length];
            using (FileStream fs = fi.OpenRead())
            {
                // 设置当前流的位置为流的开始
                fs.Seek(0, SeekOrigin.Begin);
                fs.Read(buff, 0, Convert.ToInt32(fs.Length));
            }

            return buff;
        }

        /// <summary>
        /// 删除文件夹下的所有文件（含此文件夹）
        /// </summary>
        /// <param name="dirRoot"></param>
        public static void DeleteDirAllDirectory(string dirRoot)
        {
            DirectoryInfo aDirectoryInfo = new DirectoryInfo(dirRoot);
            Directory.Delete(aDirectoryInfo.FullName, true);
            //DirectoryInfo[] dirs = aDirectoryInfo.GetDirectories();
            //foreach (DirectoryInfo f in dirs)
            //{
            //    Directory.Delete(f.FullName, true);
            //}
            //FileInfo[] files = aDirectoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
            //foreach (FileInfo f in files)
            //{
            //    File.Delete(f.FullName);
            //}
        }

        /// <summary>
        /// 删除文件夹下的所有文件
        /// </summary>
        /// <param name="dirRoot"></param>
        public static void DeleteDirChildren(string dirRoot)
        {
            DirectoryInfo aDirectoryInfo = new DirectoryInfo(dirRoot);
            DirectoryInfo[] dirs = aDirectoryInfo.GetDirectories();
            foreach (DirectoryInfo f in dirs)
            {
                Directory.Delete(f.FullName, true);
            }
            FileInfo[] files = aDirectoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
            foreach (FileInfo f in files)
            {
                File.Delete(f.FullName);
            }
        }

        /// <summary>
        /// 获取文件MD5值
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(Stream stream)
        {
            try
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(stream);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        /// <summary>
        /// 根据扩展名、ContentType (MIME)、文件头判断是否符合格式
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="fileSize">文件大小限制（MB）</param>
        /// <param name="fileTypes">文件枚举</param>
        /// <param name="isCheckHeader"></param>
        /// <returns></returns>
        public static (bool success, string msg) CheckFile(IFormFile file, int fileSize, bool isCheckHeader, params FileExtension[] fileTypes)
        {
            if (file == null)
            {
                return (false, "文件为空");
            }
            if (file.Length > fileSize * 1024 * 1024)
            {
                return (false, $"文件大小限制{fileSize}MB以内");
            }
            if (fileTypes == null || fileTypes.Length == 0)
            {
                return (false, "请传入文件类型枚举");
            }

            #region 文件后缀
            string fileExt = Path.GetExtension(file.FileName).TrimStart('.');
            if (!fileTypes.Any(a => a.ToString().Equals(fileExt, StringComparison.InvariantCultureIgnoreCase)))
            {
                return (false, "文件格式验证不通过");
            }
            #endregion

            #region 文件ContentType(MIME)
            string contentType = file.ContentType;
            if (!fileTypes.Any(a => a.GetDescription().Equals(contentType, StringComparison.InvariantCultureIgnoreCase)))
            {
                return (false, "文件格式验证不通过");
            }
            #endregion

            #region 文件头判断
            if (isCheckHeader)
            {
                string fileclass = string.Empty;
                using (BinaryReader reader = new BinaryReader(file.OpenReadStream()))
                {
                    byte[] buff = new byte[2];
                    reader.Read(buff, 0, 2);//读取每个文件的头两个字节
                    fileclass = buff[0] + buff[1].ToString();
                }
                if (!fileTypes.Any(a => ((int)a).ToString().Equals(fileclass, StringComparison.InvariantCulture)))
                {
                    return (false, "文件格式验证不通过");
                }
            }
            #endregion

            return (true, "文件格式验证通过");
        }
    }
    /// <summary>
    /// 文件后缀字典
    /// </summary>
    public enum FileExtension
    {
        [Description("image/jpg")]
        jpg = 255216,
        [Description("image/jpeg")]
        jpeg = 255217,
        [Description("image/gif")]
        gif = 7173,
        [Description("application/x-bmp")]
        bmp = 6677,
        [Description("image/png")]
        png = 13780,
        [Description("application/x-msdownload")]
        exe = 7790,
        [Description("application/x-msdownload")]
        dll = 7790,
        [Description("application/octet-stream")]
        rar = 8297,
        [Description("application/x-zip-compressed")]
        zip = 8075,
        [Description("text/xml")]
        xml = 6063,
        [Description("text/html")]
        html = 6033,
        [Description("text/xml")]
        aspx = 239187,
        [Description("text/plain")]
        cs = 117115,
        //[Description("application/x-javascript")]
        [Description("text/javascript")]
        js = 119105,
        [Description("application/json")]
        json = 12310,
        [Description("text/plain")]
        txt = 210187,
        [Description("text/plain")]
        sql = 255254,
        [Description("text/plain")]
        bat = 64101,
        [Description("text/plain")]
        btseed = 10056,
        [Description("text/plain")]
        rdp = 255254,
        [Description("application/octet-stream")]
        psd = 5666,
        [Description("application/pdf")]
        pdf = 3780,
        [Description("application/octet-stream")]
        chm = 7384,
        [Description("text/plain")]
        log = 70105,
        [Description("text/plain")]
        reg = 8269,
        [Description("application/winhlp")]
        hlp = 6395,
        [Description("application/msword")]
        doc = 208207,
        [Description("application/vnd.ms-excel")]
        xls = 208207,
        [Description("application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
        docx = 208207,
        [Description("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
        xlsx = 208207,
    }
}
