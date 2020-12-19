
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
    /// MessageDetail 实体类
    /// </summary>
    [Table(Register.DATABASE_PREFIX + nameof(MessageDetail))]//必须添加前缀，防止全系统中发生冲突
    [Serializable]
    public class MessageDetail : EntityBase<string>
    {
        public MessageDetail()
        {
            Id = Guid.NewGuid().ToString();
            AddTime = DateTime.Now;
            this.LastUpdateTime = AddTime;
        }

        public MessageDetail(MessageDetailDto messageDetailDto) : this()
        {
            LastUpdateTime = messageDetailDto.LastUpdateTime;
            MessageId = messageDetailDto.MessageId;
            UserId = messageDetailDto.UserId;
            IsRead = messageDetailDto.IsRead;
        }

        public void Update(MessageDetailDto messageDetailDto)
        {
            LastUpdateTime = messageDetailDto.LastUpdateTime;
            MessageId = messageDetailDto.MessageId;
            UserId = messageDetailDto.UserId;
            IsRead = messageDetailDto.IsRead;
        }

        /// <summary>
        /// 消息Id
        /// </summary>
        [MaxLength(50)]
        public string MessageId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [MaxLength(50)]
        public string UserId { get; set; }

        /// <summary>
        /// 是否已读(1-已读；2-未读；)
        /// </summary>
        public int IsRead { get; set; }

    }
}
