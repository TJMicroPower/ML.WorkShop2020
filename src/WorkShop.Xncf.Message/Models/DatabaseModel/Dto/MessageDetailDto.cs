
using Senparc.Ncf.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkShop.Xncf.Message.Models.DatabaseModel.Dto
{
    public class MessageDetailDto : DtoBase
    {
        public MessageDetailDto()
        {
        }

        public MessageDetailDto(string id, string messageId, string userId, int isRead)
        {
            Id = id;
            MessageId = messageId;
            UserId = userId;
            IsRead = isRead;
        }

        public string Id { get; set; }

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
