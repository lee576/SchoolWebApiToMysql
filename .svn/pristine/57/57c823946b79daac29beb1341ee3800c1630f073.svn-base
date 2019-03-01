using System;
using System.Collections;

namespace SchoolWebApi.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExceptionExtension
    {
        /// <summary>
        /// 提取异常及其内部异常堆栈跟踪
        /// </summary>
        /// <param name="exception">提取的例外</param>
        /// <param name="lastStackTrace">最后提取的堆栈跟踪（对于递归）， String.Empty or null</param>
        /// <param name="exCount">提取的堆栈数（对于递归）</param>
        /// <returns>Syste.String</returns>
        public static string ExtractAllStackTrace(this Exception exception, string lastStackTrace = null, int exCount = 1)
        {
            var ex = exception;
            //修复最后一个堆栈跟踪参数
            lastStackTrace = lastStackTrace ?? string.Empty;
            //添加异常的堆栈跟踪
            lastStackTrace += $"#{exCount}: {ex.Message}\r\n{ex.StackTrace}";
            if (exception.Data.Count > 0)
            {
                lastStackTrace += "\r\n    Data: ";
                foreach (var item in exception.Data)
                {
                    var entry = (DictionaryEntry)item;
                    lastStackTrace += $"\r\n\t{entry.Key}: {exception.Data[entry.Key]}";
                }
            }
            //递归添加内部异常
            if ((ex = ex.InnerException) != null)
                return ex.ExtractAllStackTrace($"{lastStackTrace}\r\n\r\n", ++exCount);
            return lastStackTrace;
        }
    }
}
