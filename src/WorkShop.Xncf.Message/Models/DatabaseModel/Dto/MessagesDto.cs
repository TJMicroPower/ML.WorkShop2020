
using Senparc.Ncf.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkShop.Xncf.Message.Models.DatabaseModel.Dto
{
    public class MessagesDto : DtoBase
    {
        public MessagesDto()
        {
        }

        public MessagesDto(string id, string title, string content, int method, DateTime sendTime, int type, int status)
        {
            Id = id;
            Title = title;
            Content = content;
            Method = method;
            SendTime = sendTime;
            Type = type;
            Status = status;
        }

        public string Id { get; set; }

        /// <summary>
        /// 消息标题
        /// </summary>
        [MaxLength(50)]
        public string Title { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [MaxLength(500)]
        public string Content { get; set; }

        /// <summary>
        /// 发送方式
        /// </summary>
        public int Method { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 发送形式
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 发送状态
        /// </summary>
        public int Status { get; set; }

    }
}
