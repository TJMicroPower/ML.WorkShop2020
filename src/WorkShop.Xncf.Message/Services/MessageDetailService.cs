using Microsoft.EntityFrameworkCore;
using Senparc.CO2NET.Trace;
using Senparc.Ncf.Core.Enums;
using Senparc.Ncf.Repository;
using Senparc.Ncf.Service;
using Senparc.Ncf.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkShop.Xncf.Message.Models.DatabaseModel;
using WorkShop.Xncf.Message.Models.DatabaseModel.Dto;
namespace WorkShop.Xncf.Message.Services
{
    public class MessageDetailService : ServiceBase<MessageDetail>
    {
        private readonly MessagesService messagesService;

        public MessageDetailService(IRepositoryBase<MessageDetail> repo, IServiceProvider serviceProvider,MessagesService messagesService) : base(repo, serviceProvider)
        {
            this.messagesService = messagesService;
        }

        //TODO: 更多业务方法可以写到这里
        public async Task<IEnumerable<MessageDetailDto>> GetMessageDetailList(int PageIndex, int PageSize)
        {
            List<MessageDetailDto> selectListItems = null;
            List<MessageDetail> messageDetail = (await GetFullListAsync(_ => true).ConfigureAwait(false)).OrderByDescending(_ => _.AddTime).ToList();
            selectListItems = this.Mapper.Map<List<MessageDetailDto>>(messageDetail);
            return selectListItems;
        }

        public async Task CreateOrUpdateAsync(MessageDetailDto dto)
        {
            MessageDetail messageDetail;
            if (String.IsNullOrEmpty(dto.Id))
            {
                messageDetail = new MessageDetail(dto);
            }
            else
            {
                messageDetail = await GetObjectAsync(_ => _.Id == dto.Id);
                messageDetail.Update(dto);
            }
            await SaveObjectAsync(messageDetail);
        }

        #region 后台
        public async Task InsertMessageUserAsync(string userId, string content)
        {
            var strategy = base.BaseData.BaseDB.BaseDataContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await BeginTransactionAsync(async () =>
                {

                    MessagesDto messagesDto = new MessagesDto()
                    {
                        Title = content,
                        Content = content,
                        Method = 1,
                        Status = 2
                    };
                    var messageInfo = await messagesService.CreateOrUpdateAsync(messagesDto);
                    MessageDetailDto messageDetailDto = new MessageDetailDto()
                    {
                        MessageId = messageInfo.Id,
                        UserId = userId,
                        IsRead = 2
                    };
                    await CreateOrUpdateAsync(messageDetailDto);
                });
            });
        }

        public async Task InsertMessageUserAsync(string messageId, string[] saRelationUser)
        {
            //判断是否有关联的产品，如果有，则直接写入
            if (saRelationUser.Length > 0)
            {
                List<MessageDetail> lstMsgDetail = new List<MessageDetail>();
                foreach (string item in saRelationUser)
                {
                    MessageDetail msgDetail = new MessageDetail();
                    msgDetail.MessageId = messageId;
                    msgDetail.UserId = item;
                    msgDetail.IsRead = 2;
                    lstMsgDetail.Add(msgDetail);
                }
                await base.SaveObjectListAsync(lstMsgDetail);
            }
        }
        #endregion

        #region 接口
        /// <summary>
        /// 获取消息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MessageDetailDto>> ApiGetList(string userId, int pageIndex, int pageSize)
        {
            List<MessageDetailDto> selectListItems = null;
            var seh = new SenparcExpressionHelper<MessageDetail>();
            SenparcTrace.Log($"userId-----{userId}");
            seh.ValueCompare.AndAlso(!string.IsNullOrEmpty(userId), _ => _.UserId.Equals(userId));
            var where = seh.BuildWhereExpression();
            List<MessageDetail> messageDetails = (await base.GetObjectListAsync(pageIndex, pageSize, where, "AddTime Desc")).ToList();
            selectListItems = base.Mapper.Map<List<MessageDetailDto>>(messageDetails);
            return selectListItems;
        }

        /// <summary>
        /// 获取消息详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MessageDetailDto> ApiGetDetailAsync(string id)
        {
            //获取用户当前的的消息
            var seh = new SenparcExpressionHelper<MessageDetail>();
            seh.ValueCompare.AndAlso(!string.IsNullOrEmpty(id), _ => _.Id.Equals(id));
            var where = seh.BuildWhereExpression();
            var response = await base.GetObjectAsync(where);
            //更新消息是否已读
            MessageDetailDto messageDetailDto = Mapper.Map<MessageDetailDto>(response);
            messageDetailDto.IsRead = 1;
            await CreateOrUpdateAsync(messageDetailDto);
            return messageDetailDto;
        }

        /// <summary>
        /// 清除未读消息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> ApiClearNotReadMessage(string userId)
        {
            var seh = new SenparcExpressionHelper<MessageDetail>();
            SenparcTrace.Log($"userId-----{userId}");
            seh.ValueCompare.AndAlso(!string.IsNullOrEmpty(userId), _ => _.UserId.Equals(userId));
            seh.ValueCompare.AndAlso(true, _ => _.IsRead.Equals(2));
            var where = seh.BuildWhereExpression();
            List<MessageDetail> lstMsgDetails = (await base.GetFullListAsync(where, "AddTime Desc")).ToList();
            for (int i = 0; i < lstMsgDetails.Count; i++)
            {
                lstMsgDetails[i].IsRead = 1;
            }
            await base.SaveObjectListAsync(lstMsgDetails);
            return true;
        }

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="messageDetailId"></param>
        /// <returns></returns>
        public async Task<bool> ApiDeleteMessage(string userId, string messageDetailId)
        {
            var seh = new SenparcExpressionHelper<MessageDetail>();
            SenparcTrace.Log($"userId-----{userId},messageDetailId----${messageDetailId}");
            seh.ValueCompare.AndAlso(!string.IsNullOrEmpty(userId), _ => _.UserId.Equals(userId));
            seh.ValueCompare.AndAlso(!string.IsNullOrEmpty(messageDetailId), _ => _.Id.Equals(messageDetailId));
            var where = seh.BuildWhereExpression();
            await base.DeleteAllAsync(where);
            return true;
        }
        #endregion
    }

}
