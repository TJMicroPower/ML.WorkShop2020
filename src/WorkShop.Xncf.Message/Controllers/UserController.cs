using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Senparc.Ncf.Utility;
using Senparc.Ncf.Core.Models;
using Senparc.CO2NET.Trace;
using AutoMapper;
using System.Net.Http;
using Senparc.CO2NET.HttpUtility;
using WorkShop.Xncf.Message.Services;
using WorkShop.Xncf.Message.Models.DatabaseModel.Dto;

namespace WorkShop.Xncf.Message.Controllers
{
    /// <summary>
    /// 用户接口
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    [ApiVersion("1")]
    public class UserController : BaseController
    {
        private readonly UserService userService;

        public UserController(UserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// 获取OpenId
        /// </summary>
        /// <param name="code">微信小程序Code</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetOpenIdAsync(string code)
        {
            try
            {
                string strAppId = "wx133b351ac060a310";
                string strSecret = "ad86ca4cb847a418346a8f0558bcca14";
                string strRequestUrl = $"https://api.weixin.qq.com/sns/jscode2session?appid={strAppId}&secret={strSecret}&js_code={code}&grant_type=authorization_code";
                HttpClient httpClient = new HttpClient();
                SenparcHttpClient senparcHttpClient = new SenparcHttpClient(httpClient);
                var httpResponse = await senparcHttpClient.Client.GetAsync(strRequestUrl);
                var response = httpResponse.Content.ReadAsStringAsync().Result;
                return Success(response);
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// WX登录
        /// </summary>
        /// <param name="openId">微信OpenId</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> WxLoginAsync(string openId)
        {
            try
            {
                var user = await userService.GetObjectAsync(_ => _.OpenId.Equals(openId));
                if(user != null)
                {
                    return Success(user);
                }
                Random random = new Random();
                int iRan = random.Next(100000, 999999);
                UserDto dto = new UserDto()
                {
                    NickName = $"昵称{iRan}",
                    Account = $"U{iRan}",
                    Password = "123456",
                    Name = $"姓名{iRan}",
                    Gender = "男",
                    Balance = 1000,
                    OpenId = openId
                };
                var response = await userService.CreateOrUpdateAsync(dto);
                return Success(response);
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 宾客登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GuestLoginAsync()
        {
            try
            {
                Random random = new Random();
                int iRan = random.Next(100000, 999999);
                UserDto dto = new UserDto()
                {
                    NickName = $"昵称{iRan}",
                    Account = $"U{iRan}",
                    Password = "123456",
                    Name = $"姓名{iRan}",
                    Gender = "男",
                    Balance = 1000
                };
                var response = await userService.CreateOrUpdateAsync(dto);
                return Success(response);
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }
    }
}
