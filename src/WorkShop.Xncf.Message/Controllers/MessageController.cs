using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Senparc.Ncf.Utility;
using Senparc.Ncf.Core.Models;
using Senparc.CO2NET.Trace;
using AutoMapper;
using WorkShop.Xncf.Message.Services;
using WorkShop.Xncf.Message.Models.DatabaseModel;

namespace WorkShop.Xncf.Message.Controllers
{
    /// <summary>
    /// 消息接口
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    [ApiVersion("1")]
    public class MessageController : BaseController
    {
        public readonly MessagesService _messagesService;
        private readonly MessageDetailService messageDetailService;

        public MessageController(MessagesService messagesService, MessageDetailService messageDetailService)
        {
            _messagesService = messagesService;
            this.messageDetailService = messageDetailService;
        }

        /// <summary>
        /// 消息列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ListAsync(string userId, int pageIndex, int pageSize)
        {
            try
            {
                var response = await messageDetailService.ApiGetList(userId, pageIndex, pageSize);
                var messageList = await _messagesService.GetFullListAsync(new SenparcExpressionHelper<Messages>().BuildWhereExpression());
                return Success(new
                {
                    List = from messageDetail in response
                           join message in messageList on messageDetail.MessageId equals message.Id
                           select new
                           {
                               messageDetail.Id,
                               AddTime = $"{string.Format("{0:yyyy-MM-dd HH:mm:ss}", messageDetail.AddTime)}",
                               message.Title,
                               messageDetail.IsRead,
                               message.Content
                           }
                });
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 消息详情
        /// </summary>
        /// <param name="id">消息详情Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DetailAsync(string id)
        {
            try
            {
                var messageDetail = await messageDetailService.ApiGetDetailAsync(id);
                //获取消息的具体内容
                var message = await _messagesService.GetObjectAsync(_ => _.Id.Equals(messageDetail.MessageId));
                return Success(new
                {
                    message.Title,
                    message.Content,
                    id,
                    AddTime = $"{string.Format("{0:yyyy-MM-dd HH:mm:ss}", messageDetail.AddTime)}"
                });
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 清空未读消息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ClearAsync(string userId)
        {
            try
            {
                await messageDetailService.ApiClearNotReadMessage(userId);
                var response = "清空成功";
                return Success(response);
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="messageDetailId">消息详情Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DeleteAsync(string userId, string messageDetailId)
        {
            try
            {
                await messageDetailService.ApiDeleteMessage(userId, messageDetailId);
                var response = "删除成功";
                return Success(response);
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }
    }
}
