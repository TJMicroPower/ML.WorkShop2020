
using Senparc.Ncf.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

using WorkShop.Xncf.Message.Models.DatabaseModel.Dto;

namespace WorkShop.Xncf.Message.Models.DatabaseModel
{
    /// <summary>
    /// Messages 实体类
    /// </summary>
    [Table(Register.DATABASE_PREFIX + nameof(Messages))]//必须添加前缀，防止全系统中发生冲突
    [Serializable]
    public class Messages : EntityBase<string>
    {
        public Messages()
        {
            Id = Guid.NewGuid().ToString();
            AddTime = DateTime.Now;
            this.LastUpdateTime = AddTime;
        }

        public Messages(MessagesDto messagesDto) : this()
        {
            LastUpdateTime = messagesDto.LastUpdateTime;
            Title = messagesDto.Title;
            Content = messagesDto.Content;
            Method = messagesDto.Method;
            SendTime = messagesDto.SendTime;
            Type = messagesDto.Type;
            Status = messagesDto.Status;
        }

        public void Update(MessagesDto messagesDto)
        {
            LastUpdateTime = messagesDto.LastUpdateTime;
            Title = messagesDto.Title;
            Content = messagesDto.Content;
            Method = messagesDto.Method;
            SendTime = messagesDto.SendTime;
            Type = messagesDto.Type;
            Status = messagesDto.Status;
        }

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
