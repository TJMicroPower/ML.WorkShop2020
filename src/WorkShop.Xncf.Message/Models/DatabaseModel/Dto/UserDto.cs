
using Senparc.Ncf.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkShop.Xncf.Message.Models.DatabaseModel.Dto
{
    public class UserDto : DtoBase
    {
        public UserDto()
        {
        }

        public UserDto(string id, string nickName, string account, string password, string name, string gender, decimal balance, string openId)
        {
            Id = id;
            NickName = nickName;
            Account = account;
            Password = password;
            Name = name;
            Gender = gender;
            Balance = balance;
            OpenId = openId;
        }

        public string Id { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        [MaxLength(50)]
        public string NickName { get; set; }

        /// <summary>
        /// 用户账户
        /// </summary>
        [MaxLength(50)]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [MaxLength(50)]
        public string Password { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [MaxLength(50)]
        public string Gender { get; set; }

        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 微信OpenId
        /// </summary>
        [MaxLength(50)]
        public string OpenId { get; set; }

    }
}
