using Senparc.CO2NET.Trace;
using Senparc.Ncf.Core.Enums;
using Senparc.Ncf.Repository;
using Senparc.Ncf.Service;
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
        public MessageDetailService(IRepositoryBase<MessageDetail> repo, IServiceProvider serviceProvider) : base(repo, serviceProvider)
        {
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

    }

}
