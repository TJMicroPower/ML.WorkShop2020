
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
    /// User 实体类
    /// </summary>
    [Table(Register.DATABASE_PREFIX + nameof(User))]//必须添加前缀，防止全系统中发生冲突
    [Serializable]
    public class User : EntityBase<string>
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
            AddTime = DateTime.Now;
            this.LastUpdateTime = AddTime;
        }

        public User(UserDto userDto) : this()
        {
            LastUpdateTime = userDto.LastUpdateTime;
            NickName = userDto.NickName;
            Account = userDto.Account;
            Password = userDto.Password;
            Name = userDto.Name;
            Gender = userDto.Gender;
            Balance = userDto.Balance;
            OpenId = userDto.OpenId;
        }

        public void Update(UserDto userDto)
        {
            LastUpdateTime = userDto.LastUpdateTime;
            NickName = userDto.NickName;
            Account = userDto.Account;
            Password = userDto.Password;
            Name = userDto.Name;
            Gender = userDto.Gender;
            Balance = userDto.Balance;
            OpenId = userDto.OpenId;
        }

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
