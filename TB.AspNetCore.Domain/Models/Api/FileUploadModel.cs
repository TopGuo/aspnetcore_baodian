using System;
using System.Collections.Generic;
using System.Text;
using TB.AspNetCore.Domain.Enums;

namespace TB.AspNetCore.Domain.Models.Api
{
    public class FileUploadModel
    {
        /// <summary>
        /// 文件类型
        /// </summary>
        public FileType Type { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 图片base64
        /// </summary>
        public string Picture { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 水印
        /// </summary>
        public string WaterMarks { get; set; }

    }
}
